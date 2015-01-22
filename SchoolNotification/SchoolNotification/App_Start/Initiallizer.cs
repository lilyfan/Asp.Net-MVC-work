using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SchoolNotification.Models;
using WebMatrix.WebData;
using System.Web.Security;
using System.Data.Entity.Migrations;

namespace SchoolNotification.Models
{
    public class Initiallizer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext dc)
        {
            System.Diagnostics.Debug.WriteLine("Initiallizer Seed Here! ");
            base.Seed(dc);
            InitializeIdentityForEF(dc);

        }
        private void InitializeIdentityForEF(DataContext dc)
        {
            System.Diagnostics.Debug.WriteLine("Initiallizer InitializeIdentityForEF Here! ");

            //Initialize Course
            Course int422a = new Course();
            int422a.CourseName = "Windows Web Programming";
            int422a.CourseCode = "INT422A";
            int422a.Room = "T3074";
            int422a.Date= "Wednesday";
            int422a.Time="11:40 - 13:25";
            Course jac444b = new Course("Java Programming", "JAC444B", "T2018", "Thurday","8:00 - 9:45");
            Course sys466a = new Course("Analysis and Design using OO Models", "SYS466A", "S1028", "Monday",  "9:50 - 11:35");
            Course dcn455c = new Course("Data Communications Networks", "DCN455C", "S1069", "Monday", "13:30 - 13:15");


            WebSecurity.InitializeDatabaseConnection("DefaultConnection",
                   "UserProfile", "UserId", "UserName", autoCreateTables: true);

            //Create roles
            string[] roleNames = new string[] { "Admin", "Faculty", "Student" };
            var roles = (SimpleRoleProvider)Roles.Provider;
            for (var i = 0; i < roleNames.Length; i++)
            {
                if (!roles.RoleExists(roleNames[i])) { roles.CreateRole(roleNames[i]); }
            }

            var membership = (SimpleMembershipProvider)Membership.Provider;
            //Create User=admin with password=123456
            if (membership.GetUser("admin", false) == null)
            {
                membership.CreateUserAndAccount("admin", "123456", new Dictionary<string, object> { { "FirstName", "Linpei" }, { "LastName", "Fan" }, { "EmployeeId", "087654321" }, { "Discriminator", "Admin" } });
            }
            if (!roles.GetRolesForUser("admin").Contains("Admin")) { roles.AddUsersToRoles(new[] { "admin" }, new[] { "Admin" }); }

            //Create Faculty
            //fac1
            if (membership.GetUser("Mark", false) == null)
            {
                membership.CreateUserAndAccount("Mark", "123456", new Dictionary<string, object> { { "FirstName", "Mark" }, { "LastName", "Fernandes" }, { "FacultyId", "011111111" }, { "Discriminator", "Faculty" } });
            }
            if (!roles.GetRolesForUser("Mark").Contains("Faculty")) { roles.AddUsersToRoles(new[] { "Mark" }, new[] { "Faculty" }); }

            //fac2
            if (membership.GetUser("Tom", false) == null)
            {
                membership.CreateUserAndAccount("Tom", "123456", new Dictionary<string, object> { { "FirstName", "Tom" }, { "LastName", "White" }, { "FacultyId", "011111112" }, { "Discriminator", "Faculty" } });
            }
            if (!roles.GetRolesForUser("Tom").Contains("Faculty")) { roles.AddUsersToRoles(new[] { "Tom" }, new[] { "Faculty" }); }

            //fac3
            if (membership.GetUser("Adam", false) == null)
            {
                membership.CreateUserAndAccount("Adam", "123456", new Dictionary<string, object> { { "FirstName", "Adam" }, { "LastName", "Smith" }, { "FacultyId", "011111113" }, { "Discriminator", "Faculty" } });
                //membership.CreateUser(fac1.UserName, "123456", false); }
            }
            if (!roles.GetRolesForUser("Adam").Contains("Faculty")) { roles.AddUsersToRoles(new[] { "Adam" }, new[] { "Faculty" }); }

            //fac4
            if (membership.GetUser("Mary", false) == null)
            {
                membership.CreateUserAndAccount("Mary", "123456", new Dictionary<string, object> { { "FirstName", "Mary" }, { "LastName", "Green" }, { "FacultyId", "011111114" }, { "Discriminator", "Faculty" } });
                //membership.CreateUser(fac1.UserName, "123456", false); }
            }
            if (!roles.GetRolesForUser("Mary").Contains("Faculty")) { roles.AddUsersToRoles(new[] { "Mary" }, new[] { "Faculty" }); }

            var fac1 = dc.Faculties.FirstOrDefault(n => n.FacultyId == "011111111");
            fac1.Courses.Add(int422a);
            int422a.Faculty = fac1;
            var fac2 = dc.Faculties.FirstOrDefault(n => n.FacultyId == "011111112");
            fac2.Courses.Add(jac444b);
            jac444b.Faculty = fac2;
            var fac3 = dc.Faculties.FirstOrDefault(n => n.FacultyId == "011111113");
            fac3.Courses.Add(sys466a);
            sys466a.Faculty = fac3;
            var fac4 = dc.Faculties.FirstOrDefault(n => n.FacultyId == "011111114");
            fac4.Courses.Add(dcn455c);
            dcn455c.Faculty = fac4;


            //Initialize Student
            //std1
            if (membership.GetUser("Bob", false) == null)
            {
                membership.CreateUserAndAccount("Bob", "123456", new Dictionary<string, object> { { "FirstName", "Bob" }, { "LastName", "Perry" }, { "StudentId", "022222221" }, { "Discriminator", "Student" } });
            }
            if (!roles.GetRolesForUser("Bob").Contains("Student")) { roles.AddUsersToRoles(new[] { "Bob" }, new[] { "Student" }); }
            //std2
            if (membership.GetUser("Amanda", false) == null)
            {
                membership.CreateUserAndAccount("Amanda", "123456", new Dictionary<string, object> { { "FirstName", "Amanda" }, { "LastName", "Simpson" }, { "StudentId", "022222222" }, { "Discriminator", "Student" } });
            }
            if (!roles.GetRolesForUser("Amanda").Contains("Student")) { roles.AddUsersToRoles(new[] { "Amanda" }, new[] { "Student" }); }
            //std3
            if (membership.GetUser("Wei", false) == null)
            {
                membership.CreateUserAndAccount("Wei", "123456", new Dictionary<string, object> { { "FirstName", "Wei" }, { "LastName", "Chen" }, { "StudentId", "022222223" }, { "Discriminator", "Student" } });
            }
            if (!roles.GetRolesForUser("Wei").Contains("Student")) { roles.AddUsersToRoles(new[] { "Wei" }, new[] { "Student" }); }
            //std4
            if (membership.GetUser("Jerry", false) == null)
            {
                membership.CreateUserAndAccount("Jerry", "123456", new Dictionary<string, object> { { "FirstName", "Jerry" }, { "LastName", "Lei" }, { "StudentId", "022222224" }, { "Discriminator", "Student" } });
            }
            if (!roles.GetRolesForUser("Jerry").Contains("Student")) { roles.AddUsersToRoles(new[] { "Jerry" }, new[] { "Student" }); }
            //std5
            if (membership.GetUser("Tim", false) == null)
            {
                membership.CreateUserAndAccount("Tim", "123456", new Dictionary<string, object> { { "FirstName", "Tim" }, { "LastName", "Pit" }, { "StudentId", "022222225" }, { "Discriminator", "Student" } });
            }
            if (!roles.GetRolesForUser("Tim").Contains("Student")) { roles.AddUsersToRoles(new[] { "Tim" }, new[] { "Student" }); }


            var std1 = dc.Students.FirstOrDefault(n => n.StudentId == "022222221");
            var std2 = dc.Students.FirstOrDefault(n => n.StudentId == "022222222");
            var std3 = dc.Students.FirstOrDefault(n => n.StudentId == "022222223");
            var std4 = dc.Students.FirstOrDefault(n => n.StudentId == "022222224");
            var std5 = dc.Students.FirstOrDefault(n => n.StudentId == "022222225");
            std1.Courses.Add(int422a);
            std1.Courses.Add(jac444b);
            std1.Courses.Add(sys466a);

            //Initialize Contact
            Contact con1 = new Contact("555-555-555");       
            con1.Student=std1;
            dc.Contacts.Add(con1);
            int422a.Students.Add(std1);
            jac444b.Students.Add(std1);
            sys466a.Students.Add(std1);
            Contact con2 = new Contact("bperry3@myseneca.ca");
            con2.Student=std1;
            dc.Contacts.Add(con2);
            Contact con3 = new Contact("asimpson@myseneca.ca");
            con3.Student = std2;
            dc.Contacts.Add(con3);
            jac444b.Students.Add(std2);
            dcn455c.Students.Add(std2);
            Contact con4 = new Contact("wchen30@myseneca.ca");
            con4.Student = std3;
            dc.Contacts.Add(con4);
            int422a.Students.Add(std3);
            sys466a.Students.Add(std3);
            dcn455c.Students.Add(std3);
            Contact con5 = new Contact("jlei@myseneca.ca");
            con5.Student = std4;
            dc.Contacts.Add(con5);
            jac444b.Students.Add(std4);
            sys466a.Students.Add(std4);
            Contact con6 = new Contact("tpit12@myseneca.ca");
            con6.Student = std5;
            dc.Contacts.Add(con6);
            int422a.Students.Add(std5);
            sys466a.Students.Add(std5);

            dc.Courses.Add(int422a);
            dc.Courses.Add(jac444b);
            dc.Courses.Add(sys466a);
            dc.Courses.Add(dcn455c);

            Message msg1 = new Message("INT422A", "4/2/2014", "11:40 - 13:25", "Test message 1 - class cancelled");
            dc.Messages.Add(msg1);
            Cancellation can1 = new Cancellation("4/2/2014");
            can1.Course = int422a;
            can1.Faculty = fac1;
            can1.Message = msg1;
            dc.Cancellations.Add(can1);

            dc.SaveChanges();
            
        }
    }
}