using SchoolWebApp.Models;
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
        [DisplayFormat(NullDisplayText = "No Description")]
        public string Description { get; set; }

        // The outline file as object (used to upload a file)
        [Display(Name ="Outline File")]
        public HttpPostedFileBase Outline { get; set; }

        // The outline file path as string (used to diplay the path)
        public string OutlineFilePath { get; set; }

        // List of faculties teaching this course
        public List<Faculty> Faculties { get; set; }
    }
}