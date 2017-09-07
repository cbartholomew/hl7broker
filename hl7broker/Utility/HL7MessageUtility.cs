using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Broker.Model;
using HL7Broker.Model.HL7;
using HL7Broker.DAO;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Sys.DAO;

namespace HL7Broker.Utility 
{
    public class HL7MessageUtility : Generic
    {
        public static Field getEmptyLocationNotFound()
        {
            string STREET_ONE = "NOT_FOUND";
            string STREET_TWO = "NOT_FOUND";
            string CITY = "NOT_FOUND";
            string STATE = "UK";
            string ZIP = "99999";

            string fieldValue = String.Concat(STREET_ONE, CARROT, STREET_TWO, CARROT, CITY, CARROT, STATE, CARROT, ZIP);

            Field emptyLocationField = new Field();
            emptyLocationField.value = fieldValue;
            emptyLocationField.position = 11;

            emptyLocationField.components.Add(new Component() { position = 0, value = BLANK });
            emptyLocationField.components.Add(new Component() { position = 1, value = STREET_ONE});
            emptyLocationField.components.Add(new Component() { position = 2, value = STREET_TWO});
            emptyLocationField.components.Add(new Component() { position = 3, value = CITY});
            emptyLocationField.components.Add(new Component() { position = 4, value = STATE});
            emptyLocationField.components.Add(new Component() { position = 5, value = ZIP });

            return emptyLocationField;

        }
        public static MessageType getMessageType(HL7Message message, string hl7Input)
        {          
            if (hl7Input == BLANK)
                throw new ArgumentException(ERROR_NO_MESSAGE_TYPE);

            Dictionary<string, MessageType> messageTypeDictionary = new Dictionary<string, MessageType>() 
            { 
                 {MESSAGE_TYPE_ADT, MessageType.ADT},
                 {MESSAGE_TYPE_ORM, MessageType.ORM},
                 {MESSAGE_TYPE_ORU, MessageType.ORU},
                 {MESSAGE_TYPE_SIU, MessageType.SIU},
                 {MESSAGE_TYPE_UNK, MessageType.UNKNOWN}
            };

            // get the message type based on the configuration value
            MessageType result;
            messageTypeDictionary.TryGetValue(HL7MessageUtility.getValueByPosition(message, 
                                                         SegmentType.MSH, 
                                                         MESSAGE_TYPE_POSITION), 
                                                         out result);

            // based on the configuration file, we'll return the message type
            return result;
        }

        public static SegmentType getSegmentType(string segmentType)
        {
            Dictionary<string, SegmentType> segmentTypeDictionary = new Dictionary<string, SegmentType>() 
            { 
                 {SEGMENT_TYPE_MSH, SegmentType.MSH },
                 {SEGMENT_TYPE_PID, SegmentType.PID },
                 {SEGMENT_TYPE_PD1, SegmentType.PD1 },
                 {SEGMENT_TYPE_PV1, SegmentType.PV1 },
                 {SEGMENT_TYPE_PV2, SegmentType.PV2 },
                 {SEGMENT_TYPE_ORC, SegmentType.ORC },
                 {SEGMENT_TYPE_OBR, SegmentType.OBR },
                 {SEGMENT_TYPE_OBX, SegmentType.OBX },
                 {SEGMENT_TYPE_GT1, SegmentType.GT1 },
                 {SEGMENT_TYPE_IN1, SegmentType.IN1 },
                 {SEGMENT_TYPE_IN2, SegmentType.IN2 },
                 {SEGMENT_TYPE_IN3, SegmentType.IN3 },
                 {SEGMENT_TYPE_ZD1, SegmentType.ZD1 },
                 {SEGMENT_TYPE_ZD2, SegmentType.ZD2 },
                 {SEGMENT_TYPE_UNK, SegmentType.UNKNOWN }
            };

            SegmentType result = SegmentType.UNKNOWN;
            segmentTypeDictionary.TryGetValue(segmentType, out result);

            // return the segment type
            return result;
        }

        public static List<string> getDelimitedData(string input, char delimiter)
        {
            // parse each hl7 segment by delimiter character
            List<string> delimiterResult = new List<string>();

            // split
            delimiterResult = input.Split(delimiter).ToList();

            // return delimited data
            return delimiterResult;
        }

        public static List<T> Parse<T>(string input, 
                                        char delimiter, 
                                        List<T> dataSet, 
                                        T data)
        {
            List<string> delimiterResult = HL7MessageUtility.getDelimitedData(input, delimiter);

            for (int inputIndex = ZERO, delimiterLen = delimiterResult.Count;
                 inputIndex < delimiterLen;
                 inputIndex++)
            {
                // get the data set alone
               
            }

            return dataSet;
        }

        public static string getItemCoordinates(List<Configuration> hl7MessagePosition)
        {
            List<string> coordinates = new List<string>();

            // get the message part
            List<MessagePart> messageParts = MessagePartDAO.Get();

            // remove segment and message delimiters from dataset
            messageParts.RemoveAll(i => i.name == REMOVE_MESSAGE_PART || 
                                   i.name == REMOVE_SEGMENT_PART);

            // for each message part in the db - swap it's matching position
            messageParts.ForEach(delegate(MessagePart messagePart)
            {
                // message part id - search for element
                int messagePartId = messagePart.id;
                // message part name - use for replace
                string messagePartName = messagePart.name;
                // reset to negative one
                int value = -1;
                
                // based on the message part identity, get position for that value 
                value = hl7MessagePosition.Find(p => 
                                               p.messagePart.id == messagePartId)
                                               .messageGroup.position;

                // if the value is found - add
                if (value != ZERO)
                {
                    coordinates.Add(value.ToString());
                }

            });

            return (coordinates.Count > 0) ? String.Join(PERIOD_STR, coordinates.ToArray())
                                           : BLANK;
        }

