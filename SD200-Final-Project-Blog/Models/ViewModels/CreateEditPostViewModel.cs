using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SD200_Final_Project_Blog.Models.ViewModels
{
    public class CreateEditPostViewModel
    {
        [Required(ErrorMessage = "You must provide a Post Title")]
        [StringLength(maximumLength: 25)]
        public string Title { get; set; }

        [Required]
        [AllowHtml]
        public string Body { get; set; }

        public HttpPostedFileBase HeroImage { get; set; }

        public bool Published { get; set; }
    }
}