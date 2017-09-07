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
    public class DatabaseInstanceTest
    {
        public static void Begin(DatabaseInstance databaseInstance)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // DatabaseInstance API
            List<DatabaseInstance> databaseInstances;

            // Get existing 
            beforeCount = DatabaseInstanceDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            databaseInstance = DatabaseInstanceDAO.PostUpdate(databaseInstance);

            // Reading: Use GetDatabaseInstances() to retrieve a list of obj
            databaseInstances = DatabaseInstanceDAO.Get();

            // get master item count
            afterCount = databaseInstances.Count;

            // write
            DatabaseInstanceTest.Write(databaseInstances, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // databaseInstance.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            databaseInstance = DatabaseInstanceDAO.PostUpdate(databaseInstance);

            // Reading: Use GetDatabaseInstances() to retrieve a list of obj
            databaseInstances = DatabaseInstanceDAO.Get();

            // Get existing 
            afterCount = DatabaseInstanceDAO.Get().Count;

            // write
            DatabaseInstanceTest.Write(databaseInstances, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetDatabaseInstances() to retrieve a list of obj w/ 1 item
            databaseInstances = DatabaseInstanceDAO.Get(databaseInstance);

            // get count 
            afterCount = databaseInstances.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            DatabaseInstanceTest.Write(databaseInstances, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            DatabaseInstanceDAO.Delete(databaseInstance);

            // Reading: Use GetDatabaseInstances() to retrieve a list of obj
            databaseInstances = DatabaseInstanceDAO.Get();

            // get count
            afterCount = databaseInstances.Count;

            // write
            DatabaseInstanceTest.Write(databaseInstances, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<DatabaseInstance> databaseInstances, string action, int input, int output, bool isOkToBeDifferent = false)
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
