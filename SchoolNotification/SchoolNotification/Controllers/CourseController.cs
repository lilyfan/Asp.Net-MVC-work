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

namespace SchoolNotification.Models
{
    [Authorize]
    public class CourseController : Controller
    {
        private DataContext dc = new DataContext();
        private RepoFaculty fac = new RepoFaculty();
        private RepoCourse course = new RepoCourse();
        private RepoStudent std = new RepoStudent();

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
                        ViewBag.courselist = course.getListOfCourseBase();
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


        [Authorize(Roles = "Admin, Student")]
        public ActionResult Details(int? id)
        {
            var currentUserName = User.Identity.GetUserName();
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
            {
                ViewBag.adminView = "Y";               
            }

            if(id== null){
                ViewBag.ExceptionMessage ="Invalid Id";
            }
            ViewBag.id = id;
            CourseFull coursefull = course.getCourseFull(id);
            return View(coursefull);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewModels.CourseCreate newItem = new ViewModels.CourseCreate();
            return View(newItem);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(FormCollection form, ViewModels.CourseCreate newItem)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    //System.Diagnostics.Debug.WriteLine("form count: " + form.Count);
                    if (form.Count == 7)
                    {
                        System.Diagnostics.Debug.WriteLine("form[0]" + form[0]);
                        System.Diagnostics.Debug.WriteLine("CourseName-form[1]" + form[1]);
                        System.Diagnostics.Debug.WriteLine("CourseCode-form[2]" + form[2]);
                        System.Diagnostics.Debug.WriteLine("Room-form[3]" + form[3]);
                        System.Diagnostics.Debug.WriteLine("Date-form[4]" + form[4]);
                        System.Diagnostics.Debug.WriteLine("Time-form[5]" + form[5]);
                        System.Diagnostics.Debug.WriteLine("Faculty-form[6]" + form[6]);


                        var addedItem = course.CreateCourse(newItem, form[6]);
                        if (addedItem == null)
                        {
                            ViewBag.ExceptionMessage = "Could not create cancellation";
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
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = " Invalid record";
                return View("Error");
            }
            var editedCourse = course.getCourseFull(id);
            if (editedCourse == null)
            {
                ViewBag.ExceptionMessage = "That record could not be edited because it doesn't exist";
                return View("Error");
            }

            return View(editedCourse);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModels.CourseFull editedItem)
        {
            if (ModelState.IsValid)
            {
                var newItem = course.EditCourse(editedItem);
                if(newItem == null)
                                    {
                    ViewBag.ExceptionMessage = "record " + editedItem.Id + " was not found.";
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


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = "That was an invalid record";
                return View("Error");
            }
            var courseToDel = course.getCourseFull(id);
            if (courseToDel == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it doesn't exist";
                return View("Error");
            }
            return View(courseToDel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            course.DeleteCourse(id);
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