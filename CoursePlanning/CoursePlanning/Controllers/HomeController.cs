using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// This source code file was created by the FILE > New > Project task
// It provides the default behaviour for the web app's home page rendering task

// You can keep this controller (and its view) if you want a browser-accessible entry point

namespace CoursePlanning.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "CoursePlanning";

            return View();
        }
    }
}
