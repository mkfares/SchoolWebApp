using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolWebApp.Controllers
{
    /// <summary>
    /// Faculty controller manage faculies using faculty and FacultyViewModel classes
    /// This is an example of controller without error checking
    /// See the Employee controller for advanced error checking
    /// </summary>
    public class FacultyController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public FacultyController()
        {
        }

        public FacultyController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public object UserManeger { get; private set; }

        // GET: Faculty
        public ActionResult Index()
        {
            var users = db.Faculties.ToList();
            var model = new List<FacultyViewModel>();
            foreach (var user in users)
            {
                model.Add(new FacultyViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Speciality = user.Speciality,
                    Level = user.Level,
                });
            }
            return View(model);
        }

        // GET: Faculty/Details/5
        public ActionResult Details(int id)
        {

            // find the user in the database
            var user = UserManager.FindById(id);

            // Check if the user exists
            if (user != null)
            {
                var faculty = (Faculty)user;

                FacultyViewModel model = new FacultyViewModel()
                {
                    Id = faculty.Id,
                    Email = faculty.Email,
                    FirstName = faculty.FirstName,
                    LastName = faculty.LastName,
                    Speciality = faculty.Speciality,
                    Level = faculty.Level,
                    Roles = string.Join(" ", UserManager.GetRoles(id).ToArray())
                };

                return View(model);
            }
            else
            {
                // Customize the error view: /Views/Shared/Error.cshtml
                return View("Error");
            }
        }

        // GET: Faculty/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Faculty/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FacultyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var faculty = new Faculty
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Speciality = model.Speciality,
                    Level = model.Level,
                };

                var result = UserManager.Create(faculty, model.Password);

                if (result.Succeeded)
                {
                    //TODO Add user to faculty role (check if Faculty role exists)
                    var roleResult = UserManager.AddToRoles(faculty.Id, "Faculty");

                    if (roleResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        // Display error messages in the view @Html.ValidationSummary()
                        ModelState.AddModelError(string.Empty, roleResult.Errors.First());

                        return View();
                    }
                }
                else
                {
                    // Display error messages in the view @Html.ValidationSummary()
                    ModelState.AddModelError(string.Empty, result.Errors.First());
                    return View();
                }
            }

            return View();

        }

        // GET: Faculty/Edit/5
        public ActionResult Edit(int id)
        {

            var faculty = (Faculty)UserManager.FindById(id);
            if (faculty == null)
            {
                //return HttpNotFound();
                return View("Error");
            }

            FacultyViewModel model = new FacultyViewModel
            {
                Id = faculty.Id,
                Email = faculty.Email,
                FirstName = faculty.FirstName,
                LastName = faculty.LastName,
                Speciality = faculty.Speciality,
                Level = faculty.Level,
                Roles = string.Join(" ", UserManager.GetRoles(id).ToArray())
            };

            return View(model);
        }

        // POST: Faculty/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FacultyViewModel model)
        {
            // Exclude Password and ConfirmPassword properties from the model otherwise modelstate is false
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                var faculty = (Faculty)UserManager.FindById(id);
                if (faculty == null)
                {
                    return HttpNotFound();
                }

                // Edit the faculty info
                faculty.Email = model.Email;
                faculty.FirstName = model.FirstName;
                faculty.LastName = model.LastName;
                faculty.UserName = model.Email;
                faculty.Speciality = model.Speciality;
                faculty.Level = model.Level;

                var userResult = UserManager.Update(faculty);

                if (userResult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // GET: Faculty/Delete/5
        public ActionResult Delete(int id)
        {
            var faculty = (Faculty)UserManager.FindById(id);
            if (faculty == null)
            {
                return HttpNotFound();
            }

            FacultyViewModel model = Mapper.Map<FacultyViewModel>(faculty);
            //TODO replace Automapper
            return View(model);
        }

        // POST: Faculty/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                var user = UserManager.FindById(id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                var result = UserManager.Delete(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return View();
        }
    }
}
