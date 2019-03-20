using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD200_Final_Project_Blog.MyHelpers
{
    [NotMapped]
    public static class DateHelper
    {
        public static string GetCreatedDateAndTime(this DateTime DateCreated) => $"{DateCreated.ToString("m") + ", " + DateCreated.ToShortTimeString()} | {DateCreated.Year}";

        public static string GetCreatedDateTimeFromNow(this DateTime DateCreated)
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
    }
}