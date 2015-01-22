using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.ViewModels;
using SchoolNotification.Models;
using AutoMapper;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.Web.Security;
using System.Data.Entity.Validation;

namespace SchoolNotification.ViewModels
{
    public class RepoStudent : RepositoryBase
    {
        //public StudentFull CreateStudent(string fn, string ln, string stdId, string courseIds)
        public StudentFull CreateStudent(StudentCreate newItem, string courseIds)
        {
            System.Diagnostics.Debug.WriteLine("Start CreateStudent");
            var membership = (SimpleMembershipProvider)Membership.Provider;
            var roles = (SimpleRoleProvider)Roles.Provider;
            var stdUsername = newItem.FirstName;
            //Student student = new Student(newItem.FirstName, newItem.LastName, newItem.StudentId);
            if (membership.GetUser(stdUsername, false) == null)
            {
                membership.CreateUserAndAccount(stdUsername, "123456", new Dictionary<string, object> { { "FirstName", newItem.FirstName }, { "LastName", newItem.LastName }, { "StudentId", newItem.StudentId }, { "Discriminator", "Student" } });
            }
            if (!roles.GetRolesForUser(stdUsername).Contains("Student")) { roles.AddUsersToRoles(new[] { stdUsername }, new[] { "Student" }); }
            System.Diagnostics.Debug.WriteLine("CreateStudent here!!");
            var student = dc.Students.Include("Courses").FirstOrDefault(n=>n.StudentId ==newItem.StudentId);

            var nums = courseIds.Split(',');
            List<Int32> ls = new List<int>();
            foreach(var i in nums)
            {
                ls.Add(Convert.ToInt32(i));
            }
            foreach(var i in ls)
            {
                var course = dc.Courses.Include("Students").Include("Faculty").FirstOrDefault(n => n.Id == i);
                //var course = dc.Courses.Find(i);
                System.Diagnostics.Debug.WriteLine("CreateStudent course.CourseCode: "+course.CourseCode+" course facId: "+course.Faculty.UserId);
                student.Courses.Add(course);
                System.Diagnostics.Debug.WriteLine("CreateStudent after student.Coures.Add(): " + student.Courses.Count());
                course.Students.Add(student);
                System.Diagnostics.Debug.WriteLine("CreateStudent after course.Students.Add(): " + course.Students.Count());
            }
            try
            {
                dc.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (DbEntityValidationResult result in e.EntityValidationErrors)
                {
                    System.Diagnostics.Debug.WriteLine("{0} is Valid? {1}", result.Entry.Entity.ToString(), result.IsValid);

                    foreach (DbValidationError err in result.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("{0}: {1}", err.PropertyName, err.ErrorMessage);
                    }
                }
            }

            return Mapper.Map<StudentFull>(student);
       }
        public StudentFull EditStudent(StudentFull editItem, string courseIds)
        {
            System.Diagnostics.Debug.WriteLine("Student edit editItem.UserId: " + editItem.UserId);
            var studentToEdit = dc.Students.Include("Courses").FirstOrDefault(n=>n.UserId == editItem.UserId);
            if (studentToEdit == null)
            {
                return null;
            }
            else
            {
                dc.Entry(studentToEdit).Collection(s => s.Courses).Load();
                studentToEdit.Courses = new List<Course>();

                dc.Entry(studentToEdit).CurrentValues.SetValues(Mapper.Map<Student>(editItem));

                var nums = courseIds.Split(',');
                List<Int32> coursels = new List<int>();
                foreach (var i in nums)
                {
                    coursels.Add(Convert.ToInt32(i));
                }

                foreach (var i in coursels)
                {
                    System.Diagnostics.Debug.WriteLine("course Id: " + i);
                    var course = dc.Courses.Include("Students").FirstOrDefault(n => n.Id == i);
                    
                    studentToEdit.Courses.Add(course);
                    course.Students.Add(studentToEdit);
                }

                try
                {
                    dc.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (DbEntityValidationResult result in e.EntityValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine("{0} is Valid? {1}", result.Entry.Entity.ToString(), result.IsValid);

                        foreach (DbValidationError err in result.ValidationErrors)
                        {
                            System.Diagnostics.Debug.WriteLine("{0}: {1}", err.PropertyName, err.ErrorMessage);
                        }
                    }
                }
            }
            return Mapper.Map<StudentFull>(studentToEdit);
        }
        public void DeleteStudent(int? id)
        {
            var itemToDelete = dc.Students.Include("Contacts").FirstOrDefault(s=>s.UserId == id);
            if (itemToDelete == null)
            {
                return;
            }
            else
            {
                try
                {
                    dc.Students.Remove(itemToDelete);
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        //Get a ViewModel or list of ViewModels
        public IEnumerable<StudentBase> getListOfStudentBase()
        {
            var stds = dc.Students.OrderBy(f => f.StudentId);
            if (stds == null) return null;
            
            try {                 
                var temp= Mapper.Map<IEnumerable<StudentBase>>(stds);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
            
        }
        public StudentFull getStudentFull(int? id)
        {
            //var sId = Convert.ToInt32(id);
            var std = dc.Students.Include("Courses").SingleOrDefault(n => n.UserId == id);
            if (std == null) return null;
            try
            {
                return Mapper.Map<StudentFull>(std);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
                return null;
            }
        }

        //Get select lisf of Students method
        public SelectList getStudentSelectList()
        {
            var student = dc.Students.OrderBy(n => n.UserId);
            if (student == null) return null;

            try
            {
                IEnumerable<Student> stu = Mapper.Map<IEnumerable<Student>>(student);
                if (student == null)
                {
                    SelectList sl = new SelectList(stu, "Id", "FirstName" + " " + "LastName" + " " + "StudentId");
                    return sl;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("Get student list null value");
                    return null;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Get Student AutoMapper Exception: " + e.StackTrace);
                return null;
            }

        }
    
    }
}