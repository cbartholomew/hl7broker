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
    public class CredentialTypeTest
    {
        public static void Begin(CredentialType credentialType)
        {
            int beforeCount = 0;
            int afterCount = 0;

            // CredentialType API
            List<CredentialType> credentialTypes;

            // Get existing 
            beforeCount = CredentialTypeDAO.Get().Count;

            // Insert and Updating: if ID is included, it will update
            credentialType = CredentialTypeDAO.PostUpdate(credentialType);

            // Reading: Use GetCredentialTypes() to retrieve a list of obj
            credentialTypes = CredentialTypeDAO.Get();

            // get master item count
            afterCount = credentialTypes.Count;

            // write
            CredentialTypeTest.Write(credentialTypes, "INSERT", beforeCount, afterCount, true);
            Console.Read();

            // make a soft update to some property (Optional)
            // credentialType.<property> = 1;

            // re-assign the before count
            beforeCount = afterCount;

            // Insert and Updating: if ID is included, it will update
            credentialType = CredentialTypeDAO.PostUpdate(credentialType);

            // Reading: Use GetCredentialTypes() to retrieve a list of obj
            credentialTypes = CredentialTypeDAO.Get();

            // Get existing 
            afterCount = CredentialTypeDAO.Get().Count;

            // write
            CredentialTypeTest.Write(credentialTypes, "UPDATE", beforeCount, afterCount);
            Console.Read();

            // Reading: Use GetCredentialTypes() to retrieve a list of obj w/ 1 item
            credentialTypes = CredentialTypeDAO.Get(credentialType);

            // get count 
            afterCount = credentialTypes.Count;

            // reassign count
            beforeCount = afterCount;

            // write
            CredentialTypeTest.Write(credentialTypes, "Single", afterCount, 1);
            Console.Read();

            // Deleting - Send in the obj w/ at minimal the ID populated
            CredentialTypeDAO.Delete(credentialType);

            // Reading: Use GetCredentialTypes() to retrieve a list of obj
            credentialTypes = CredentialTypeDAO.Get();

            // get count
            afterCount = credentialTypes.Count;

            // write
            CredentialTypeTest.Write(credentialTypes, "Removed", beforeCount, afterCount, true);
            Console.Read();
        }

        private static void Write(List<CredentialType> credentialTypes, string action, int input, int output, bool isOkToBeDifferent = false)
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
