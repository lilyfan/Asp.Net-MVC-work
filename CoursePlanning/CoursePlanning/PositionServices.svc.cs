using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CoursePlanning.Controllers;

namespace CoursePlanning
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PositionServices" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PositionServices.svc or PositionServices.svc.cs at the Solution Explorer and start debugging.
    public class PositionServices : IPositionServices
    {
        ServiceLayer.Worker m = new ServiceLayer.Worker();
        public IEnumerable<PositionBase> AllPositions()
        {
            return m.Positions.GetAll();
        }

        public PositionBase GetPositionById(int id)
        {
            return m.Positions.GetById(id);
        }

        public PositionBase AddPosition(PositionAdd newItem)
        {
            return m.Positions.AddNew(newItem);
        }

        public PositionBase EditPosition(PositionEdit editedItem)
        {
            return m.Positions.EditExisting(editedItem);
        }

        public bool DeletePosition(int id)
        {
            return m.Programs.DeleteExisting(id);
        }
    }
}
