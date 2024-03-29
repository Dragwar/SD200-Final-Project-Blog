﻿using System;
using System.Collections.Generic;

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
        public DateTime? DateUpdated { get; set; }
        public string HeroImageUrl { get; set; }
        public string Slug { get; set; }

        public virtual List<Comment> Comments { get; set; }

        public Post()
        {
            Id = Guid.NewGuid();
            DateCreated = DateTime.Now;
            Comments = new List<Comment>();
        }
    }
}