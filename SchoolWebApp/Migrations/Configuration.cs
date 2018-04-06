namespace SchoolWebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using SchoolWebApp.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //TODO Define roles to add to your app, keep the Admin role first
            string[] roles = { "Admin", "Faculty", "Staff" };

            //TODO Change admin user login information
            string adminEmail = "admin@mail.edu";
            string adminUserName = "admin@mail.edu";
            string adminPassword = "Admin1!";


            // Create roles
            var roleStore = new CustomRoleStore(context);
            var roleManager = new RoleManager<CustomRole, int>(roleStore);

            foreach (var role in roles)
            {
                if (!roleManager.RoleExists(role))
                {
                    roleManager.Create(new CustomRole { Name = role });
                }
            }

            // Define admin user
            var userStore = new CustomUserStore(context);
            var userManager = new ApplicationUserManager(userStore);

            //TODO Change the type of the admin user
            var admin = new ApplicationUser
            {
                UserName = adminUserName,
                Email = adminEmail,
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            // Create admin user
            if (userManager.FindByName(admin.UserName) == null)
            {
                userManager.Create(admin, adminPassword);
            }

            // Add admin user to admin role
            // roles[0] is "Admin"
            var user = userManager.FindByName(admin.UserName);
            if (!userManager.IsInRole(user.Id, roles[0]))
            {
                userManager.AddToRole(admin.Id, roles[0]);
            }

            // Add examples of departments
            var departments = new List<Department>
            {
                  new Department { Name = "CS" },
                  new Department { Name = "MIS" },
                  new Department { Name = "HR" },
                  new Department { Name = "IT" }
            };

            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            // Add examples of employees
            var employees = new List<Employee>
            {
                new Employee {UserName = "emp1", Email = "emp1@g.com", FirstName = "Emp1", LastName = "Emp11" },
                new Employee {UserName = "emp2", Email = "emp2@g.com", FirstName = "Emp2", LastName = "Emp22" },
                new Employee {UserName = "emp3", Email = "emp3@g.com", FirstName = "Emp3", LastName = "Emp33" },
                new Employee {UserName = "emp4", Email = "emp4@g.com", FirstName = "Emp4", LastName = "Emp33" },
            };

            foreach (var employee in employees)
            {
                if (userManager.FindByName(employee.UserName) == null)
                {
                    userManager.Create(employee, "emp123");
                }

                // Employees are not added to any role
            }

            // Add examples of faculties
            var faculties = new List<Faculty>
            {
                new Faculty { UserName = "fac1", Email ="fac1@g.ocm", FirstName ="Fac1", LastName ="Fac11",
                    Speciality = "Database", Level =FacultyLevel.Lecturer,
                    DepartmentId = departments.Single(d=>d.Name=="CS").Id },
                new Faculty { UserName = "fac2", Email ="fac2@g.ocm", FirstName ="Fac2", LastName ="Fac22",
                    Speciality = "Information Systems", Level =FacultyLevel.AssitantProfessor,
                    DepartmentId = departments.Single(d=>d.Name=="MIS").Id },
                new Faculty { UserName = "fac3", Email ="fac3@g.ocm", FirstName ="Fac3", LastName ="Fac33",
                    Speciality = "Networks", Level =FacultyLevel.Lecturer,
                    DepartmentId = departments.Single(d=>d.Name=="IT").Id },
            };

            foreach (var faculty in faculties)
            {
                if (userManager.FindByName(faculty.UserName) == null)
                {
                    userManager.Create(faculty, "fac123");
                }

                var usertemp = userManager.FindByName(faculty.UserName);
                if (!userManager.IsInRole(usertemp.Id, roles[1]))
                {
                    userManager.AddToRole(usertemp.Id, roles[1]);
                }
            }

            // Add examples of courses
            // NOTE: Check how to initialize a property of type collection: Faculties
            var courses = new List<Course>
            {
                new Course { Code = "BBIS101", Title = "Intro to Computers", Faculties = new List<Faculty>() },
                new Course { Code = "BBIS102", Title = "Programming 1", Faculties = new List<Faculty>() },
                new Course { Code = "BBIS103", Title = "Data Structures", Faculties = new List<Faculty>() },
                new Course { Code = "BBIS201", Title = "Programming 2", Faculties = new List<Faculty>() },
                new Course { Code = "BBIS301", Title = "Operating Systems", Faculties = new List<Faculty>() },
                new Course { Code = "BBIS401", Title = "Software Engineering", Faculties = new List<Faculty>() }
            };

            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.Code, s));
            context.SaveChanges();

            // Assign faculties to courses
            AssignFacultyToCourse(context, courseCode: "BBIS101", facultyLastName: "Fac11"); // Example of named parameters
            AssignFacultyToCourse(context, "BBIS101", "Fac22");
            AssignFacultyToCourse(context, "BBIS102", "Fac11");
            AssignFacultyToCourse(context, "BBIS103", "Fac22");
            AssignFacultyToCourse(context, "BBIS301", "Fac33");

            context.SaveChanges();

        }
        void AssignFacultyToCourse(ApplicationDbContext context, string courseCode, string facultyLastName)
        {
            var course = context.Courses.SingleOrDefault(c => c.Code == courseCode);
            var faculty = course.Faculties.SingleOrDefault(f => f.LastName == facultyLastName);
            //TODO Check the test
            if (faculty != null)
                course.Faculties.Add(context.Faculties.Single(f => f.LastName == facultyLastName));
        }
    }
}
