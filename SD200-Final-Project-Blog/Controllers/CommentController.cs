using SD200_Final_Project_Blog.Models;
using SD200_Final_Project_Blog.Models.Domain;
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


            Post foundPost = DbContext.Posts.FirstOrDefault(post => post.Comments.Any(comment => comment.Id == id.Value));

            if (foundPost == null)
            {
                return RedirectToAction(nameof(HomeController.Index), nameof(HomeController));
            }


            Comment foundComment = foundPost.Comments.FirstOrDefault(comment => comment.Id == id.Value);

            if (foundComment == null)
            {
                return Redirect(Url.Action(nameof(HomeController.PostBySlug), "Home", new { slug = foundPost.Slug }));
            }


            foundPost.Comments.Remove(foundComment);
            DbContext.SaveChanges();

            return Redirect(Url.Action(nameof(HomeController.PostBySlug), "Home", new { slug = foundPost.Slug }));
        }


    }
}