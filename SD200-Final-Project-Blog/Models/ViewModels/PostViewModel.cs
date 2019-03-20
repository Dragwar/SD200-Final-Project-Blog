using SD200_Final_Project_Blog.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            LatestPosts = new List<IndexPostViewModel>();
        }
        public List<IndexPostViewModel> LatestPosts { get; set; }

        public Guid? Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string HeroImageUrl { get; set; }
        public string PostAuthorName { get; set; }

        public List<CommentViewModel> Comments { get; set; }
        public int CommentCount { get => Comments.Count; }

        public string CommentError { get; set; }

        /// <summary>
        /// Retrieves all post information from passed in post
        /// and this needs latestPosts as well to display along side the Main Post
        /// returns the built up PostViewModel from the two passed in parameters
        /// </summary>
        /// <param name="post">Retrieves all post information from this</param>
        /// <param name="latestPosts"></param>
        /// <returns>PostViewModel</returns>
        public static PostViewModel CreatePostViewModel(Post post, List<IndexPostViewModel> latestPosts, string commentError)
        {
            PostViewModel postViewModel = new PostViewModel()
            {
                Id = post.Id,
                PostAuthorName = post.User == null ? "" : post.User.UserName,
                Title = post.Title,
                Body = post.Body,
                DateCreated = post.DateCreated,

                // if DateUpdated is null then it will default to show DateCreated (not modifying actual Post)
                DateUpdated = (DateTime)(post.DateUpdated.HasValue ? post.DateUpdated : post.DateCreated),
                Published = post.Published,
                HeroImageUrl = post.HeroImageUrl,

                CommentError = commentError,
                Comments = post.Comments
                    .Select(comment => new CommentViewModel()
                    {
                        Id = comment.Id,
                        CommentAuthorName = comment.User == null ? "" : comment.User.UserName,
                        Body = comment.Body,
                        DateCreated = comment.DateCreated,

                        // if DateUpdated is null then it will default to show DateCreated (not modifying actual Comment)
                        DateUpdated = (DateTime)(post.DateUpdated.HasValue ? post.DateUpdated : post.DateCreated),
                        UpdatedReason = comment.UpdatedReason,
                    }).ToList(),

                // Get three latest posts (without the current post)
                LatestPosts = latestPosts,
            };
            return postViewModel;
        }
    }
}