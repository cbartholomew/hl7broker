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
    public class SegmentTypeTest
    {
        public static void Begin(SegmentType messageType)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // SegmentType API
            List<SegmentType> messageTypes;

            // Get existing 
            beforeCount = SegmentTypeDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            messageType = SegmentTypeDAO.PostUpdate(messageType);

            // Reading: Use GetSegmentTypes() to retrieve a list of obj
            messageTypes = SegmentTypeDAO.Get();

            // get master item count
            afterCount = messageTypes.Count;

            // write
            SegmentTypeTest.Write(messageTypes, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // messageType.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            messageType = SegmentTypeDAO.PostUpdate(messageType);

            // Reading: Use GetSegmentTypes() to retrieve a list of obj
            messageTypes = SegmentTypeDAO.Get();

            // Get existing 
            afterCount = SegmentTypeDAO.Get().Count;

            // write
            SegmentTypeTest.Write(messageTypes, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetSegmentTypes() to retrieve a list of obj w/ 1 item
            messageTypes = SegmentTypeDAO.Get(messageType);

            // get count 
            afterCount = messageTypes.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            SegmentTypeTest.Write(messageTypes, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            SegmentTypeDAO.Delete(messageType);

            // Reading: Use GetSegmentTypes() to retrieve a list of obj
            messageTypes = SegmentTypeDAO.Get();

            // get count
            afterCount = messageTypes.Count;

            // write
            SegmentTypeTest.Write(messageTypes, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<SegmentType> messageTypes, string action, int input, int output, bool isOkToBeDifferent = false)
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
