using System;

namespace SD200_Final_Project_Blog.Controllers
{
    public class CreateEditCommentViewModel
    {
        public Guid? Id { get; set; }

        public string CommentAuthorName { get; set; }
        public string UserId { get; set; }

        public string Body { get; set; }
        public string UpdatedReason { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}