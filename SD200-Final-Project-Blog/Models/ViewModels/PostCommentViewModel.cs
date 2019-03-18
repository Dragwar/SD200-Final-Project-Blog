using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class PostCommentViewModel
    {
        public Guid? Id { get; set; }

        public string AuthorName { get; set; }

        public string Body { get; set; }
        public string UpdatedReason { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public string GetCommentPostTimeFromNow()
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

        public string GetCommentPostDateAndTime() => $"{DateCreated.ToString("m") + ", " + DateCreated.ToShortTimeString()} | {DateCreated.Year}";
    }
}