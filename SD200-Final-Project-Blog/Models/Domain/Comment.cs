using System;

namespace SD200_Final_Project_Blog.Models.Domain
{
    public class Comment
    {
        public Guid Id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }

        public string Body { get; set; }
        public string UpdatedReason { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public Comment()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.Now;
        }
    }
}