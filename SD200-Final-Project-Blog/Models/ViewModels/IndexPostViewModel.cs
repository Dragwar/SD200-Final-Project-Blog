using SD200_Final_Project_Blog.Models.Domain;
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

        private string GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            htmlString = Regex.Replace(htmlString, htmlTagPattern, "");
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
            return htmlString;
        }

        public string GetPostTimeFromNow()
        {
            int postDay = DateTime.Today.Day - DateCreated.Day;
            TimeSpan postTime = DateTime.Now.TimeOfDay - DateCreated.TimeOfDay;
            if (postDay >= 1)
            {
                return $"{postDay} {(postTime.Days == 0 ? "day" : "days")} ago";
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

        public string GetMiniBody(int textLength)
        {
            string output = GetPlainTextFromHtml(Body);
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
                DateUpdated = post.DateUpdated,
                Published = post.Published,
                HeroImageUrl = post.HeroImageUrl,
                CommentCount = post.Comments.Count,
            };
            return indexPostViewModel;
        }
    }
}