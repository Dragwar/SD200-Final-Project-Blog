using SD200_Final_Project_Blog.Models.Domain;
using SD200_Final_Project_Blog.MyHelpers;
using System;
using System.Text.RegularExpressions;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class IndexPostViewModel
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string HeroImageUrl { get; set; }
        public string PostAuthorName { get; set; }

        public int CommentCount { get; set; }

        public string Slug { get; set; }


        public string GetMiniBody(int textLength)
        {
            string output = Body.GetPlainTextFromHtml();
            output = output.Substring(0, output.Length <= textLength ? output.Length : textLength);
            return output;
        }

        /// <summary>
        /// Retrieves all post information from passed in post
        /// returns the built up PostViewModel from the passed in parameter (Post object)
        /// </summary>
        /// <param name="post">Retrieves all post information from this</param>
        /// <returns>IndexPostViewModel</returns>
        public static IndexPostViewModel CreateIndexPostViewModel(Post post)
        {
            IndexPostViewModel indexPostViewModel = new IndexPostViewModel()
            {
                Id = post.Id,
                PostAuthorName = post.User == null ? "Anonymous User" : post.User.UserName,
                Title = post.Title,
                Slug = post.Slug,
                Body = post.Body,
                DateCreated = post.DateCreated,

                // if DateUpdated is null then it will default to show DateCreated (not modifying actual Post)
                DateUpdated = (DateTime)(post.DateUpdated.HasValue ? post.DateUpdated : post.DateCreated),
                Published = post.Published,
                HeroImageUrl = post.HeroImageUrl,
                CommentCount = post.Comments.Count,
            };
            return indexPostViewModel;
        }
    }
}