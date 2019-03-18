using System;
using System.ComponentModel.DataAnnotations;

namespace SD200_Final_Project_Blog.Controllers
{
    public class CreateEditCommentViewModel
    {
        public Guid? Id { get; set; }

        public string CommentAuthorName { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public string UpdatedReason { get; set; }
    }
}