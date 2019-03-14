using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SD200_Final_Project_Blog.Models.Domain
{
    public class Post
    {
        public Guid Id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string HeroImageUrl { get; set; }

        //public string GenerateSlug()
        //{
        //    NumberFormatInfo formatInfo = NumberFormatInfo.CurrentInfo;
        //    //formatInfo.
        //    string slug = Title;
        //    string symbols = @"[^\w\.@-]";
        //    //symbols += 
        //    slug =
        //    slug = Regex.Replace(slug, symbols, "");
        //    slug = Regex.Replace(slug, @"^\s+$[\r\n]*", "", RegexOptions.Multiline);
        //    return slug;
        //}
    }
}