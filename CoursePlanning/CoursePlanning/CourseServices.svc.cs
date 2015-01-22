using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CoursePlanning.Controllers;

namespace CoursePlanning
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CourseServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CourseServices.svc or CourseServices.svc.cs at the Solution Explorer and start debugging.
    public class CourseServices : ICourseServices
    {
        ServiceLayer.Worker m = new ServiceLayer.Worker();
        public IEnumerable<CourseBase> AllCourses()
        {
            // Use the repository method to get all employees
            return m.Courses.GetAll();
        }

        public CourseBase GetCourseById(int id)
        {
            return m.Courses.GetById(id);
        }

        public CourseBase GetCourseByCode(string code)
        {
            return m.Courses.GetByCode(code);
        }

        public IEnumerable<ProgramBase> GetProgramsByCourse(int courseId)
        {
            return m.Courses.GetProgramList(courseId);
        }

        public CourseBase AddCourse(CourseAdd newItem)
        {
            return m.Courses.AddNew(newItem); 
        }

        public CourseBase EditCourse(CourseEdit editedItem)
        {
            return m.Courses.EditExisting(editedItem);
        }

        public bool AddProgramToCourse(int courseId, int programId)
        {
            return m.Courses.AddProgram(courseId, programId);
        }

        public bool RemoveProgramFromCourse(int courseId, int programId)
        {
            return m.Courses.RemoveProgram(courseId, programId);
        }

        public bool DeleteCourse(int courseId)
        {
            return m.Courses.DeleteExisting(courseId);
        }
    }
}
