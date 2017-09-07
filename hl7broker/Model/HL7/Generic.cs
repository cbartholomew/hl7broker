using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Broker.Utility;

namespace HL7Broker.Model.HL7
{
    public class Generic : GenericUtility
    {

        public int position { get; set; }
        public string value { get; set; }
        
        // define the hl7 splits
        public const char PERIOD = '.';
        public const char TILDE = '~';
        public const char CARROT = '^';
        public const char AMP = '&';
        public const char ESCAPE = '/';
        public const char PIPE = '|';
        public const int LF = 10;
        public const int CR = 13;
        public const char VT    = '\u000B';
        public const char FS    = '\u001C';
        public const char CR2   = '\u000D';
        public const char TAB = '\t';
        public const string PERIOD_STR = ".";

        // message position template
        public const string COORDINATES_TEMPLATE = "FIELD.COMPONENT.SUB.REPETITION";
        public const string REMOVE_SEGMENT_PART = "SEGMENT";
        public const string REMOVE_MESSAGE_PART = "MESSAGE";

        // message type constants
        public const string  MESSAGE_TYPE_ADT = "ADT";
        public const string  MESSAGE_TYPE_ORM = "ORM";
        public const string  MESSAGE_TYPE_ORU = "ORU";
        public const string  MESSAGE_TYPE_SIU = "SIU";
        public const string  MESSAGE_TYPE_UNK = "UNK";

        // segment type constants
        public const string SEGMENT_TYPE_MSH = "MSH";
        public const string SEGMENT_TYPE_PID = "PID";
        public const string SEGMENT_TYPE_PD1 = "PD1";
        public const string SEGMENT_TYPE_PV1 = "PV1";
        public const string SEGMENT_TYPE_PV2 = "PV2";
        public const string SEGMENT_TYPE_ORC = "ORC";
        public const string SEGMENT_TYPE_OBR = "OBR";
        public const string SEGMENT_TYPE_OBX = "OBX";
        public const string SEGMENT_TYPE_GT1 = "GT1";
        public const string SEGMENT_TYPE_IN1 = "IN1";
        public const string SEGMENT_TYPE_IN2 = "IN2";
        public const string SEGMENT_TYPE_IN3 = "IN3";
        public const string SEGMENT_TYPE_ZD1 = "ZD1";
        public const string SEGMENT_TYPE_ZD2 = "ZD2";
        public const string SEGMENT_TYPE_UNK = "UNK";

        // app table identities
        public const int APP_MESSAGE_HEADER_INSTANCE = 1;
        public const int APP_MESSAGE                 = 3;

        // column data types
        public const string TEXT_TYPE       = "TEXT";
        public const string DATE_TYPE       = "DATE";
        public const string INTEGER_TYPE    = "INTEGER";

        // outbound handler: SHIELDS EXPRESS LINK (SHOULD MOVE TO CONFIG)
        public const string RSERVER = "RSERVER";
        public const string RSERVER_TEST = "RSERVER_TEST";
        public const string RADIOLOGIST_TYPE = "R";
        public const int ENTERPRISE_IDENTITY = 1;

        public enum MessageType
        {
            ADT,
            ORM,
            ORU,
            SIU,
            UNKNOWN
        }

        public enum SegmentType
        {
            MSH,
            EVN,
            PID,
            PD1,
            PV1,
            PV2,
            ORC,
            OBR,
            OBX,
            GT1,
            IN1,
            IN2,
            IN3,
            ZD1,
            ZD2,
            UNKNOWN
        }

    }
}
