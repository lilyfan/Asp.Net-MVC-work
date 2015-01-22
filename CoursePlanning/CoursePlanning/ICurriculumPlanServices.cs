using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CoursePlanning.Controllers;

namespace CoursePlanning
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ICurriculumPlanServices" in both code and config file together.
    [ServiceContract]
    public interface ICurriculumPlanServices
    {
        [OperationContract]
        IEnumerable<CurriculumPlanBase> AllCurriculumPlans();

        [OperationContract]
        CurriculumPlanBase GetCurriculumPlanById(int id);

        [OperationContract]
        CurriculumPlanBase AddCurriculumPlan(CurriculumPlanAdd newItem);

        [OperationContract]
        CurriculumPlanBase EditCurriculumPlan(CurriculumPlanEdit editedItem);

        [OperationContract]
        bool EditPutCurriculumPlanByProgram(int curriculumPlanId, int programId);

        [OperationContract]
        bool AddPositionToCurriculumPlan(int curriculumPlanId, int positionId);

        [OperationContract]
        bool RemovePositionFromCurriculumPlan(int curriculumPlanId, int positionId);

        [OperationContract]
        bool DeleteCurriculumPlan(int id);
    }
}
