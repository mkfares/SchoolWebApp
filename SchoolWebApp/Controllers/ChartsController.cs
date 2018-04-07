using SchoolWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Controllers
{
    public class ChartsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Charts
        public ActionResult Index()
        {
            // This chart displays on
            // x-axes: Department names that contain one or more faculties
            // y-axes: Number of faculties in each department

            var departments = db.Departments.ToList();
            var faculties = db.Faculties;
            int count = 0;
            var labels = new List<string>();
            var data = new List<int>();
            foreach (var item in departments)
            {
                // Find the number of faculties in the current department
                count = faculties.Count(m => m.Department.Id == item.Id);
                if(count != 0)
                {
                    labels.Add(item.Name);
                    data.Add(count);
                }
            }

            // Convert labels and data from lists to arrays and save them in ViewBag
            ViewBag.Labels = labels.ToArray();
            ViewBag.Data = data.ToArray();

            return View();
        }
    }
}