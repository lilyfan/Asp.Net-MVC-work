using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolNotification.ViewModels;
using AutoMapper;
using WebMatrix.WebData;
using System.Web.Security;
using System.Data.Entity.Validation;
using SchoolNotification.Models;
using System.Web.Mvc;

namespace SchoolNotification.ViewModels
{
    public class RepoFaculty : RepositoryBase
    {
        public FacultyFull CreateFaculty(string fn, string ln, string fId, string courseIds)
        {
            var membership = (SimpleMembershipProvider)Membership.Provider;
            var roles = (SimpleRoleProvider)Roles.Provider;
            var facUserName = fn;
            //Faculty faculty= new Faculty(fn, ln, fId);
            if (membership.GetUser(facUserName, false) == null)
            {
                membership.CreateUserAndAccount(facUserName, "123456", new Dictionary<string, object> { { "FirstName", fn }, { "LastName", ln }, { "FacultyId", fId }, { "Discriminator", "Faculty" } });
            }
            if (!roles.GetRolesForUser(facUserName).Contains("Faculty")) { roles.AddUsersToRoles(new[] { facUserName }, new[] { "Faculty" }); }
            var faculty = dc.Faculties.Include("Courses").FirstOrDefault(n => n.FacultyId == fId);
            
            var nums = courseIds.Split(',');
            List<Int32> coursels = new List<int>();
            foreach(var i in nums)
            {
                coursels.Add(Convert.ToInt32(i));
            }
            foreach (var i in coursels)
            {
                System.Diagnostics.Debug.WriteLine("course Id: " + i);
                var course = dc.Courses.Include("Faculty").FirstOrDefault(n => n.Id == i);
                faculty.Courses.Add(course);
                course.Faculty = faculty;
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
            return Mapper.Map<FacultyFull>(faculty);
       }
        public FacultyFull EditFaculty(FacultyFull editItem, string courseIds)
        {
            var facultyToEdit = dc.Faculties.Include("Courses").FirstOrDefault(n=>n.UserId==editItem.UserId);
            System.Diagnostics.Debug.WriteLine("faculty edit editItem.UserId " + editItem.UserId);
            if (facultyToEdit == null)
            {
                return null;
            }
            else
            {
                dc.Entry(facultyToEdit).Collection(s => s.Courses).Load();
                facultyToEdit.Courses = new List<Course>();

                dc.Entry(facultyToEdit).CurrentValues.SetValues(Mapper.Map<Faculty>(editItem));

                var nums = courseIds.Split(',');
                List<Int32> coursels = new List<int>();
                foreach (var i in nums)
                {
                    coursels.Add(Convert.ToInt32(i));
                }
                foreach (var i in coursels)
                {
                    System.Diagnostics.Debug.WriteLine("course Id: " + i);
                    var course = dc.Courses.Include("Faculty").FirstOrDefault(n => n.Id == i);
                    facultyToEdit.Courses.Add(course);
                    course.Faculty = facultyToEdit;
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
            return Mapper.Map<FacultyFull>(facultyToEdit);
        }
        public void DeleteFaculty(int? id)
        {
            System.Diagnostics.Debug.WriteLine("RepoFaculty delete faculty fac.Id: " + id);

            var itemToDelete = dc.Faculties.Include("Courses").FirstOrDefault(n=>n.UserId == id);
            
            if (itemToDelete == null)
            {
                return;
            }
            else
            {
                try
                {
                    dc.Faculties.Remove(itemToDelete);
                    dc.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        //Get a ViewModel or list of ViewModels
        public IEnumerable<FacultyBase> getListOfFacultyBase()
        {
            var facs = dc.Faculties.Where(f => f.FacultyId != null);
            //System.Diagnostics.Debug.WriteLine("course size: " + facs.Count());
            if (facs == null) return null;
            
            try {                 
                var temp= Mapper.Map<IEnumerable<FacultyBase>>(facs);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Automapper exception: "+e.StackTrace);
                return null;
            }
        }
        public FacultyFull getFacultyFull(int? id)
        {
            var faculty = dc.Faculties.Include("Courses").SingleOrDefault(n => n.UserId == id);
            if (faculty == null) return null;

            try {                 
                var temp= Mapper.Map<FacultyFull>(faculty);
                return temp;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Automapper exception: "+e.StackTrace);
                return null;
            }
        }
        public SelectList getFacultySelectList()
        {
            var lsFaculties = new List<string>();
            lsFaculties.Add("Select Faculty");
            var facs = dc.Faculties.OrderBy(f=>f.FacultyId);
            foreach( var i in facs)
            {
                var facFullname = i.LastName + ", " + i.FirstName + "(" + i.FacultyId+")";
                lsFaculties.Add(facFullname);
            }
            SelectList sl = new SelectList(lsFaculties);
            return sl;
        }

    }
}