using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class CreateEditPostViewModel
    {
        [Required(ErrorMessage = "You must provide a Post Title")]
        [StringLength(maximumLength: 25)]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }


        public bool Published { get; set; }
    }
}