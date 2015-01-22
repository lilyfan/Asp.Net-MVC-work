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

namespace SchoolNotification.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private DataContext dc  = new DataContext();
        private RepoAdmin fac = new RepoAdmin();

        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult List()
        {
            try
            {
                return View(fac.getListOfAdminBase());
            }
            catch (Exception e)
            {
                ViewBag.ExceptionMessage = e.Message;
                return View("Error");
            }
            
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = "Invalid Id";
            }

            if (fac == null)
            {
                ViewBag.ExceptionMessage = "Not found user";
            }
            System.Diagnostics.Debug.WriteLine("AdminController id" + id);
            AdminFull admin = fac.getAdminFull(id);
            return View(admin);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            System.Diagnostics.Debug.WriteLine("Admin controller create ");
            System.Diagnostics.Debug.WriteLine("form count: " + form.Count);
            if (ModelState.IsValid)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("form count: " + form.Count);
                    if (form.Count == 4)
                    {
                        System.Diagnostics.Debug.WriteLine("form[0]" + form[0]);
                        System.Diagnostics.Debug.WriteLine("form[1]" + form[1]);
                        System.Diagnostics.Debug.WriteLine("form[2]" + form[2]);
                        System.Diagnostics.Debug.WriteLine("form[3]" + form[3]);
                        
                        var addedItem = fac.CreateAdmin(form[1], form[2], form[3]);
                        if (addedItem == null)
                        {
                            ViewBag.ExceptionMessage = "Could not create admin";
                            return View("Error");
                        }
                    }
                    return RedirectToAction("List");
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
            AdminFull admin = fac.getAdminFull(id);
            if (admin == null)
            {
                ViewBag.ExceptionMessage = "That record could not be edited because it doesn't exist";
                return View("Error");
            }

            return View(admin);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModels.AdminFull editedItem)
        {
            if (ModelState.IsValid)
            {
                var newItem = fac.EditAdmin(editedItem);
                if (newItem == null)
                {
                    ViewBag.ExceptionMessage = "record " + editedItem.UserId + " was not found.";
                    return View("Error");
                }
                else
                {
                    return RedirectToAction("List");
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
            AdminFull admin = fac.getAdminFull(id);
            if (admin == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it doesn't exist";
                return View("Error");
            }
            return View(admin);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            fac.DeleteAdmin(id);
            return RedirectToAction("List");
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