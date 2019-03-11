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
                    //.Where(post => post.UserId == userId)
                    .Select(post => new IndexPostViewModel
                    {
                        Id = post.Id,
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

        public ActionResult Post()
        {
            /// <summary>
            ///     Just for finding which header nav to bold
            /// </summary>
            /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Post);

            return View();
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



        private ActionResult SavePost(Guid? id, CreateEditPostViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Get current user id
            //string userId = User.Identity.GetUserId();

            if (DbContext.Posts.Any(post => (
                //post.UserId == userId && 
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