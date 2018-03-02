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
    public class EmployeeController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public EmployeeController()
        {
        }

        public EmployeeController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

        // GET: Employee
        public ActionResult Index()
        {
            var users = db.Employees.ToList();
            var model = new List<EmployeeViewModel>();
            foreach (var item in users)
            {
                model.Add(new EmployeeViewModel
                {
                    Id = item.Id,
                    Email = item.Email,
                    FirstName = item.FirstName,
                    LastName = item.LastName
                });
            }
            return View(model);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {
            // Find the user then cast her to employee
            var user = (Employee) UserManager.FindById(id);
            EmployeeViewModel model = Mapper.Map<EmployeeViewModel>(user);
            return View(model);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            //TODO: Add this list to EmployeeViewModel as a propeerty
            //ViewBag.RoleId = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(EmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Employee
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();

        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var user = (Employee)UserManager.FindById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            EmployeeViewModel model = Mapper.Map<EmployeeViewModel>(user);
            return View(model);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EmployeeViewModel model)
        {
            // Exclude Password and ConfirmPassword properties from the model otherwise modelstate is false
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                var user = (Employee)UserManager.FindById(id);
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Email;
                var result = UserManager.Update(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            var user = (Employee) UserManager.FindById(id);
            EmployeeViewModel model = Mapper.Map<EmployeeViewModel>(user);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // POST: Employee/Delete/5
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
