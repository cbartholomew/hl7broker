using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Settings;

namespace HL7BrokerSuite.Utility
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
        public const string COMMA_STR = ",";

        // generic constants
        public const string BLANK = "";

        // log constants
        public const string ZERO = "0";
        public const string LOG_TEMPLATE = "Date:[DATE_TIME],[MESSAGE]";

        // element
        public const int FIRST_ELEMENT_INDEX = 0;

        // DAL constants
        public const string TABLE               = "#TABLE#";
        public const string COLUMNS             = "#COLUMNS#";
        public const string COLUMN_VALUES       = "#COLUMN_VALUES#";
        public const string DATA_TYPE_TEXT      = "TEXT";
        public const string DATA_TYPE_INTEGER   = "INTEGER";
        public const string DATA_TYPE_CURRENT_DATE = "DATE";


    }
}
