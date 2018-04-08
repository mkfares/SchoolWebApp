using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolWebApp.Models
{
    [Table("Department")]
    public class Department
    {
        public Department()
        {
            Faculties = new HashSet<Faculty>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

        public virtual ICollection<Faculty> Faculties { get; set; }
    }
}