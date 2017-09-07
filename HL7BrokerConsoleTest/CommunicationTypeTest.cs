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
    public class CommunicationTypeTest
    {
        public static void Begin(CommunicationType communicationType)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // CommunicationType API
            List<CommunicationType> communicationTypes;

            // Get exsisting 
            beforeCount = CommunicationTypeDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            communicationType = CommunicationTypeDAO.PostUpdate(communicationType);

            // Reading: Use GetCommunicationTypes() to retrieve a list of communicationTypelications
            communicationTypes = CommunicationTypeDAO.Get();

            // get master item count
            afterCount = communicationTypes.Count;

            // write
            CommunicationTypeTest.Write(communicationTypes, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            communicationType = CommunicationTypeDAO.PostUpdate(communicationType);

            // Reading: Use GetCommunicationTypes() to retrieve a list of communicationTypelications
            communicationTypes = CommunicationTypeDAO.Get();

            // Get exsisting 
            afterCount = CommunicationTypeDAO.Get().Count;

            // write
            CommunicationTypeTest.Write(communicationTypes, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // get a single communicationTypelication (returns a list)
            communicationTypes = CommunicationTypeDAO.Get(communicationType);

            // get count 
            afterCount = communicationTypes.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            CommunicationTypeTest.Write(communicationTypes, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the communicationTypelication w/ at minimal the ID populated
            CommunicationTypeDAO.Delete(communicationType);

            // Reading: Use GetCommunicationTypes() to retreieve a list of communicationTypelications
            communicationTypes = CommunicationTypeDAO.Get();

            // get count
            afterCount = communicationTypes.Count;

            // write
            CommunicationTypeTest.Write(communicationTypes, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<CommunicationType> communicationTypes, string action, int input, int output, bool isOkToBeDifferent = false)
        {
            Console.WriteLine(">>" + action + "<<");
            if (input == output && isOkToBeDifferent == false)
            {
                Console.WriteLine("OK: Input is the same as expected output");
            }
            else if (input == output && isOkToBeDifferent == true)
            {
                Console.WriteLine("WARNING: Input is the same as expected output");
            }
            else if (input != output && isOkToBeDifferent == true)
            {
                Console.WriteLine("OK: Input is NOT the same as expected output");
            }
            else
            {
                Console.WriteLine("WARNING: Input is NOT the same as expected output");
            }
        }

    }
}
