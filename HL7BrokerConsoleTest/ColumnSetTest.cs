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
    public class ColumnSetTest
    {
        public static void Begin(ColumnSet columnSet)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // ColumnSet API
            List<ColumnSet> columnSets;

            // Get exsisting 
            beforeCount = ColumnSetDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            columnSet = ColumnSetDAO.PostUpdate(columnSet);

            // Reading: Use GetColumnSets() to retrieve a list of columnSetlications
            columnSets = ColumnSetDAO.Get();

            // get master item count
            afterCount = columnSets.Count;

            // write
            ColumnSetTest.Write(columnSets, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property
            columnSet.name = "TEST_UPDATE";

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            columnSet = ColumnSetDAO.PostUpdate(columnSet);

            // Reading: Use GetColumnSets() to retrieve a list of columnSetlications
            columnSets = ColumnSetDAO.Get();

            // Get exsisting 
            afterCount = ColumnSetDAO.Get().Count;

            // write
            ColumnSetTest.Write(columnSets, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // get a single columnSetlication (returns a list)
            columnSets = ColumnSetDAO.Get(columnSet);

            // get count 
            afterCount = columnSets.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            ColumnSetTest.Write(columnSets, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the columnSetlication w/ at minimal the ID populated
            ColumnSetDAO.Delete(columnSet);

            // Reading: Use GetColumnSets() to retreieve a list of columnSetlications
            columnSets = ColumnSetDAO.Get();

            // get count
            afterCount = columnSets.Count;

            // write
            ColumnSetTest.Write(columnSets, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<ColumnSet> columnSets, string action, int input, int output, bool isOkToBeDifferent = false)
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
