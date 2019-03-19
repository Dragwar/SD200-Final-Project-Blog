using SD200_Final_Project_Blog.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public List<PostCommentViewModel> Comments { get; set; }
        public int CommentCount { get => Comments.Count; }

        public string GetPostTimeFromNow()
        {
            int postDay = DateTime.Today.Day - DateCreated.Day;
            TimeSpan postTime = DateTime.Now.TimeOfDay - DateCreated.TimeOfDay;
            if (postDay >= 1)
            {
                return $"{postDay} {(postTime.Hours == 1 ? "day" : "days")} ago";
            }
            else if (postDay == 0 && postTime.Hours >= 1)
            {
                return $"{postTime.Hours} {(postTime.Hours == 1 ? "hour" : "hours")} ago";
            }
            else if (postTime.Hours == 0 && postTime.Minutes >= 1)
            {
                return $"{postTime.Minutes} {(postTime.Minutes == 1 ? "minute" : "minutes")} ago";
            }
            else if (postTime.Minutes == 0 && postTime.Seconds >= 1)
            {
                return $"{postTime.Seconds} {(postTime.Seconds == 1 ? "second" : "seconds")} ago";
            }
            else
            {
                return $"Just Posted";
            }
        }

        public string GetPostDateAndTime() => $"{DateCreated.ToString("m") + ", " + DateCreated.ToShortTimeString()} | {DateCreated.Year}";

        /// <summary>
        /// Retrieves all post information from passed in post
        /// and this needs latestPosts as well to display along side the Main Post
        /// returns the built up PostViewModel from the two passed in parameters
        /// </summary>
        /// <param name="post">Retrieves all post information from this</param>
        /// <param name="latestPosts"></param>
        /// <returns>PostViewModel</returns>
        public static PostViewModel CreatePostViewModel(Post post, List<IndexPostViewModel> latestPosts)
        {
            PostViewModel postViewModel = new PostViewModel()
            {
                Id = post.Id,
                PostAuthorName = post.User == null ? "" : post.User.UserName,
                Title = post.Title,
                Body = post.Body,
                DateCreated = post.DateCreated,
                DateUpdated = post.DateUpdated,
                Published = post.Published,
                HeroImageUrl = post.HeroImageUrl,

                Comments = post.Comments
                            .Select(comment => new PostCommentViewModel()
                            {
                                Id = comment.Id,
                                CommentAuthorName = comment.User == null ? "" : comment.User.UserName,
                                Body = comment.Body,
                                DateCreated = comment.DateCreated,
                                DateUpdated = comment.DateUpdated,
                                UpdatedReason = comment.UpdatedReason,
                            }).ToList(),

                // Get three latest posts (without the current post)
                LatestPosts = latestPosts,
            };
            return postViewModel;
        }
    }
}