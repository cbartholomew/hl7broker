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
    public class CredentialTest
    {
        public static void Begin(Credential credential)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // Credential API
            List<Credential> credentials;

            // Get existing 
            beforeCount = CredentialDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            credential = CredentialDAO.PostUpdate(credential);

            // Reading: Use GetCredentials() to retrieve a list of obj
            credentials = CredentialDAO.Get();

            // get master item count
            afterCount = credentials.Count;

            // write
            CredentialTest.Write(credentials, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // credential.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            credential = CredentialDAO.PostUpdate(credential);

            // Reading: Use GetCredentials() to retrieve a list of obj
            credentials = CredentialDAO.Get();

            // Get existing 
            afterCount = CredentialDAO.Get().Count;

            // write
            CredentialTest.Write(credentials, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetCredentials() to retrieve a list of obj w/ 1 item
            credentials = CredentialDAO.Get(credential);

            // get count 
            afterCount = credentials.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            CredentialTest.Write(credentials, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            CredentialDAO.Delete(credential);

            // Reading: Use GetCredentials() to retrieve a list of obj
            credentials = CredentialDAO.Get();

            // get count
            afterCount = credentials.Count;

            // write
            CredentialTest.Write(credentials, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<Credential> credentials, string action, int input, int output, bool isOkToBeDifferent = false)
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
