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
    public class ApplicationTest
    {
        public static void Begin(Application application)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // Application API
            List<Application> applications;

            // Get exsisting 
            beforeCount = ApplicationDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            application = ApplicationDAO.PostUpdate(application);

            // Reading: Use GetApplications() to retrieve a list of applicationlications
            applications = ApplicationDAO.Get();

            // get master item count
            afterCount = applications.Count;

            // write
            ApplicationTest.Write(applications, "INSERT", beforeCount, afterCount,true);
            Console.Read();

            // make a soft update to some property
            application.name = "TEST_UPDATE";

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            application = ApplicationDAO.PostUpdate(application);

            // Reading: Use GetApplications() to retrieve a list of applicationlications
            applications = ApplicationDAO.Get();

            // Get exsisting 
            afterCount = ApplicationDAO.Get().Count;

            // write
            ApplicationTest.Write(applications, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // get a single applicationlication (returns a list)
            applications = ApplicationDAO.Get(application);

            // get count 
            afterCount = applications.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            ApplicationTest.Write(applications, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the applicationlication w/ at minimal the ID populated
            ApplicationDAO.Delete(application);

            // Reading: Use GetApplications() to retreieve a list of applicationlications
            applications = ApplicationDAO.Get();

            // get count
            afterCount = applications.Count;

            // write
            ApplicationTest.Write(applications, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<Application> applications, string action, int input, int output, bool isOkToBeDifferent = false)
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
