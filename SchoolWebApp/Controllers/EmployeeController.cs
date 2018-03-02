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
        // Example of displaying custom error view (Views/Shared/Error.cshtml) when id is null
        // The is parameter changed from int to int? to accept nulls
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                // Convert id to int instead of int?
                int userId = id ?? default(int);

                // find the user in the database
                var user = UserManager.FindById(userId);

                // Check if the user exists and it is an emplyee not a simple application user
                if (user != null && user is Employee)
                {
                    user = (Employee)user;
                    EmployeeViewModel model = Mapper.Map<EmployeeViewModel>(user);
                    ViewBag.Roles = UserManager.GetRoles(userId).ToList();

                    return View(model);
                }
                else
                {
                    // Customize the error view: /Views/Shared/Error.cshtml
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            // Example of usage of a checkbox list. See the /Views/Employee/Create.cshtml view
            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeViewModel model, params string[] roles)
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
                    if (roles != null)
                    {
                        // Add user to selected roles
                        var roleResult = UserManager.AddToRoles(user.Id, roles);

                        if (roleResult.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            // Display error messages in the view @Html.ValidationSummary()
                            ModelState.AddModelError(string.Empty, roleResult.Errors.First());

                            // Create a check list object
                            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");

                            // Return a view if you want to see error message saved in ModelState
                            // Redirect() and RedirectToAction() clear the messages
                            return View();
                        }
                    }
                }
                else
                {
                    // See above comment for ModelState errors
                    ModelState.AddModelError(string.Empty, result.Errors.First());
                    ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
                    return View();
                }
            }

            ViewBag.Roles = new SelectList(db.Roles.ToList(), "Name", "Name");
            return View();

        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id != null)
            {
                var userId = id ?? default(int);

                var user = (Employee)UserManager.FindById(userId);
                if (user == null)
                {
                    //return HttpNotFound();
                    return View("Error");
                }

                EmployeeViewModel model = Mapper.Map<EmployeeViewModel>(user);

                var userRoles = UserManager.GetRoles(userId);
                var rolesSelectList = db.Roles.ToList().Select(r => new SelectListItem()
                {
                    Selected = userRoles.Contains(r.Name),
                    Text = r.Name,
                    Value = r.Name
                });

                ViewBag.RolesSelectList = rolesSelectList;

                return View(model);
            }

            //return HttpNotFound();
            return View("Error");
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, EmployeeViewModel model, params string[] roles)
        {
            // Exclude Password and ConfirmPassword properties from the model otherwise modelstate is false
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid && id != null)
            {

                // Convert id to non-nullable int
                var userId = id ?? default(int);

                var user = (Employee)UserManager.FindById(userId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                //TODO: Check the usage of AutoMapper in this case (AM creates a new object that is not attached)
                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.UserName = model.Email;
                var userResult = UserManager.Update(user);

                if (userResult.Succeeded)
                {
                    var userRoles = UserManager.GetRoles(user.Id);
                    roles = roles ?? new string[] { };
                    var roleResult = UserManager.AddToRoles(user.Id, roles.Except(userRoles).ToArray<string>());

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, roleResult.Errors.First());
                        return View();
                    }

                    roleResult = UserManager.RemoveFromRoles(user.Id, userRoles.Except(roles).ToArray<string>());

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError(string.Empty, roleResult.Errors.First());
                        return View();
                    }

                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var userId = id ?? default(int);
                var user = (Employee)UserManager.FindById(userId);
                if (user == null)
                {
                    return HttpNotFound();
                }

                EmployeeViewModel model = Mapper.Map<EmployeeViewModel>(user);
                return View(model);
            }

            return HttpNotFound();
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid && id != null)
            {
                var userId = id ?? default(int);
                var user = UserManager.FindById(userId);
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
