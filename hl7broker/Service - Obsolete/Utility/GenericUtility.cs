using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.Service.Utility
{
    public class GenericUtility
    {
        // define the hl7 spit
        public const char TILDE = '~';
        public const char CARROT = '^';
        public const char AMP = '&';
        public const char PIPE = '|';
        public const char ESCAPE = '/';
        public const int NL = 10;
        public const int CR = 13;

        // generic constants
        public const string BLANK = "";

        // log constants
        public const string ZERO = "0";
        public const string LOG_TEMPLATE = "Date:[DATE_TIME],[MESSAGE]";
    }
}
