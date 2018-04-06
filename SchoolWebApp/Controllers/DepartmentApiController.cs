using SchoolWebApp.ApiModels;
using SchoolWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace SchoolWebApp.Controllers
{
    public class DepartmentApiController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/DepartmentApi
        public IEnumerable<DepartmentApiModel> Get()
        {
            // You may use AutoMapper here
            var list = new List<DepartmentApiModel>();
            foreach (var item in db.Departments)
            {
                list.Add(new DepartmentApiModel { Id = item.Id, Name = item.Name });
            }
            return list;
        }

        // GET: api/DepartmentApi/5
        [ResponseType(typeof(DepartmentApiModel))]
        public IHttpActionResult Get(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            var model = new DepartmentApiModel { Id = department.Id, Name = department.Name };
            return Ok(model);
        }

        // POST: api/DepartmentApi
        [ResponseType(typeof(DepartmentApiModel))]
        public IHttpActionResult PostDepartment(DepartmentApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Department department = new Department { Name = model.Name };
            db.Departments.Add(department);
            db.SaveChanges();

            // Get the new Id from the database
            model.Id = department.Id;

            // Return status code 201 success with creation
            return CreatedAtRoute("DefaultApi", new { id = department.Id }, model);
        }

        // This equivalent to UPDATE Operation
        // PUT: api/DepartmentApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(int id, DepartmentApiModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.Id)
            {
                return BadRequest();
            }

            var department = new Department { Id = model.Id, Name = model.Name };
            db.Entry(department).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // DELETE: api/DepartmentApi/5
        [ResponseType(typeof(DepartmentApiModel))]
        public IHttpActionResult Delete(int id)
        {
            Department department = db.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            db.Departments.Remove(department);
            db.SaveChanges();

            var model = new DepartmentApiModel { Id = department.Id, Name = department.Name };
            return Ok(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DepartmentExists(int id)
        {
            return db.Departments.Count(e => e.Id == id) > 0;
        }
    }
}
