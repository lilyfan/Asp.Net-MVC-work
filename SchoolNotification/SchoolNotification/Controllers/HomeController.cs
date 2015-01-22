using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using SchoolNotification.Models;
using SchoolNotification.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using System.Web.Security;
using WebMatrix.WebData;

namespace SchoolNotification.Controllers
{
    public class HomeController : Controller
    {
        private DataContext dc = new DataContext();
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
                        //System.Diagnostics.Debug.WriteLine("Here");
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Faculty"))
                    {
                        return RedirectToAction("Index", "Faculty");
                    }
                    else if (roles.GetRolesForUser(currentUserName).Contains("Student"))
                    {
                        return RedirectToAction("Index", "Student");
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception e)
                {
                    ViewBag.ExceptionMessage = e.Message;
                    return View("Error");
                }
            }
            else { return View(); }

        }

    }
}
