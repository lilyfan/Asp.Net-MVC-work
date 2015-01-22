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

namespace SchoolNotification.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private DataContext dc = new DataContext();
        //private RepoCancellation can = new RepoCancellation();
        //private RepoMessage msg = new RepoMessage();
        //private RepoCourse course = new RepoCourse();
        private RepoStudent std = new RepoStudent();
        private RepoContact con = new RepoContact();

        [Authorize(Roles = "Admin, Student")]
        public ActionResult Index()
        {
            var currentUserName = User.Identity.GetUserName();
            var currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            var roles = (SimpleRoleProvider)Roles.Provider;
            int stdId = Convert.ToInt32(RouteData.Values["id"]);
            if (currentUserName != null)
            {
                try
                {
                    if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
                    {
                        System.Diagnostics.Debug.WriteLine("Contact Controller student user stdId: "+stdId);
                        ViewBag.adminView = "Y";
                        ViewBag.stdUserId = stdId;
                        ViewBag.contactlist = con.getListOfContactForStudent(stdId);
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Student"))
                    {
                        System.Diagnostics.Debug.WriteLine("Contact Controller student user");
                        ViewBag.stdView = "Y";
                        ViewBag.stdUserId = currentUserId;
                        ViewBag.contactlist = con.getListOfContactForStudent(currentUserId);
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
            if (id == null)
            {
                ViewBag.ExceptionMessage = "Invalid Id";
            }
            return View(con.getContactFull(id));
        }

        [Authorize(Roles = "Admin, Student")]
        public ActionResult Create()
        {
            ViewModels.ContactCreate newItem = new ViewModels.ContactCreate();
            //ViewBag.courses = course.getCourseSelectList();
            //ViewBag.dates = can.getDatesSelectList();
            return View(newItem);
        }

        [Authorize(Roles = "Admin, Student")]
        [HttpPost]
        public ActionResult Create(FormCollection form, ViewModels.ContactCreate newItem, int? id)
        {
            int stdUserId = 0;
            var currentUserName = User.Identity.GetUserName();
            //var currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
            {
                stdUserId = (int) id;
                
            }
            else if (roles.GetRolesForUser(currentUserName).Contains("Student"))
            {
                stdUserId = WebSecurity.GetUserId(User.Identity.Name);
            }
            if (ModelState.IsValid)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("Student Controller form count: " + form.Count);
                    if (form.Count == 2)
                    {
                        System.Diagnostics.Debug.WriteLine("form[0]" + form[0]);
                        System.Diagnostics.Debug.WriteLine("ContactInfo-form[1]" + form[1]);
                        //System.Diagnostics.Debug.WriteLine("StudentId-form[2]" + form[2]);
                        var addedItem = con.CreateContact(newItem, stdUserId);
                        if (addedItem == null)
                        {
                            ViewBag.ExceptionContact = "Could not create cancellation";
                            return View("Error");
                        }
                    }
                    
                    return RedirectToAction("Index", new { id = id });
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

        [Authorize(Roles = "Admin, Student")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = " Invalid record";
                return View("Error");
            }
            var contact = con.getContactFull(id);
            if (contact == null)
            {
                ViewBag.ExceptionMessage = "That record could not be edited because it doesn't exist";
                return View("Error");
            }
            ViewBag.conId = id;
            return View(contact);
        }

        [Authorize(Roles = "Admin, Student")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModels.ContactFull editedItem)
        {
            var id = Convert.ToInt32(RouteData.Values["id"]);
            var userId = Convert.ToInt32(Request.QueryString["userId"]);
            ViewBag.stdUserId = userId;
            System.Diagnostics.Debug.WriteLine("Contact Controller Edit userId: " + userId);
            if (ModelState.IsValid)
            {
                var newItem = con.EditContact(editedItem);
                if (newItem == null)
                {
                    ViewBag.ExceptionMessage = "record " + editedItem.Id + " was not found.";
                    return View("Error");
                }
                else
                {
                    return RedirectToAction("Index", new { id = userId });
                }
            }
            else
            {
                return View("Error");
            }
        }

        [Authorize(Roles = "Admin, Student")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = "That was an invalid record";
                return View("Error");
            }
            var contact = con.getContactFull(id);
            if (contact == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it doesn't exist";
                return View("Error");
            }
            return View(contact);
        }

        [Authorize(Roles = "Admin, Student")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var userId = Convert.ToInt32(Request.QueryString["userId"]);
            ViewBag.stdUserId = userId;
            con.DeleteContact(id);
            return RedirectToAction("Index", new { id = userId });
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