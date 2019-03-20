using System;
using System.ComponentModel.DataAnnotations;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class CreateCommentViewModel
    {
        public Guid? PostId { get; set; }

        public Guid? Id { get; set; }

        [Required]
        public string Body { get; set; }

        public string ErrorMessage { get; set; }
    }
}