using SchoolWebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebApp.ViewModels
{
    public class DepartmentViewModel
    {
        public DepartmentViewModel()
        {
            Faculties = new List<Faculty>();
        }
        public int Id { get; set; }
        public string Name { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        // List of faculties in this department
        public List<Faculty> Faculties { get; set; }
    }
}