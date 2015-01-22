using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CoursePlanning.Controllers;

namespace CoursePlanning
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICourseServices" in both code and config file together.
    [ServiceContract]
    public interface ICourseServices
    {
        [OperationContract]
        IEnumerable<CourseBase> AllCourses();

        [OperationContract]
        CourseBase GetCourseById(int id);

        [OperationContract]
        CourseBase GetCourseByCode(string code);

        [OperationContract]
        IEnumerable<ProgramBase> GetProgramsByCourse(int courseId);

        [OperationContract]
        CourseBase AddCourse(CourseAdd newItem);

        [OperationContract]
        CourseBase EditCourse(CourseEdit editedItem);

        [OperationContract]
        bool AddProgramToCourse(int courseId, int programId);

        [OperationContract]
        bool RemoveProgramFromCourse(int courseId, int programId);

        [OperationContract]
        bool DeleteCourse(int courseId);


    }
}
