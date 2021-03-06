﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Sys.Model;

namespace HL7BrokerConsoleTest
{
    public class MessagePartTest
    {
        public static void Begin(MessagePart messagePart)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // MessagePart API
            List<MessagePart> messageParts;

            // Get existing 
            beforeCount = MessagePartDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            messagePart = MessagePartDAO.PostUpdate(messagePart);

            // Reading: Use GetMessageParts() to retrieve a list of obj
            messageParts = MessagePartDAO.Get();

            // get master item count
            afterCount = messageParts.Count;

            // write
            MessagePartTest.Write(messageParts, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // messagePart.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            messagePart = MessagePartDAO.PostUpdate(messagePart);

            // Reading: Use GetMessageParts() to retrieve a list of obj
            messageParts = MessagePartDAO.Get();

            // Get existing 
            afterCount = MessagePartDAO.Get().Count;

            // write
            MessagePartTest.Write(messageParts, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetMessageParts() to retrieve a list of obj w/ 1 item
            messageParts = MessagePartDAO.Get(messagePart);

            // get count 
            afterCount = messageParts.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            MessagePartTest.Write(messageParts, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            MessagePartDAO.Delete(messagePart);

            // Reading: Use GetMessageParts() to retrieve a list of obj
            messageParts = MessagePartDAO.Get();

            // get count
            afterCount = messageParts.Count;

            // write
            MessagePartTest.Write(messageParts, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<MessagePart> messageParts, string action, int input, int output, bool isOkToBeDifferent = false)
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