        public static string getItemCoordinatesByCachedConfig(List<MessageGroup> hl7MessagePosition, 
                                                              List<MessagePart> messageParts)
        {
            List<string> coordinates = new List<string>();

            // remove segment and message delimiters from dataset
            messageParts.RemoveAll(i => i.name == REMOVE_MESSAGE_PART ||
                                   i.name == REMOVE_SEGMENT_PART);

            // for each message part in the db - swap it's matching position
            messageParts.ForEach(delegate(MessagePart messagePart)
            {
                // message part id - search for element
                int messagePartId = messagePart.id;
                // message part name - use for replace
                string messagePartName = messagePart.name;
                // reset to negative one
                int value = -1;

                // based on the message part identity, get position for that value 
                value = hl7MessagePosition.Find(p =>
                                               p.messagePartId == messagePartId)
                                               .position;

                // if the value is found - add
                if (value != ZERO)
                {
                    coordinates.Add(value.ToString());
                }

            });

            return (coordinates.Count > 0) ? String.Join(PERIOD_STR, coordinates.ToArray())
                                           : BLANK;
        }
       
        public static string getValueByPosition(HL7Message message,
           SegmentType segmentType,
           string position,
           int segmentOrder = NEG_ONE)
        {
            string[] positionMap;

            // if there is a period, we are looking through multiple values
            if (position.Contains(PERIOD))
            {
                positionMap = position.Split(PERIOD);
            }
            else
            {
                // we are only interested in a field data
                positionMap = new string[ONE];
                positionMap[ZERO] = position;
            }

            // associates the field no  to the mapped value below
            int fieldNo = ZERO;
            int componentNo = ZERO;
            int subComponentNo = ZERO;
            int repetitionNo = ZERO;

            // gets the length (and determines the case) based on the map (3.1.1.1)
            int mapCount = (position != BLANK) ? positionMap.Length : ZERO;

            switch (mapCount)
            {
                case 1:
                    // map fields
                    fieldNo = Convert.ToInt32(positionMap[FIELD_ELEMENT]);

                    // return value in text only
                    return Field.getFieldByPos(message, segmentType, fieldNo, segmentOrder).value;
                case 2:
                    // map fields
                    fieldNo = Convert.ToInt32(positionMap[FIELD_ELEMENT]);
                    componentNo = Convert.ToInt32(positionMap[COMPONENT_ELEMENT]);

                    // set classes 
                    Field c2f = Field.getFieldByPos(message, segmentType, fieldNo);
                    
                    // return value in text only
                    return Component.getComponentByPos(c2f, componentNo).value;
                case 3:
                    // map fields
                    fieldNo = Convert.ToInt32(positionMap[FIELD_ELEMENT]);
                    componentNo = Convert.ToInt32(positionMap[COMPONENT_ELEMENT]);
                    subComponentNo = Convert.ToInt32(positionMap[SUB_COMPONENT_ELEMENT]);

                    // set classes 
                    Field c3f = Field.getFieldByPos(message, segmentType, fieldNo);
                    Component c3c = Component.getComponentByPos(c3f, componentNo);

                    // return value in text only
                    return SubComponent.getSubComponentByPos(c3c, subComponentNo).value;
                case 4:
                    // map fields
                    fieldNo = Convert.ToInt32(positionMap[FIELD_ELEMENT]);
                    componentNo = Convert.ToInt32(positionMap[COMPONENT_ELEMENT]);
                    subComponentNo = Convert.ToInt32(positionMap[SUB_COMPONENT_ELEMENT]);
                    repetitionNo = Convert.ToInt32(positionMap[REPETITION_ELEMENT]);

                    // set classes 
                    Field c4f = Field.getFieldByPos(message, segmentType, fieldNo);
                    Component c4c = Component.getComponentByPos(c4f, componentNo);
                    SubComponent c4sc = SubComponent.getSubComponentByPos(c4c, subComponentNo);

                    // return value in text only
                    return Repetition.getRepetitionByPos(c4sc, repetitionNo).value;
                default:
                    // cant' find it
                    return BLANK;
            }
        }

        public static string getAck(string ackType, 
                                    string messageControlId, 
                                    string dateStamp,
                                    string application,
                                    string facility,
                                    string message)
        {
            List<string> hl7Ack = new List<string>() { 
                {
                    "MSH|^~\\&|" 
                    +  application      + "|" 
                    +  facility         + "|" 
                    +  facility         + "|" 
                    +  application      + "|" 
                    +  dateStamp        + "||ACK^O01|" 
                    + messageControlId  + "|P|2.3"
                },
                {
                    "MSA|" 
                    + ackType + "|" 
                    + messageControlId + "|" 
                    + message
                }
            };

            return String.Concat(hl7Ack[0], 
                                Convert.ToChar(Generic.CR), 
                                Convert.ToChar(Generic.LF), 
                                hl7Ack[1]);
        }

        public static string padHL7MessageForTransfer(string hl7Message)
        {
            if (hl7Message == BLANK) 
            {
                return BLANK;
            }

            return String.Concat(Generic.VT,
                                hl7Message,
                                Generic.FS,
                                Generic.CR2);
        }

        public static string scrubHL7MessageForParse(string hl7Message) 
        {           
            hl7Message = hl7Message.Replace(Generic.VT.ToString(), BLANK);
            hl7Message = hl7Message.Replace(Generic.FS.ToString(), BLANK);
            hl7Message = hl7Message.Replace(Generic.TAB.ToString(), BLANK);
            return hl7Message;
        }

        public static int adjustIndexToMatchPosition(int position)
        { 
            return (position != ZERO) ? position - ONE : position;            
        }
    }
}
