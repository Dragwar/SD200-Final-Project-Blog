﻿using System;

namespace SD200_Final_Project_Blog.Models.Domain
{
    public class Post
    {
        public Guid Id { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual string UserId { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public bool Published { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}