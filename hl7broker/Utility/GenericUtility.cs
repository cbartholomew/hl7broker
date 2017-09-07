using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using HL7Broker.Model.HL7;

namespace HL7Broker.Utility
{
    public class GenericUtility
    {
        // generic constants
        public const string BLANK = "";

        // log constants
        public const string ZERO_STR = "0";
        public const int ZERO = 0;
        public const int ONE = 1;
        public const int NEG_ONE = -1;
        public const int ZERO_ELEMENT = 0;
        public const int FIRST_ELEMENT = 1;
        public const int ERROR_CODE_NINE = 9999;        
        public const string LOG_TEMPLATE = "Date:[DATE_TIME],[MESSAGE]";
        public const string LOG_TEMPLATE_NO_DATE = "[MESSAGE]";
        public const string AT_SIGN = "@";
        public const int FIELD_ELEMENT  = 0;
        public const int COMPONENT_ELEMENT = 1;
        public const int SUB_COMPONENT_ELEMENT  = 2;
        public const int REPETITION_ELEMENT = 3;

        // Report Constants
        public const int REPORT_OBX_DTTM = 14;
        public const int REPORT_TEXT_ELEMENT = 5;
        public const int REPORT_ITERATION_ELEMENT = 1;
        public const int REPORT_TYPE_ELEMENT = 11;
        public const string ADDENDUM = "A";
        public const string ORIGINAL_REPORT_ITERATION = "1";

        // ACK Constants
        // positions
        public const int    SEGMENT_POSITION = 0;
        public const string MESSAGE_TYPE_POSITION = "9.1";
        public const string MESSAGE_DATE_STAMP_POSITION = "7";
        public const string MESSAGE_HEADER_CONTROL_ID = "10";
        public const string MESSAGE_HEADER_APPLICATION_NAME = "HL7Broker";
        public const string MESSAGE_HEADER_FACILITY_NAME = "SEL";
        public const string ERROR_CONTROL_ID = "99999999";
        public const string ERROR_NO_MESSAGE_TYPE = "The message type is not defined in the MSH segment header";
        public const string ERROR_MESSAGE_FROM_BROKER = "An error has occured on the broker server, please contact your application administrator.";
        public const string ERROR_MESSAGE_REJECTED = "This message has been rejected by the receiving application, please contact your application administrator.";
        public const string MESSAGE_NO_ERROR = "OK";
        public const string ENTERPRISE = "Enterprise";

        public static int GetCurrentManagedThreadId()
        {
            return Process.GetCurrentProcess().Id;
        }

        public static string GetHL7TextForServiceMessage(string sourceMessage, string output, string result, int enterpriseId)
        {
            return String.Concat(sourceMessage, 
                                 Generic.CARROT, 
                                 output,
                                 Generic.AMP,
                                 result,
                                 Generic.AMP,
                                 enterpriseId,
                                 Generic.PIPE);
        }
    }
}
