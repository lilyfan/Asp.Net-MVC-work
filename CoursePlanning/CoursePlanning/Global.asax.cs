using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
// more...
using AutoMapper;

namespace CoursePlanning
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Load the data store initializer (for simple projects)
            System.Data.Entity.Database.SetInitializer(new Models.StoreInitializer());

            // HTTP OPTIONS method handler
            GlobalConfiguration.Configuration.MessageHandlers.Add(new Handlers.HttpOptionsMethodHandler());

            // AutoMapper maps
            // These define mappings between 'design model' and 'resource model' classes
            // Remember, we never work with 'design model' classes in our controllers 
            // and end-user 'use cases'

            // Models to Controllers base
            // Controllers add to Models
            // Controllers base to Controllers with link

            Mapper.CreateMap<Models.Course, Controllers.CourseBase>();
            Mapper.CreateMap<Models.Course, Controllers.CourseList>();
            Mapper.CreateMap<Controllers.CourseAdd, Models.Course>();
            Mapper.CreateMap<Controllers.CourseBase, Controllers.CourseWithLink>();

            Mapper.CreateMap<Models.Program, Controllers.ProgramBase>();
            Mapper.CreateMap<Models.Program, Controllers.ProgramList>();
            Mapper.CreateMap<Models.Program, Controllers.ProgramAdd>();
            Mapper.CreateMap<Models.Program, Controllers.ProgramEdit>();
            Mapper.CreateMap<Controllers.ProgramAdd, Models.Program>();
            Mapper.CreateMap<Controllers.ProgramBase, Controllers.ProgramWithLink>();
            Mapper.CreateMap<Controllers.ProgramAdd, Controllers.ProgramBase>();
            Mapper.CreateMap<Controllers.ProgramBase, Controllers.ProgramAdd>();
            Mapper.CreateMap<Controllers.ProgramBase, Controllers.ProgramList>();
            Mapper.CreateMap<Controllers.ProgramList, Controllers.ProgramBase>();

            Mapper.CreateMap<Models.CurriculumPlan, Controllers.CurriculumPlanBase>();
            Mapper.CreateMap<Models.CurriculumPlan, Controllers.CurriculumPlanList>();
            Mapper.CreateMap<Controllers.CurriculumPlanAdd, Models.CurriculumPlan>();
            Mapper.CreateMap<Controllers.CurriculumPlanBase, Controllers.CurriculumPlanWithLink>();
            Mapper.CreateMap<Controllers.CurriculumPlanBase, Controllers.CurriculumPlanList>();
            Mapper.CreateMap<Controllers.CurriculumPlanList, Controllers.CurriculumPlanBase>();
            Mapper.CreateMap<Controllers.CurriculumPlanList, Models.CurriculumPlan>();
            Mapper.CreateMap<Models.CurriculumPlan, Controllers.CurriculumPlanEdit>();
            Mapper.CreateMap<Controllers.CurriculumPlanEdit, Models.CurriculumPlan>();

            Mapper.CreateMap<Models.Position, Controllers.PositionBase>();
            Mapper.CreateMap<Models.Position, Controllers.PositionList>();
            Mapper.CreateMap<Controllers.PositionAdd, Models.Position>();
            Mapper.CreateMap<Controllers.PositionBase, Controllers.PositionWithLink>();

        }
    }
}
