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
    public class InterfaceTest
    {
        public static void Begin(Interface _interface)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // Interface API
            List<Interface> _interfaces;

            // Get existing 
            beforeCount = InterfaceDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            _interface = InterfaceDAO.PostUpdate(_interface);

            // Reading: Use GetInterfaces() to retrieve a list of obj
            _interfaces = InterfaceDAO.Get();

            // get master item count
            afterCount = _interfaces.Count;

            // write
            InterfaceTest.Write(_interfaces, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // _interface.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            _interface = InterfaceDAO.PostUpdate(_interface);

            // Reading: Use GetInterfaces() to retrieve a list of obj
            _interfaces = InterfaceDAO.Get();

            // Get existing 
            afterCount = InterfaceDAO.Get().Count;

            // write
            InterfaceTest.Write(_interfaces, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetInterfaces() to retrieve a list of obj w/ 1 item
            _interfaces = InterfaceDAO.Get(_interface);

            // get count 
            afterCount = _interfaces.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            InterfaceTest.Write(_interfaces, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            InterfaceDAO.Delete(_interface);

            // Reading: Use GetInterfaces() to retrieve a list of obj
            _interfaces = InterfaceDAO.Get();

            // get count
            afterCount = _interfaces.Count;

            // write
            InterfaceTest.Write(_interfaces, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<Interface> _interfaces, string action, int input, int output, bool isOkToBeDifferent = false)
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
