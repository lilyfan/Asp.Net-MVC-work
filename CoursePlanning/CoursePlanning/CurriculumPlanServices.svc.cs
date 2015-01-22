using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CoursePlanning.Controllers;

namespace CoursePlanning
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CurriculumPlanServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CurriculumPlanServices.svc or CurriculumPlanServices.svc.cs at the Solution Explorer and start debugging.
    public class CurriculumPlanServices : ICurriculumPlanServices
    {
        ServiceLayer.Worker m = new ServiceLayer.Worker();

        public IEnumerable<CurriculumPlanBase> AllCurriculumPlans()
        {
            return m.CurriculumPlans.GetAll();
        }

        public CurriculumPlanBase GetCurriculumPlanById(int id)
        {
            return m.CurriculumPlans.GetById(id);
        }

        public CurriculumPlanBase AddCurriculumPlan(CurriculumPlanAdd newItem)
        {
            return m.CurriculumPlans.AddNew(newItem);
        }

        public CurriculumPlanBase EditCurriculumPlan(CurriculumPlanEdit editedItem)
        {
            return m.CurriculumPlans.EditExisting(editedItem);
        }

        public bool EditPutCurriculumPlanByProgram(int curriculumPlanId, int programId)
        {
            return m.Programs.UpdateCurriculumPlan(curriculumPlanId, programId);
        }

        public bool AddPositionToCurriculumPlan(int curriculumPlanId, int positionId)
        {
            return m.CurriculumPlans.AddPosition(curriculumPlanId, positionId);
        }

        public bool RemovePositionFromCurriculumPlan(int curriculumPlanId, int positionId)
        {
            return m.Programs.RemoveCourse(curriculumPlanId, positionId);
        }

        public bool DeleteCurriculumPlan(int id)
        {
            return m.CurriculumPlans.DeleteExisting(id);
        }
    }
}
