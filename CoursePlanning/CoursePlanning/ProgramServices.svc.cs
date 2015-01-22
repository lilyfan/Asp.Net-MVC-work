using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AutoMapper;
using CoursePlanning.Controllers;

namespace CoursePlanning
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProgramServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProgramServices.svc or ProgramServices.svc.cs at the Solution Explorer and start debugging.
    public class ProgramServices : IProgramServices
    {
        ServiceLayer.Worker m = new ServiceLayer.Worker();
        public IEnumerable<ProgramBase> AllPrograms()
        {
            return m.Programs.GetAll(); 
        }

        public ProgramBase GetProgramById(int id)
        {
            return m.Programs.GetById(id);
        }

        public ProgramBase GetProgramByCode(string code)
        {
            return m.Programs.GetByCode(code);
        }

        public IEnumerable<CourseBase> GetCoursesByProgram(int programId)
        {
            return m.Programs.GetCourseList(programId); 
        }

        public ProgramBase AddProgram(ProgramAdd newItem)
        {
            var curriculumPlanId = newItem.AssoCurriculumPlanId;
            var fetchedCP = m.CurriculumPlans.GetById(curriculumPlanId);
            if (fetchedCP == null)
            {
                return null;
            }
            else {
                return m.Programs.AddNew(newItem, Mapper.Map<CurriculumPlanList>(fetchedCP));
            }
            
        }

        public ProgramBase EditProgram(ProgramEdit editedItem)
        {
            return m.Programs.EditExisting(editedItem);
        }

        public bool EditCurriculumPlanByProgram(int programId, int curriculumPlanId)
        {
            return m.Programs.UpdateCurriculumPlan(programId, curriculumPlanId);
        }

        public bool AddCourseToProgram(int programId, int courseId)
        {
            return m.Programs.AddCourse(programId, courseId);
        }

        public bool RemoveProgramFromProgram(int programId, int courseId)
        {
            return m.Programs.RemoveCourse(programId, courseId);
        }

        public bool DeleteProgram(int id)
        {
            return m.Programs.DeleteExisting(id);
        }
    }
}
