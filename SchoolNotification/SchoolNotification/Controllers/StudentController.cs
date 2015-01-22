using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using SchoolNotification.Models;
using SchoolNotification.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using WebMatrix.WebData;
using System.Web.Security;
using AutoMapper;


namespace SchoolNotification.Controllers
{
    [Authorize(Roles = "Admin, Student")]
    public class StudentController : Controller
    {

        private DataContext dc = new DataContext();
        private RepoFaculty fac = new RepoFaculty();
        private RepoCourse course = new RepoCourse();
        private RepoStudent std = new RepoStudent();
        private RepoContact con = new RepoContact();

        [Authorize(Roles = "Admin, Student")]
        public ActionResult Index()
        {
            var currentUserName = User.Identity.GetUserName();
            var currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (currentUserName != "")
            {
                try
                {
                    if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
                    {
                        ViewBag.adminView = "Y";
                        ViewBag.stdlist = std.getListOfStudentBase();
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Student"))
                    {
                        ViewBag.stdView = "Y";
                        ViewBag.courselistforstd = course.getListOfCourseForStudent(currentUserId);
                    }
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                    return View("Error");
                }
            }
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = "Invalid Id";
            }

            if (std == null)
            {
                ViewBag.ExceptionMessage = "Not found user";
            }
            System.Diagnostics.Debug.WriteLine("StudentController id" + id);
            ViewBag.contactlist = con.getContactSelectListForStudent(id);
            ViewBag.courselist = course.getCourseSelectListForStudent(id);
            StudentFull student = std.getStudentFull(id);
            return View(student);
        }

        public ActionResult Create()
        {
            ViewBag.courses = course.getCourseSelectList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            //System.Diagnostics.Debug.WriteLine("Student controller create ");
            //System.Diagnostics.Debug.WriteLine("form count: " + form.Count);
            ViewBag.userId = WebSecurity.GetUserId(User.Identity.Name);
            if (ModelState.IsValid)
            {
                try
                {
                    //System.Diagnostics.Debug.WriteLine("Student Controller form count: " + form.Count);
                    if (form.Count == 5)
                    {
                        System.Diagnostics.Debug.WriteLine("form[0]" + form[0]);
                        System.Diagnostics.Debug.WriteLine("FirstName-form[1]" + form[1]);
                        System.Diagnostics.Debug.WriteLine("LastName-form[2]" + form[2]);
                        System.Diagnostics.Debug.WriteLine("StudentId-form[3]" + form[3]);
                        System.Diagnostics.Debug.WriteLine("Courses-form[4]" + form[4]);

                        ViewModels.StudentCreate newItem = new StudentCreate(form[1], form[2], form[3]);
                        //var addedItem = std.CreateStudent(form[1], form[2], form[3], form[4]);
                        var addedItem = std.CreateStudent(newItem, form[4]);
                        if (addedItem == null)
                        {
                            ViewBag.ExceptionMessage = "Could not create faculty";
                            return View("Error");
                        }
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                    return View("Error");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                ViewBag.errors = errors.ToString();
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = " Invalid record";
                return View("Error");
            }
            ViewBag.userId = RouteData.Values["id"];
            ViewBag.courselist = course.getCourseSelectList();
            StudentFull student = std.getStudentFull(id);
            if (student == null)
            {
                ViewBag.ExceptionMessage = "That record could not be edited because it doesn't exist";
                return View("Error");
            }

            return View(student);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form, ViewModels.StudentFull editedItem)
        {
            int id = Convert.ToInt32(RouteData.Values["id"]);
            editedItem = Mapper.Map<StudentFull>(dc.Students.Include("Courses").FirstOrDefault(n => n.UserId == id));
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Student edit form[5]: " + form[5]);
                var newItem = std.EditStudent(editedItem, form[5]);
                if (newItem == null)
                {
                    ViewBag.ExceptionMessage = "record " + editedItem.UserId + " was not found.";
                    return View("Error");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = "That was an invalid record";
                return View("Error");
            }
            StudentFull studnet = std.getStudentFull(id);
            if (studnet == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it doesn't exist";
                return View("Error");
            }
            return View(studnet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            std.DeleteStudent(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dc.Dispose();
            }
            base.Dispose(disposing);
        }




    }
}