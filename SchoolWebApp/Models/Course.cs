using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    [Table("Course")]
    public class Course
    {
        public Course()
        {
            Faculties = new HashSet<Faculty>();
        }
        public int CourseId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Faculty> Faculties { get; set; }
    }
}