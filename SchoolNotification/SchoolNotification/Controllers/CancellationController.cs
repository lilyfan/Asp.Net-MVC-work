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
    public class CancellationController : Controller
    {
        private DataContext dc = new DataContext();
        private RepoCancellation can = new RepoCancellation();
        private RepoFaculty fac = new RepoFaculty();
        private RepoCourse course = new RepoCourse();

        [Authorize(Roles = "Admin, Faculty, Student")]
        public ActionResult Index()
        {
            var currentUserName = User.Identity.Name;
            var roles = (SimpleRoleProvider)Roles.Provider;
            //var searchStr = "";
            var searchStr = Request.QueryString["SearchString"];
            System.Diagnostics.Debug.WriteLine("Cancellation Controller Index searchStr: " + searchStr+";");
            if (currentUserName != "")
            {
                try
                {
                    if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
                    {
                        ViewBag.adminView = "Y";
                        //System.Diagnostics.Debug.WriteLine("Here");
                        if (searchStr == null)
                        {
                            ViewBag.canlist = can.getListOfCancellationBase();
                        }
                        else
                        {
                            ViewBag.canlist = can.searchListOfCancellationBase(searchStr);
                        }
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Faculty"))
                    {
                        ViewBag.facultyView = "Y";
                        System.Diagnostics.Debug.WriteLine("cancellation.Faculty Name: " + User.Identity.Name);
                        var currentUser = dc.Faculties.FirstOrDefault(n => n.UserName == User.Identity.Name);
                        System.Diagnostics.Debug.WriteLine("cancellation Controller Faculty Id: " + currentUser.UserId);
                        ViewBag.canlist = can.getListOfCancellationForFaculty(currentUser.UserId);
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Student"))
                    {
                        ViewBag.studentView = "Y";
                        var currentUserId = WebSecurity.GetUserId(User.Identity.Name);
                        System.Diagnostics.Debug.WriteLine("cancellation Controller Student Id: " + currentUserId);
                        var canlist = can.getListOfCancellationBaseForStd(currentUserId);
                        if(canlist.Count() != 0){
                            ViewBag.viewcan = true;
                            ViewBag.canlist = canlist;
                        } 
                        
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
        [HttpPost]
        public string Index(FormCollection fc, string searchString)
        {
            return "<h3> From [HttpPost]Index: " + searchString + "</h3>";
        }

        [Authorize(Roles = "Admin, Faculty, Student")]
        public ActionResult Details(int? id)
        {
            var currentUserName = User.Identity.GetUserName();
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
            {
                ViewBag.adminView = "Y";
            }
            else if (roles.GetRolesForUser(currentUserName).Contains("Faculty"))
            {
                ViewBag.facultyView = "Y";
            }
            if(id== null){
                ViewBag.ExceptionMessage ="Invalid Id";
            }
            ViewBag.id = id;
            return View(can.getCancellationFull(id));
        }
        [Authorize(Roles = "Admin,Faculty")]
        public ActionResult Create()
        {
            var currentUserName = User.Identity.GetUserName();
            var currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            var roles = (SimpleRoleProvider)Roles.Provider;
            if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
            {
                ViewBag.adminView = "Y";
                ViewBag.courses = course.getCourseSelectList();
                ViewBag.faculties = fac.getFacultySelectList();
            }
            else if (roles.GetRolesForUser(currentUserName).Contains("Faculty"))
            {
                ViewBag.facultyView = "Y";
                ViewBag.courses = course.getCourseSelectListForFaculty(currentUserId);
            }
            ViewModels.CancellationCreate newItem = new ViewModels.CancellationCreate();           
            ViewBag.dates = can.getDatesSelectList();
            return View(newItem);
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            var currentUserName = User.Identity.GetUserName();
            var currentUserId = WebSecurity.GetUserId(User.Identity.Name);
            var roles = (SimpleRoleProvider)Roles.Provider;
            string facId = "";
            if(ModelState.IsValid)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine("form count: "+form.Count);
                    if (roles.GetRolesForUser(currentUserName).Contains("Admin"))
                    {
                        if (form.Count == 6)
                        {
                            System.Diagnostics.Debug.WriteLine("form[0]" + form[0]);
                            System.Diagnostics.Debug.WriteLine("form[1]" + form[1]);
                            System.Diagnostics.Debug.WriteLine("form[2]" + form[2]);
                            System.Diagnostics.Debug.WriteLine("form[3]" + form[3]);
                            System.Diagnostics.Debug.WriteLine("form[4]" + form[4]);
                            System.Diagnostics.Debug.WriteLine("form[5]" + form[5]);

                            System.Diagnostics.Debug.WriteLine("Admin create cancellation form[5].ToString().IndexOf('(')" + form[5].ToString().IndexOf("("));
                            System.Diagnostics.Debug.WriteLine("Admin create cancellation form[5].ToString().IndexOf(')')" + form[5].ToString().IndexOf(")"));
                            facId = form[5].ToString().Substring(form[5].ToString().IndexOf('(')+1, 9);
                            System.Diagnostics.Debug.WriteLine("Admin create cancellation facId" + facId);
                            var addedItem = can.CreateCancellation(form[1], form[2], form[3], form[4], facId);
                            if (addedItem == null)
                            {
                                ViewBag.ExceptionMessage = "Could not create cancellation";
                                return View("Error");
                            }
                        }
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Faculty"))
                    {
                        if (form.Count == 5)
                        {
                            System.Diagnostics.Debug.WriteLine("form[0]" + form[0]);
                            System.Diagnostics.Debug.WriteLine("form[1]" + form[1]);
                            System.Diagnostics.Debug.WriteLine("form[2]" + form[2]);
                            System.Diagnostics.Debug.WriteLine("form[3]" + form[3]);
                            System.Diagnostics.Debug.WriteLine("form[4]" + form[4]);

                            facId = dc.Faculties.Find(currentUserId).FacultyId;
                            System.Diagnostics.Debug.WriteLine("Faculty create cancellation facId" + facId);
                            var addedItem = can.CreateCancellation(form[1], form[2], form[3], form[4], facId);
                            if (addedItem == null)
                            {
                                ViewBag.ExceptionMessage = "Could not create cancellation";
                                return View("Error");
                            }
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
            //var currentUser = await faculty.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                ViewBag.ExceptionMessage = " Invalid record";
                return View("Error");
            }
            var cancellation = can.getCancellationFull(id);
            if (cancellation == null)
            {
                ViewBag.ExceptionMessage = "That record could not be edited because it doesn't exist";
                return View("Error");
            }
            /*if (cancellation.User.Id != currentUser.Id)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }*/
            
            return View(cancellation);
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ViewModels.CancellationFull editedItem)
        {
            if (ModelState.IsValid)
            {
                var newItem = can.EditCancellation(editedItem);
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
        [Authorize(Roles = "Admin, Faculty")]
        public ActionResult Delete(int? id)
        {
            //var currentUser = await faculty.FindByIdAsync(User.Identity.GetUserId());
            if (id == null)
            {
                ViewBag.ExceptionMessage = "That was an invalid record";
                return View("Error");
            }
            var cancellation = can.getCancellationFull(id);
            if(cancellation == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it doesn't exist";
                return View("Error");
            }
            /*if (cancellation.User.Id != currentUser.Id)
            {
                 return new HttpStatusCodeResult(HttpStatusCode.Unauthorized);
            }*/
            return View(cancellation);
        }
        [Authorize(Roles = "Admin, Faculty")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            can.DeleteCancellation(id);
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