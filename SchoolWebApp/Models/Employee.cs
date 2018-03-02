using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    [Table("Employee")]
    public class Employee : ApplicationUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}