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

namespace SchoolNotification.Models
{
    [Authorize]
    public class FacultyController : Controller
    {
        private DataContext dc = new DataContext();
        private RepoFaculty fac = new RepoFaculty();
        private RepoCourse course = new RepoCourse();

        [Authorize(Roles="Admin, Faculty")]
        public ActionResult Index()
        {
            var currentUserName = User.Identity.Name;
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (currentUserName != "")
            {
                try
                {
                    if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
                    {
                        ViewBag.faclist = fac.getListOfFacultyBase();
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Faculty"))
                    {

                        return RedirectToAction("Index", "Cancellation");
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

        

        [Authorize(Roles="Admin")]
        public ActionResult Details(int? id)
        {
            if(id== null){
                ViewBag.ExceptionMessage ="Invalid Id";
            }
            
            if (fac == null)
            {
                ViewBag.ExceptionMessage = "Not found user";
            }
            System.Diagnostics.Debug.WriteLine("FacultyController id" + id);
            ViewBag.courselist = course.getCourseSelectListForFaculty(id);
            FacultyFull faculty = fac.getFacultyFull(id);
            return View(faculty);
        }

        public ActionResult Create()
        {
            ViewBag.courses = course.getCourseSelectList();
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        //public ActionResult Create(FormCollection form, ViewModels.FacultyCreate newItem)
        public ActionResult Create(FormCollection form)
        {
            System.Diagnostics.Debug.WriteLine("Faculty controller create ");
            System.Diagnostics.Debug.WriteLine("form count: " + form.Count);
            //var currentUser = await admin.FindByIdAsync(User.Identity.GetUserId());
            if(ModelState.IsValid)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("form count: " + form.Count);
                    if (form.Count == 5)
                    {
                        System.Diagnostics.Debug.WriteLine("form[0]" + form[0]);
                        System.Diagnostics.Debug.WriteLine("form[1]" + form[1]);
                        System.Diagnostics.Debug.WriteLine("form[2]" + form[2]);
                        System.Diagnostics.Debug.WriteLine("form[3]" + form[3]);
                        System.Diagnostics.Debug.WriteLine("form[4]" + form[4]);
                        var addedItem = fac.CreateFaculty(form[1], form[2], form[3], form[4]);
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

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            var currentUserId = User.Identity.GetUserId();
            if (id == null)
            {
                ViewBag.ExceptionMessage = " Invalid record";
                return View("Error");
            }
            ViewBag.courselist = course.getCourseSelectList();
            FacultyFull faculty = fac.getFacultyFull(id);
            if (faculty == null)
            {
                ViewBag.ExceptionMessage = "That record could not be edited because it doesn't exist";
                return View("Error");
            }
            
            return View(faculty);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection form, ViewModels.FacultyFull editedItem)
        {
            int id = Convert.ToInt32(RouteData.Values["id"]);
            editedItem = Mapper.Map<FacultyFull>(dc.Faculties.Include("Courses").FirstOrDefault(n=>n.UserId == id));
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Faculty edit form[4]: " + form[4]);
                var newItem = fac.EditFaculty(editedItem, form[4]);
                if(newItem == null)
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

        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            //var currentUser = await admin.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                ViewBag.ExceptionMessage = "That was an invalid record";
                return View("Error");
            }
            FacultyFull faculty = fac.getFacultyFull(id);
            if (faculty == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it doesn't exist";
                return View("Error");
            }
            return View(faculty);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            fac.DeleteFaculty(id);
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