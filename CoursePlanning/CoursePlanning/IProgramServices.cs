using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CoursePlanning.Controllers;

namespace CoursePlanning
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IProgramServices" in both code and config file together.
    [ServiceContract]
    public interface IProgramServices
    {
        [OperationContract]
        IEnumerable<ProgramBase> AllPrograms();

        [OperationContract]
        ProgramBase GetProgramById(int id);

        [OperationContract]
        ProgramBase GetProgramByCode(string code);

        [OperationContract]
        IEnumerable<CourseBase> GetCoursesByProgram(int programId);

        [OperationContract]
        ProgramBase AddProgram(ProgramAdd newItem);

        [OperationContract]
        ProgramBase EditProgram(ProgramEdit editedItem);

        [OperationContract]
        bool EditCurriculumPlanByProgram(int programId, int curriculumPlanId);

        [OperationContract]
        bool AddCourseToProgram(int courseId, int programId);

        [OperationContract]
        bool RemoveProgramFromProgram(int courseId, int programId);

        [OperationContract]
        bool DeleteProgram(int courseId);
    }
}
