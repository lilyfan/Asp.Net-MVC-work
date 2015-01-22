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
    public class MessageController : Controller
    {
        private DataContext dc = new DataContext();
        private RepoCancellation can = new RepoCancellation();
        private RepoMessage msg = new RepoMessage();
        private RepoCourse course = new RepoCourse();

        [Authorize(Roles = "Admin, Faculty, Student")]
        public ActionResult Index()
        {
            var currentUserName = User.Identity.GetUserName();
            var currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            var roles = (SimpleRoleProvider)Roles.Provider;
            int canId = Convert.ToInt32(RouteData.Values["id"]);
            if (currentUserName != null)
            {
                try
                {
                    if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
                    {
                        ViewBag.adminView = "Y";
                        ViewBag.msglist = msg.getListOfMessageBase();
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Faculty"))
                    {
                        ViewBag.facutlyView = "Y";
                        ViewBag.msglist = msg.getListOfMessageForFaculty(currentUserId);
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Student"))
                    {
                        ViewBag.studentView = "Y";
                        ViewBag.msglist = msg.getListOfMessageBase(); ;
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
        [Authorize(Roles = "Admin, Faculty, Student")]
        public ActionResult Details(int? id)
        {
            var currentUserName = User.Identity.GetUserName();
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (roles.GetRolesForUser(currentUserName).Contains("Admin")){ ViewBag.adminView = "Y"; }
            else if (roles.GetRolesForUser(currentUserName).Contains("Faculty")) { ViewBag.facultyView = "Y";}
            if (id == null)
            {
                ViewBag.ExceptionMessage = "Invalid Id";
            }
            return View(msg.getMessageFull(id));
        }
        [Authorize(Roles = "Admin, Faculty")]
        public ActionResult Create()
        {
            ViewModels.MessageCreate newItem = new ViewModels.MessageCreate();
            return View(newItem);
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public ActionResult Create(FormCollection form, ViewModels.MessageCreate newItem)
        {
            //var currentUser = await faculty.FindByIdAsync(User.Identity.GetUserId());
            if (ModelState.IsValid)
            {
                try
                {
                    if (form.Count == 5)
                    {
                        var addedItem = msg.CreateMessage(newItem);
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
        [Authorize(Roles = "Admin, Faculty")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = " Invalid record";
                return View("Error");
            }
            var message = msg.getMessageFull(id);
            if (message == null)
            {
                ViewBag.ExceptionMessage = "That record could not be edited because it doesn't exist";
                return View("Error");
            }

            return View(message);
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModels.MessageFull editedItem)
        {
            if (ModelState.IsValid)
            {
                var newItem = msg.EditMessage(editedItem);
                if (newItem == null)
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

        [Authorize(Roles = "Admin, Faculty")]
        public ActionResult Delete(int? id)
        {
            //var currentUser = await faculty.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                ViewBag.ExceptionMessage = "That was an invalid record";
                return View("Error");
            }
            var message = msg.getMessageFull(id);
            if (message == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it doesn't exist";
                return View("Error");
            }
            return View(message);
        }

        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            msg.DeleteMessage(id);
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