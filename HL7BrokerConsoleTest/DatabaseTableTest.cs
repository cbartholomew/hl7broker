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
    public class DatabaseTableTest
    {
        public static void Begin(DatabaseTable databaseTable)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // DatabaseTable API
            List<DatabaseTable> databaseTables;

            // Get existing 
            beforeCount = DatabaseTableDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            databaseTable = DatabaseTableDAO.PostUpdate(databaseTable);

            // Reading: Use GetDatabaseTables() to retrieve a list of obj
            databaseTables = DatabaseTableDAO.Get();

            // get master item count
            afterCount = databaseTables.Count;

            // write
            DatabaseTableTest.Write(databaseTables, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // databaseTable.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            databaseTable = DatabaseTableDAO.PostUpdate(databaseTable);

            // Reading: Use GetDatabaseTables() to retrieve a list of obj
            databaseTables = DatabaseTableDAO.Get();

            // Get existing 
            afterCount = DatabaseTableDAO.Get().Count;

            // write
            DatabaseTableTest.Write(databaseTables, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetDatabaseTables() to retrieve a list of obj w/ 1 item
            databaseTables = DatabaseTableDAO.Get(databaseTable);

            // get count 
            afterCount = databaseTables.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            DatabaseTableTest.Write(databaseTables, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            DatabaseTableDAO.Delete(databaseTable);

            // Reading: Use GetDatabaseTables() to retrieve a list of obj
            databaseTables = DatabaseTableDAO.Get();

            // get count
            afterCount = databaseTables.Count;

            // write
            DatabaseTableTest.Write(databaseTables, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<DatabaseTable> databaseTables, string action, int input, int output, bool isOkToBeDifferent = false)
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
