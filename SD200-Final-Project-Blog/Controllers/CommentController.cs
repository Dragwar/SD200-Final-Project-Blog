using Microsoft.AspNet.Identity;
using SD200_Final_Project_Blog.Models;
using SD200_Final_Project_Blog.Models.Domain;
using SD200_Final_Project_Blog.Models.ViewModels;
using SD200_Final_Project_Blog.MyHelpers;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SD200_Final_Project_Blog.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }

        public CommentController()
        {
            DbContext = new ApplicationDbContext();
        }

        [HttpPost]
        // Roles: don't allow $"" syntax for some reason...
        [Authorize(Roles = nameof(UserRolesEnum.Admin) + "," + nameof(UserRolesEnum.Moderator))]
        public ActionResult DeleteComment(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            // finding the comment using the post because I want to redirect back to that post via it's slug
            Post foundPost = DbContext.Posts.FirstOrDefault(post => post.Comments.Any(comment => comment.Id == id.Value));

            if (foundPost == null)
            {
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
            }

            // I could've used:
            // "Comment foundComment = DbContext.Comments.FirstOrDefault(/*compare ids*/);" 
            // to get the comment as well 
            // (I wanted to redirect back to the post where said comment was on)
            Comment foundComment = foundPost.Comments.FirstOrDefault(comment => comment.Id == id.Value);

            if (foundComment == null)
            {
                return Redirect(Url.Action(nameof(HomeController.PostBySlug), "Home", new { slug = foundPost.Slug }));
            }


            DbContext.Comments.Remove(foundComment);
            DbContext.SaveChanges();

            return Redirect(Url.Action(nameof(HomeController.PostBySlug), "Home", new { slug = foundPost.Slug }));
        }

        [HttpPost]
        public ActionResult CreateComment(Guid? postId, CreateCommentViewModel newComment)
        {
            if (!postId.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            string currentUserId = User.Identity.GetUserId();

            Post foundPost = DbContext.Posts.FirstOrDefault(post => post.Id == postId);

            if (foundPost == null)
            {
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
            }

            Comment myComment = new Comment()
            {
                Body = newComment.Body,
                UserId = currentUserId,
                User = DbContext.Users.FirstOrDefault(user => user.Id == currentUserId),
            };

            foundPost.Comments.Add(myComment);
            DbContext.SaveChanges();

            return Redirect(Url.Action(nameof(HomeController.PostBySlug), "Home", new { slug = foundPost.Slug }));
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRolesEnum.Admin) + "," + nameof(UserRolesEnum.Moderator))]
        public ActionResult EditComment(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            EditCommentViewModel foundComment = DbContext.Comments
                .Select(comment => new EditCommentViewModel()
                {
                    Id = comment.Id,
                    Body = comment.Body,

                    // reset reason to "" because I want the moderator to type the reason each comment edit
                    UpdatedReason = "",

                    CommentAuthorName = comment.User.UserName,
                }).FirstOrDefault(comment => comment.Id == id.Value);


            return View(foundComment);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRolesEnum.Admin) + "," + nameof(UserRolesEnum.Moderator))]
        public ActionResult EditComment(Guid? id, EditCommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!id.HasValue || model == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            Comment foundComment = DbContext.Comments.FirstOrDefault(comment => comment.Id == id.Value);

            Post foundPost = DbContext.Posts.FirstOrDefault(post => post.Comments.Any(comment => comment.Id == id.Value));

            if (foundComment == null)
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foundComment.DateUpdated = DateTime.Now;
            foundComment.Body = model.Body;
            foundComment.UpdatedReason = $"edited by {User.Identity.GetUserName()}: {model.UpdatedReason}";

            DbContext.SaveChanges();

            return Redirect(Url.Action(nameof(HomeController.PostBySlug), "Home", new { slug = foundPost.Slug }));
        }
    }
}