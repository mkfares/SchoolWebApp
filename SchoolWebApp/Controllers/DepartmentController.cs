﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using SchoolWebApp.Models;
using SchoolWebApp.ViewModels;

namespace SchoolWebApp.Controllers
{
    public class DepartmentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Department
        /// <summary>
        /// List all departments
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            
            var departments = db.Departments.ToList();
          
            var model = new List<DepartmentViewModel>();
            foreach (var item in departments)
            {
                model.Add(new DepartmentViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreationDate = item.CreationDate,
                    StartTime = item.StartTime,
                });
            }
            return View(model);
        }

        // GET: Department/Details/5
        /// <summary>
        /// Get department details
        /// </summary>
        /// <param name="id">Department Id</param>
        /// <returns>Details view</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            DepartmentViewModel model = Mapper.Map<Department, DepartmentViewModel>(department);

            return View(model);
        }

        // GET: Department/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Department/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Example on how to check the data in the view model
                if (model.CreationDate < DateTime.Today.AddDays(-30))
                {
                    // The message will be displayed in @Html.ValidationMessageFor(m=>m.CreationDate)
                    ModelState.AddModelError("CreationDate", "The creation date should not be older than 30 days");

                    // The message will be displayed in @Html.ValidationSummary()
                    ModelState.AddModelError(String.Empty, "Issue with the creation date");

                    // Return the model with the entered data to be corrected
                    return View(model);
                }
                Department department = Mapper.Map<DepartmentViewModel, Department>(model);

                db.Departments.Add(department);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Department/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }

            DepartmentViewModel model = Mapper.Map<Department, DepartmentViewModel>(department);

            return View(model);
        }

        // POST: Department/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Department department = Mapper.Map<DepartmentViewModel, Department>(model);
                db.Entry(department).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Department/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            DepartmentViewModel model = Mapper.Map<DepartmentViewModel>(department);

            return View(model);
        }

        // POST: Department/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Department department = db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // GET: /Department/ListFaculties/5
        public PartialViewResult ListFacultiesPartial(int id)
        {
            var users = db.Faculties.Where(d => d.DepartmentId == id).ToList();
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
                    Department = user.Department.Name,
                });
            }

            return PartialView(model);
        }

        public ActionResult GetCountFacultiesPartial(int id)
        {
            // Modify the condition inside the Count() to suite your needs
            int count = db.Faculties.Count(p => p.DepartmentId == id);
            return PartialView(count);
        }
    }
}
