using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    [Table("Faculty")]
    public class Faculty : Employee
    {
        public Faculty()
        {
            Courses = new HashSet<Course>();
        }

        public string Speciality { get; set; }

        public FacultyLevel Level { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }

    public enum FacultyLevel
    {
        Lecturer,
        AssitantProfessor,
        AssociateProfessor,
        Professor
    }
}