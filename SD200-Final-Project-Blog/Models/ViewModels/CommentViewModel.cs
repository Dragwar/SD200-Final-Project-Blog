using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class CommentViewModel
    {
        public Guid? Id { get; set; }

        public string CommentAuthorName { get; set; }

        public string Body { get; set; }
        public string UpdatedReason { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

    }
}