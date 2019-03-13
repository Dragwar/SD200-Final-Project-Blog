using Microsoft.AspNet.Identity;
using SD200_Final_Project_Blog.Models;
using SD200_Final_Project_Blog.Models.Domain;
using SD200_Final_Project_Blog.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SD200_Final_Project_Blog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }

        public HomeController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            /// <summary>
            ///     Just for finding which header nav to bold
            /// </summary>
            /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Index);

            // Get current user id
            //string userId = User.Identity.GetUserId();

            List<IndexPostViewModel> model;

            if (DbContext.Posts.Any())
            {
                model = DbContext.Posts
                    .Select(post => new IndexPostViewModel
                    {
                        Id = post.Id,
                        PostAuthorName = post.User.UserName,
                        Title = post.Title,
                        Body = post.Body,
                        DateCreated = post.DateCreated,
                        DateUpdated = post.DateUpdated,
                        Published = post.Published,
                    }).ToList();

                // Get descending order by the dateCreated for model (most recent posts is first oldest posts are last)
                model.Sort((postA, postB) => postB.DateCreated.CompareTo(postA.DateCreated));
            }
            else
            {
                model = new List<IndexPostViewModel>();
            }

            return View(model);
        }

        public ActionResult Blog()
        {
            /// <summary>
            ///     Just for finding which header nav to bold
            /// </summary>
            /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Blog);

            return View();
        }

        [HttpGet]
        public ActionResult Post(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            /// <summary>
            ///     Just for finding which header nav to bold
            /// </summary>
            /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Post);

            Post foundPost = null;
            PostViewModel model = null;

            if (DbContext.Posts.Any())
            {
                foundPost = DbContext.Posts
                    .FirstOrDefault(post => post.Id == id);
                if (foundPost != null)
                {
                    List<IndexPostViewModel> allPosts = DbContext.Posts
                        .Select(post => new IndexPostViewModel()
                        {
                            Id = post.Id,
                            Title = post.Title,
                            PostAuthorName = post.User == null ? "Anonymous User" : post.User.UserName,
                            Body = post.Body,
                            DateCreated = post.DateCreated,
                            DateUpdated = post.DateUpdated,
                            Published = post.Published,
                        }).ToList();

                    // Get descending order by the dateCreated for model (most recent posts is first oldest posts are last)
                    allPosts.Sort((postA, postB) => postB.DateCreated.CompareTo(postA.DateCreated));

                    model = new PostViewModel()
                    {
                        Id = foundPost.Id,
                        PostAuthorName = foundPost.User == null ? "" : foundPost.User.UserName,
                        Title = foundPost.Title,
                        Body = foundPost.Body,
                        DateCreated = foundPost.DateCreated,
                        DateUpdated = foundPost.DateUpdated,
                        Published = foundPost.Published,

                        // Get three latest posts (without the current post)
                        LatestPosts = allPosts.Where(post => post.Id != foundPost.Id).Take(3).ToList(),
                    };

                }
            }

            if (foundPost == null || model == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRolesEnum.Admin))]
        public ActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRolesEnum.Admin))]
        public ActionResult CreatePost(CreateEditPostViewModel createModel)
        {
            return SavePost(null, createModel);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRolesEnum.Admin))]
        public ActionResult EditPost(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            Post foundPost = DbContext.Posts.FirstOrDefault(post => post.Id == id.Value);

            if (foundPost == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }


            CreateEditPostViewModel model = new CreateEditPostViewModel
            {

                Title = foundPost.Title,
                Body = foundPost.Body,
                Published = foundPost.Published,
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRolesEnum.Admin))]
        public ActionResult EditPost(Guid id, CreateEditPostViewModel createModel)
        {
            return SavePost(id, createModel);
        }

        private ActionResult SavePost(Guid? id, CreateEditPostViewModel model)
        {
            string currentUserId = User.Identity.GetUserId();

            if (!ModelState.IsValid)
            {
                return View();
            }

            if (DbContext.Posts.Any(post => (
                post.Title == model.Title && (!id.HasValue || post.Id != id.Value)
            )))
            {
                ModelState.AddModelError(nameof(CreateEditPostViewModel.Title), "Post title should be unique");

                return View();
            }

            Post myPost;

            if (!id.HasValue)
            {
                myPost = new Post()
                {
                    Id = Guid.NewGuid(),
                    UserId = currentUserId,

                    // Should break if user doesn't exist (because only admin users can create posts)
                    User = DbContext.Users.First(user => user.Id == currentUserId),
                    DateCreated = DateTime.Now,
                };
                DbContext.Posts.Add(myPost);
            }
            else
            {
                myPost = DbContext.Posts.FirstOrDefault(post => post.Id == id.Value);

                if (myPost == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }

            myPost.Title = model.Title;
            myPost.Body = model.Body;
            myPost.DateUpdated = DateTime.Now;

            DbContext.SaveChanges();

            return RedirectToAction(nameof(HomeController.Index));
        }

        [HttpPost]
        [Authorize(Roles = nameof(UserRolesEnum.Admin))]
        public ActionResult DeletePost(Guid? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            Post foundPost = DbContext.Posts.First(post => post.Id == id);

            DbContext.Posts.Remove(foundPost);
            DbContext.SaveChanges();
            return RedirectToAction(nameof(HomeController.Index));
        }
    }
}