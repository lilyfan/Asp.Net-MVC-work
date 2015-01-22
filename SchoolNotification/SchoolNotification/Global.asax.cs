using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using System.Data.Entity;
using SchoolNotification.Models;
using WebMatrix.WebData;
//using SchoolNotification.Migrations;


namespace SchoolNotification
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataContext, Configuration>());
            //System.Data.Entity.Database.SetInitializer(new SchoolNotification.Models.Initiallizer());
            System.Diagnostics.Debug.WriteLine("Global Here! ");
            Database.SetInitializer<DataContext>(new Initiallizer());
            System.Diagnostics.Debug.WriteLine("Global Here! 2222");

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();





            //To ViewModels classes
            Mapper.CreateMap<Models.Admin, ViewModels.AdminBase>();
            Mapper.CreateMap<Models.Admin, ViewModels.AdminFull>();
            Mapper.CreateMap<Models.Faculty, ViewModels.FacultyBase>();
            Mapper.CreateMap<Models.Faculty, ViewModels.FacultyFull>();
            Mapper.CreateMap<Models.Course, ViewModels.CourseBase>();
            Mapper.CreateMap<Models.Course, ViewModels.CourseFull>();
            Mapper.CreateMap<Models.Cancellation, ViewModels.CancellationBase>();
            Mapper.CreateMap<Models.Cancellation, ViewModels.CancellationFull>();
            Mapper.CreateMap<Models.Message, ViewModels.MessageBase>();
            Mapper.CreateMap<Models.Message, ViewModels.MessageFull>();
            Mapper.CreateMap<Models.Student, ViewModels.StudentBase>();
            Mapper.CreateMap<Models.Student, ViewModels.StudentFull>();
            Mapper.CreateMap<Models.Contact, ViewModels.ContactBase>();
            Mapper.CreateMap<Models.Contact, ViewModels.ContactFull>();

            //From ViewModels classes
            Mapper.CreateMap<ViewModels.AdminBase, Models.Admin>();
            Mapper.CreateMap<ViewModels.AdminFull, Models.Admin>();
            Mapper.CreateMap<ViewModels.FacultyBase, Models.Faculty>();
            Mapper.CreateMap<ViewModels.FacultyFull, Models.Faculty>();
            Mapper.CreateMap<ViewModels.CourseBase, Models.Course>();
            Mapper.CreateMap<ViewModels.CourseFull, Models.Course>();
            Mapper.CreateMap<ViewModels.CancellationBase, Models.Cancellation>();
            Mapper.CreateMap<ViewModels.CancellationFull, Models.Cancellation>();
            Mapper.CreateMap<ViewModels.MessageBase, Models.Message>();
            Mapper.CreateMap<ViewModels.MessageFull, Models.Message>();
            Mapper.CreateMap<ViewModels.StudentFull, Models.Student>();
            Mapper.CreateMap<ViewModels.StudentBase, Models.Student>();
            Mapper.CreateMap<ViewModels.ContactFull, Models.Contact>();
            Mapper.CreateMap<ViewModels.ContactBase, Models.Contact>();

            //Other maps
            Mapper.CreateMap<ViewModels.AdminCreate, Models.Admin>();
            Mapper.CreateMap<Models.Admin, ViewModels.AdminCreate>();
            Mapper.CreateMap<ViewModels.FacultyCreate, Models.Faculty>();
            Mapper.CreateMap<Models.Faculty, ViewModels.FacultyCreate>();
            Mapper.CreateMap<ViewModels.CourseCreate, Models.Course>();
            Mapper.CreateMap<Models.Course, ViewModels.CourseCreate>();
            Mapper.CreateMap<ViewModels.CourseCreate, ViewModels.CourseFull>();
            Mapper.CreateMap<ViewModels.CourseFull, ViewModels.CourseCreate>();
            Mapper.CreateMap<ViewModels.CancellationCreate, Models.Cancellation>();
            Mapper.CreateMap<Models.Cancellation, ViewModels.CancellationCreate>();
            Mapper.CreateMap<ViewModels.MessageCreate, Models.Message>();
            Mapper.CreateMap<Models.Message, ViewModels.MessageCreate>();
            Mapper.CreateMap<ViewModels.StudentCreate, Models.Student>();
            Mapper.CreateMap<Models.Student, ViewModels.StudentCreate>();
            Mapper.CreateMap<ViewModels.ContactCreate, Models.Contact>();
            Mapper.CreateMap<Models.Contact, ViewModels.ContactCreate>();

        }
    }
}