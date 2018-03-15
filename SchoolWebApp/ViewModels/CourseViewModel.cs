using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebApp.ViewModels
{
    public class CourseViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        // The outline file
        [Display(Name ="Outline File")]
        public HttpPostedFileBase Outline { get; set; }

        // The outline file path
        public string OutlineFilePath { get; set; }
    }
}