using SchoolWebApp.Models;
using System;
using System.Collections.Generic;
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

        // List of faculties in this department
        public List<Faculty> Faculties { get; set; }
    }
}