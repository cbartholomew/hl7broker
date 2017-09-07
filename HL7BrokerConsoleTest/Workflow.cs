using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Sys.Model;

namespace HL7BrokerConsoleTest
{
    public class Workflow
    {
        public static void Begin()
        {
            List<Application> applications = ApplicationDAO.Get();
            List<Communication> communications = CommunicationDAO.Get();
            List<CommunicationType> communicationTypes = CommunicationTypeDAO.Get();
            List<DirectionType> directionTypes = DirectionTypeDAO.Get();

            foreach (Application app in applications)
            {                
                // for each application, initalize inbound interface connections

                        
            }
        
        }        
    }
}
