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
    public class MessageGroupInstanceTest
    {
        public static void Begin(MessageGroupInstance messageGroupInstance)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // MessageGroupInstance API
            List<MessageGroupInstance> messageGroupInstances;

            // Get existing 
            beforeCount = MessageGroupInstanceDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            messageGroupInstance = MessageGroupInstanceDAO.PostUpdate(messageGroupInstance);

            // Reading: Use GetMessageGroupInstances() to retrieve a list of obj
            messageGroupInstances = MessageGroupInstanceDAO.Get();

            // get master item count
            afterCount = messageGroupInstances.Count;

            // write
            MessageGroupInstanceTest.Write(messageGroupInstances, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // messageGroupInstance.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            messageGroupInstance = MessageGroupInstanceDAO.PostUpdate(messageGroupInstance);

            // Reading: Use GetMessageGroupInstances() to retrieve a list of obj
            messageGroupInstances = MessageGroupInstanceDAO.Get();

            // Get existing 
            afterCount = MessageGroupInstanceDAO.Get().Count;

            // write
            MessageGroupInstanceTest.Write(messageGroupInstances, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetMessageGroupInstances() to retrieve a list of obj w/ 1 item
            messageGroupInstances = MessageGroupInstanceDAO.Get(messageGroupInstance);

            // get count 
            afterCount = messageGroupInstances.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            MessageGroupInstanceTest.Write(messageGroupInstances, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            MessageGroupInstanceDAO.Delete(messageGroupInstance);

            // Reading: Use GetMessageGroupInstances() to retrieve a list of obj
            messageGroupInstances = MessageGroupInstanceDAO.Get();

            // get count
            afterCount = messageGroupInstances.Count;

            // write
            MessageGroupInstanceTest.Write(messageGroupInstances, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<MessageGroupInstance> messageGroupInstances, string action, int input, int output, bool isOkToBeDifferent = false)
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
