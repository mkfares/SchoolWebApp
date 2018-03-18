using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Course
        public ActionResult Index()
        {
            var courses = db.Courses.ToList();

            var model = new List<CourseViewModel>();
            foreach (var item in courses)
            {
                model.Add(new CourseViewModel
                {
                    Id = item.CourseId,
                    Code = item.Code,
                    Title = item.Title,
                    Description = item.Description
                });
            }
            return View(model);
        }

        // GET: Course/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            var model = new CourseViewModel
            {
                Id = course.CourseId,
                Code = course.Code,
                Title = course.Title,
                Description = course.Description,
                OutlineFilePath = course.OutlineFilePath,
                Faculties = course.Faculties.ToList()
            };

            return View(model);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        public ActionResult Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Create the course from the model
                var course = new Course
                {
                    Code = model.Code,
                    Title = model.Title,
                    Description = model.Description
                };

                //TODO Remove invalid characters from the filename such as white spaces
                // check if the uplaoded file is empty (do not upload empty files)
                if (model.Outline != null && model.Outline.ContentLength > 0)
                {
                    // Allowed extensions to be uploaded
                    var extensions = new[] { "pdf", "docx", "doc" };

                    // using System.IO for Path class
                    // Get the file name without the path
                    string filename = Path.GetFileName(model.Outline.FileName);

                    // Get the extension of the file
                    string ext = Path.GetExtension(filename).Substring(1);

                    // Check if the extension of the file is in the list of allowed extensions
                    if (!extensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                    {
                        ModelState.AddModelError(string.Empty, "Accepted file are pdf, docx, and doc documents");
                        return View();
                    }

                    // Set the application folder where to save the uploaded file
                    string appFolder = "~/Content/Uploads/";

                    // Generate a random string to add to the file name
                    // This is to avoid the files with the same names
                    var rand = Guid.NewGuid().ToString();

                    // Combine the application folder location with the file name
                    string path = Path.Combine(Server.MapPath(appFolder), rand + "-" + filename);

                    // Save the file in ~/Content/Uploads/filename.xyz
                    model.Outline.SaveAs(path);

                    // Add the path to the course object
                    course.OutlineFilePath = appFolder + rand + "-" + filename;

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Empty files are not accepted");
                    return View();
                }

                // Save the created course to the database
                db.Courses.Add(course);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Course/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Course/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
