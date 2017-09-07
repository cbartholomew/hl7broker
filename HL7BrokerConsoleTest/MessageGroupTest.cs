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
    public class MessageGroupTest
    {
        public static void Begin(MessageGroup messageGroup)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // MessageGroup API
            List<MessageGroup> messageGroups;

            // Get existing 
            beforeCount = MessageGroupDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            messageGroup = MessageGroupDAO.PostUpdate(messageGroup);

            // Reading: Use GetMessageGroups() to retrieve a list of obj
            messageGroups = MessageGroupDAO.Get();

            // get master item count
            afterCount = messageGroups.Count;

            // write
            MessageGroupTest.Write(messageGroups, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // messageGroup.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            messageGroup = MessageGroupDAO.PostUpdate(messageGroup);

            // Reading: Use GetMessageGroups() to retrieve a list of obj
            messageGroups = MessageGroupDAO.Get();

            // Get existing 
            afterCount = MessageGroupDAO.Get().Count;

            // write
            MessageGroupTest.Write(messageGroups, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetMessageGroups() to retrieve a list of obj w/ 1 item
            messageGroups = MessageGroupDAO.Get(messageGroup);

            // get count 
            afterCount = messageGroups.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            MessageGroupTest.Write(messageGroups, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            MessageGroupDAO.Delete(messageGroup);

            // Reading: Use GetMessageGroups() to retrieve a list of obj
            messageGroups = MessageGroupDAO.Get();

            // get count
            afterCount = messageGroups.Count;

            // write
            MessageGroupTest.Write(messageGroups, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<MessageGroup> messageGroups, string action, int input, int output, bool isOkToBeDifferent = false)
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
