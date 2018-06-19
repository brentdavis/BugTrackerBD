namespace BugTrackerBD.Migrations
{
    using BugTrackerBD.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTrackerBD.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTrackerBD.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            var userManager = new UserManager<ApplicationUser>(
               new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(u => u.Email == "johnmahoney@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "johnmahoney@Mailinator.com",
                    Email = "johnmahoney@Mailinator.com",
                    FirstName = "John",
                    LastName = "Mahoney",
                    DisplayName = "johnm47"
                }, "Lenny2261#");
            }

            if (!context.Users.Any(u => u.Email == "brentdavis56@gmail.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "brentdavis56@gmail.com",
                    Email = "brentdavis56@gmail.com",
                    FirstName = "Brent",
                    LastName = "Davis",
                    DisplayName = "bd56"
                }, "Sheeps23#");
            }

            if (!context.Users.Any(u => u.Email == "jerhudson47@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "jerhudson47@Mailinator.com",
                    Email = "jerhudson47@Mailinator.com",
                    FirstName = "Jeremy",
                    LastName = "Hudson",
                    DisplayName = "jerh47"
                }, "Lenny2261#");
            }

            if (!context.Users.Any(u => u.Email == "benabsher77@Mailinator.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "benabsher77@Mailinator.com",
                    Email = "benabsher77@Mailinator.com",
                    FirstName = "Ben",
                    LastName = "Absher",
                    DisplayName = "benab77"
                }, "Lenny2261#");
            }

            var userId = userManager.FindByEmail("brentdavis56@gmail.com").Id;
            userManager.AddToRole(userId, "Admin");
            userId = userManager.FindByEmail("johnmahoney@Mailinator.com").Id;
            userManager.AddToRole(userId, "ProjectManager");
            userId = userManager.FindByEmail("jerhudson47@Mailinator.com").Id;
            userManager.AddToRole(userId, "Developer");
            userId = userManager.FindByEmail("benabsher77@Mailinator.com").Id;
            userManager.AddToRole(userId, "Submitter");

            context.AttachmentTypes.AddOrUpdate(
               t => t.Type,
                   new AttachmentType { Type = "default"},
                   new AttachmentType { Type = "txt"},
                   new AttachmentType { Type = "doc"},
                   new AttachmentType { Type = "docx"},
                   new AttachmentType { Type = "pdf"},
                   new AttachmentType { Type = "xls"},
                   new AttachmentType { Type = "xlsx"},
                   new AttachmentType { Type = "jpg"},
                   new AttachmentType { Type = "gif"},
                   new AttachmentType { Type = "png"},
                   new AttachmentType { Type = "tiff"}
           );

        }
    }
}
