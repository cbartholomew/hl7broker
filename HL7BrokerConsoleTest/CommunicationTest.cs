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
    public class CommunicationTest
    {
        public static void Begin(Communication communication)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // Communication API
            List<Communication> communications;

            // Get exsisting 
            beforeCount = CommunicationDAO.Get().Count;
     
            // Insert and Updating: if ID is included, it will update
            communication = CommunicationDAO.PostUpdate(communication);

            // Reading: Use GetCommunications() to retrieve a list of communicationlications
            communications = CommunicationDAO.Get();

            // get master item count
            afterCount = communications.Count;

            // write
            CommunicationTest.Write(communications, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property
            communication.applicationId = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            communication = CommunicationDAO.PostUpdate(communication);

            // Reading: Use GetCommunications() to retrieve a list of communicationlications
            communications = CommunicationDAO.Get();

            // Get exsisting 
            afterCount = CommunicationDAO.Get().Count;

            // write
            CommunicationTest.Write(communications, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // get a single communicationlication (returns a list)
            communications = CommunicationDAO.Get(communication);

            // get count 
            afterCount = communications.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            CommunicationTest.Write(communications, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the communicationlication w/ at minimal the ID populated
            CommunicationDAO.Delete(communication);

            // Reading: Use GetCommunications() to retreieve a list of communicationlications
            communications = CommunicationDAO.Get();

            // get count
            afterCount = communications.Count;

            // write
            CommunicationTest.Write(communications, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<Communication> communications, string action, int input, int output, bool isOkToBeDifferent = false)
        {
            Console.WriteLine(">>" + action + "<<");
            if (input == output && isOkToBeDifferent == false)
            {
                Console.WriteLine("OK: Input is the same as expected output");
            }
            else if(input == output && isOkToBeDifferent == true)
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
