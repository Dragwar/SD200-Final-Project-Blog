﻿using Microsoft.AspNet.Identity;
using SD200_Final_Project_Blog.Models;
using SD200_Final_Project_Blog.Models.Domain;
using SD200_Final_Project_Blog.Models.ViewModels;
using SD200_Final_Project_Blog.MyHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SD200_Final_Project_Blog.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext DbContext { get; set; }
        public static readonly HashSet<string> AllowedFileExtensions = new HashSet<string> { ".jpg", ".jpeg", ".png", ".gif" };
        private const string UserImageUploadFolderRelativePath = @"~/UserUploads/PostHeroImages/";
        public static readonly HashSet<char> MyUnwantedSymbols = new HashSet<char>()
        {'.', '!', '*', '\'', '"', '`', '(', ')', ';', ':', '@', '&', '=', '+', '$', ',', '/', '|', '\\', '?', '%', '#', '[', ']', '{', '}'};

        public HomeController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            // Just for finding which header nav to bold
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Index);

            List<IndexPostViewModel> model;

            if (DbContext.Posts.Any())
            {
                model = DbContext.Posts
                    .ToList() // need to retrieve the list before using this method below
                    .Select(post => IndexPostViewModel.CreateIndexPostViewModel(post))
                    .ToList();

                // Get descending order by the dateCreated for model (most recent posts is first oldest posts are last)
                model.Sort((postA, postB) => postB.DateCreated.CompareTo(postA.DateCreated));

                // Filter out unpublished posts when user isn't admin
                if (!User.IsInRole(nameof(UserRolesEnum.Admin)))
                {
                    model = model.Where(post => post.Published).ToList();
                }

            }
            else
            {
                model = new List<IndexPostViewModel>();
            }

            return View(model);
        }

        // JUST USING INDEX PAGE TO SHOW ALL POSTS FOR NOW
        //public ActionResult Blog()
        //{
        //    /// <summary>
        //    ///     Just for finding which header nav to bold
        //    /// </summary>
        //    /// <variable name="CurrentControllerMethodName">Holds the method name (view name)</variable>
        //    ViewBag.CurrentControllerMethodName = nameof(HomeController.Blog);

        //    return View();
        //}

        [HttpGet]
        public ActionResult Post(Guid? id, string commentError)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }


            if (!string.IsNullOrEmpty(commentError) && !string.IsNullOrWhiteSpace(commentError))
            {
                ModelState.AddModelError("", commentError);
            }

            // Just for finding which header nav to bold
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Post);

            Post foundPost = null;
            PostViewModel model = null;

            if (DbContext.Posts.Any())
            {
                foundPost = DbContext.Posts.FirstOrDefault(post => post.Id == id);

                if (foundPost != null)
                {
                    List<IndexPostViewModel> latestPosts = DbContext.Posts
                        .Where(post => post.Id != foundPost.Id)
                        .ToList() // need to retrieve the list before using this method below
                        .Select(post => IndexPostViewModel.CreateIndexPostViewModel(post))
                        .ToList();

                    // Get descending order by the dateCreated for model (most recent posts is first oldest posts are last)
                    latestPosts.Sort((postA, postB) => postB.DateCreated.CompareTo(postA.DateCreated));
                    latestPosts = latestPosts.Take(3).ToList();

                    // Filter out unpublished posts when user isn't admin
                    if (!User.IsInRole(nameof(UserRolesEnum.Admin)))
                    {
                        latestPosts = latestPosts.Where(post => post.Published).ToList();
                    }

                    model = PostViewModel.CreatePostViewModel(foundPost, latestPosts, commentError);

                    // sort comments by creation date
                    model.Comments.Sort((commentA, commentB) => commentB.DateCreated.CompareTo(commentA.DateCreated));
                }
            }

            if (foundPost == null || model == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            return View(model);
        }

        [HttpGet]
        [Route("Blog/{slug}")]
        public ActionResult PostBySlug(string slug, string commentError)
        {
            if (string.IsNullOrEmpty(slug) || string.IsNullOrWhiteSpace(slug))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            if (!string.IsNullOrEmpty(commentError) && !string.IsNullOrWhiteSpace(commentError))
            {
                ModelState.AddModelError("", commentError);
            }

            // Just for finding which header nav to bold
            ViewBag.CurrentControllerMethodName = nameof(HomeController.Post);

            Post foundPost = null;
            PostViewModel model = null;

            if (DbContext.Posts.Any())
            {
                foundPost = DbContext.Posts.FirstOrDefault(post => post.Slug == slug);

                if (foundPost != null)
                {
                    List<IndexPostViewModel> latestPosts = DbContext.Posts
                        .Where(post => post.Id != foundPost.Id)
                        .ToList() // need to retrieve the list before using this method below
                        .Select(post => IndexPostViewModel.CreateIndexPostViewModel(post))
                        .ToList();

                    // Get descending order by the dateCreated for model (most recent posts is first oldest posts are last)
                    latestPosts.Sort((postA, postB) => postB.DateCreated.CompareTo(postA.DateCreated));
                    latestPosts = latestPosts.Take(3).ToList();

                    // Filter out unpublished posts when user isn't admin
                    if (!User.IsInRole(nameof(UserRolesEnum.Admin)))
                    {
                        latestPosts = latestPosts.Where(post => post.Published).ToList();
                    }

                    model = PostViewModel.CreatePostViewModel(foundPost, latestPosts, commentError);                    

                    // sort comments by creation date
                    model.Comments.Sort((commentA, commentB) => commentB.DateCreated.CompareTo(commentA.DateCreated));
                }
            }

            if (foundPost == null || model == null)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            return View(nameof(HomeController.Post), model);
        }

        [HttpGet]
        [Authorize(Roles = nameof(UserRolesEnum.Admin))]
        public ActionResult CreatePost()
        {
            ViewBag.CurrentControllerMethodName = nameof(HomeController.CreatePost);
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


            CreateEditPostViewModel model = CreateEditPostViewModel.CreateCreateEditPostViewModel(foundPost);

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

            if (DbContext.Posts.Any(post => post.Title == model.Title && (!id.HasValue || post.Id != id.Value)))
            {
                ModelState.AddModelError(nameof(CreateEditPostViewModel.Title), "Post title should be unique");

                return View();
            }


            string fileExtension;
            string serverMapPath = Server.MapPath(UserImageUploadFolderRelativePath);

            // Validating file upload
            if (model.HeroImage != null)
            {
                fileExtension = Path.GetExtension(model.HeroImage.FileName);

                if (!AllowedFileExtensions.Contains(fileExtension.ToLower()))
                {
                    ModelState.AddModelError("", "File extension is not allowed.");
                    return View();
                }
            }

            Post myPost;

            if (!id.HasValue)
            {
                // make slug
                string slug = model.Title.GenerateSlug(MyUnwantedSymbols);

                myPost = new Post()
                {
                    Slug = slug,
                    UserId = currentUserId,

                    // Should redirect to index if user doesn't exist (because only admin users can create posts)
                    User = DbContext.Users.FirstOrDefault(user => user.Id == currentUserId),
                };

                // if user isn't logged in and some how tries to create a post
                if (myPost.User == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }

                if (string.IsNullOrEmpty(slug) || string.IsNullOrEmpty(slug))
                {
                    myPost.Slug = $"post-{myPost.Id.GenerateRandomStringFromGuid(8)}";
                }

                // check if slug is already taken
                bool slugAlreadyTaken = DbContext.Posts.Any(post => post.Slug == slug);

                if (slugAlreadyTaken)
                {
                    // generate a unique slug by appending part of the post's Guid
                    slug += $"-{new string(myPost.Id.ToString().Skip(9).Take(4).ToArray())}";
                    myPost.Slug = slug;
                }

                DbContext.Posts.Add(myPost);
            }
            else
            {
                myPost = DbContext.Posts.FirstOrDefault(post => post.Id == id.Value);
                myPost.DateUpdated = DateTime.Now;

                if (myPost == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }

            myPost.Title = model.Title;
            myPost.Body = model.Body;
            myPost.Published = model.Published;


            // Handling file upload
            if (model.HeroImage != null)
            {
                if (!Directory.Exists(serverMapPath))
                {
                    Directory.CreateDirectory(serverMapPath);
                }

                string fileName = model.HeroImage.FileName;
                string fullPathWithName = serverMapPath + fileName;

                model.HeroImage.SaveAs(fullPathWithName);

                myPost.HeroImageUrl = UserImageUploadFolderRelativePath + fileName;
            }


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

            Post foundPost = DbContext.Posts.FirstOrDefault(post => post.Id == id);

            if (foundPost == null)
            {
                RedirectToAction(nameof(HomeController.Index));
            }

            // remove comments before deleting actual post
            // this will avoid deletion error
            DbContext.Comments.RemoveRange(foundPost.Comments);

            DbContext.Posts.Remove(foundPost);
            DbContext.SaveChanges();
            return RedirectToAction(nameof(HomeController.Index));
        }

        [HttpGet]
        public ActionResult SearchPosts()
        {
            ViewBag.CurrentControllerMethodName = nameof(HomeController.SearchPosts);

            List<IndexPostViewModel> emptyModel = new List<IndexPostViewModel>();

            return View(emptyModel);
        }

        [HttpPost]
        public ActionResult SearchPosts(string userSearch)
        {
            if (string.IsNullOrEmpty(userSearch) || string.IsNullOrWhiteSpace(userSearch))
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            ViewBag.CurrentControllerMethodName = nameof(HomeController.SearchPosts);

            string query = userSearch.ToLower().Trim();

            List<IndexPostViewModel> model = DbContext.Posts
                .Where(post => (
                    post.Title.ToLower().Contains(query) ||
                    post.Slug.ToLower().Contains(query) ||
                    post.Body.ToLower().Contains(query)
                )).ToList() // need to retrieve the list before using this method below
                .Select(post => IndexPostViewModel.CreateIndexPostViewModel(post))
                .ToList();

            ViewBag.userSearch = userSearch;

            // If no posts were found
            if (!model.Any())
            {
                ModelState.AddModelError("", $@"No results found for ""{userSearch}""");
                return View(model);
            }

            // Filter out unpublished posts when user isn't admin
            if (!User.IsInRole(nameof(UserRolesEnum.Admin)))
            {
                model = model.Where(post => post.Published).ToList();
            }

            return View(model);
        }
    }
}
