using System;
using System.ComponentModel.DataAnnotations;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class CreateCommentViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Body { get; set; }
    }
}