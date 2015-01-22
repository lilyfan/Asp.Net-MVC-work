using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Validation;

namespace SchoolNotification.Models
{
    public class DataContext : DbContext//IdentityDbContext<ApplicationUser>
    {
        public DataContext() : base("name = DefaultConnection") { }

        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            System.Diagnostics.Debug.WriteLine("Here2 ");
            // Change the name of the table to be Users instead of AspNetUsers
            modelBuilder.Entity<IdentityUser>()
                .ToTable("Users");
            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");
            modelBuilder.Entity<Faculty>().ToTable("Faculties");
        }*/

        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Admin> Admins { get; set; }
        //public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Models.Course> Courses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Models.Cancellation> Cancellations { get; set; }
        public DbSet<Message> Messages { get; set; }

        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.AdminBase> AdminBases { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.AdminFull> AdminFulls { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.FacultyBase> FacultyBases { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.FacultyFull> FacultyFulls { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.FacultyCreate> FacultyCreates { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.StudentBase> StudentBases { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.StudentFull> StudentFulls { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.StudentCreate> StudentCreates { get; set; }

        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.CourseBase> CourseBases { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.CourseFull> CourseFulls { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.CourseCreate> CourseCreates { get; set; }

        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.CancellationBase> CancellationBases { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.CancellationFull> CancellationFulls { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.CancellationCreate> CancellationCreates { get; set; }

        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.MessageBase> MessageBases { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.MessageFull> MessageFulls { get; set; }
        public System.Data.Entity.DbSet<SchoolNotification.ViewModels.MessageCreate> MessageCreates { get; set; }

    }

    public partial class DataContextEntities : DbContext
    {
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}