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
    public class DirectionTypeTest
    {
        public static void Begin(DirectionType directionType)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // DirectionType API
            List<DirectionType> directionTypes;

            // Get existing 
            beforeCount = DirectionTypeDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            directionType = DirectionTypeDAO.PostUpdate(directionType);

            // Reading: Use GetDirectionTypes() to retrieve a list of obj
            directionTypes = DirectionTypeDAO.Get();

            // get master item count
            afterCount = directionTypes.Count;

            // write
            DirectionTypeTest.Write(directionTypes, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // directionType.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            directionType = DirectionTypeDAO.PostUpdate(directionType);

            // Reading: Use GetDirectionTypes() to retrieve a list of obj
            directionTypes = DirectionTypeDAO.Get();

            // Get existing 
            afterCount = DirectionTypeDAO.Get().Count;

            // write
            DirectionTypeTest.Write(directionTypes, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetDirectionTypes() to retrieve a list of obj w/ 1 item
            directionTypes = DirectionTypeDAO.Get(directionType);

            // get count 
            afterCount = directionTypes.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            DirectionTypeTest.Write(directionTypes, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            DirectionTypeDAO.Delete(directionType);

            // Reading: Use GetDirectionTypes() to retrieve a list of obj
            directionTypes = DirectionTypeDAO.Get();

            // get count
            afterCount = directionTypes.Count;

            // write
            DirectionTypeTest.Write(directionTypes, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<DirectionType> directionTypes, string action, int input, int output, bool isOkToBeDifferent = false)
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
