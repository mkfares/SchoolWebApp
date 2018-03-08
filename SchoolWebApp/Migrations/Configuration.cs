namespace SchoolWebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using SchoolWebApp.Models;
    using System;
    using System.Data.Entity.Migrations;

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

            //NOTE Add department sample
            CreateDepartment(context);
        }

        private void CreateDepartment(ApplicationDbContext context)
        {
            context.Departments.AddOrUpdate(
                  p => p.Name, // Use name instead of Id (Id does not work since it is an identity - generated automatically by the database)
                  new Department { Name = "CS" },
                  new Department { Name = "MIS" },
                  new Department { Name = "HR" },
                  new Department { Name = "IT" }
                  );
        }
    }
}
