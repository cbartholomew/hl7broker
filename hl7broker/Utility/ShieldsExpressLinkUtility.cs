using HL7Broker.Model.HL7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7Broker.DAO;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Utility;
using System.Text.RegularExpressions;


namespace HL7Broker.Utility
{
    public class ShieldsExpressLinkUtility : Generic
    {
        public const string REPORT_HTML_HEADER = "<HTML><HEAD><TITLE>REPORT</TITLE></HEAD>";
        public const string REPORT_HTML_FOOTER = "</HTML>";
        public const string HTML_LINE_BREAK    = "<BR>";

        // *** ADDENDUM 06/18/2014 03:01PM ***
        public const string ADDENDUM_PATTERN_MATCH_BEGIN = @"^\x2A{3} ADDENDUM \d{2}/\d{2}/\d{4} \d{2}:\d{2}(PM|AM) \x2A{3}$";
        // *** END OF ADDENDUM ***
        public const string ADDENDUM_PATTERN_MATCH_END = @"^\x2A{3} END OF ADDENDUM \x2A{3}$";
        // ** FINAL ADDENDUM **
        public const string HAS_ADDENDUM_PATTERN = @"^\x2A{2} FINAL ADDENDUM \x2A{2}$";
        // ORIGINAL REPORT
        public const string ORIGINAL_REPORT_PATTERN = @"ORIGINAL REPORT";



        public enum ObjectType
	    {
	        Location,
            Patient,	
            Organization,
            Exam,
            Report,
            Referring,
            Radiologist
	    }

        public static T getShieldsAppObject<T>(ObjectType type,
                                                HL7Message message,
                                                List<MessageGroupInstance> messageGroupInstances,
                                                List<MessageGroup> messageGroups,
                                                List<WebservicePropertySet> wsPropertySet)
        {
            Object shieldsAppObject 
                = new Object();

            WSShieldsApps.Exam exam
                = new WSShieldsApps.Exam();
            WSShieldsApps.Location location
                = new WSShieldsApps.Location();
            WSShieldsApps.Patient patient
                = new WSShieldsApps.Patient();
            WSShieldsApps.Organization organization
                = new WSShieldsApps.Organization();
            WSShieldsApps.Report report
                = new WSShieldsApps.Report();
            WSShieldsApps.Doctor referring
                = new WSShieldsApps.Doctor();
            WSShieldsApps.Doctor radiologist
                = new WSShieldsApps.Doctor();

            // for each property, get the hl7 position - then get the value for it
            wsPropertySet.ForEach(delegate(WebservicePropertySet wsProperty)
            {
                // cleaner way to get the value that ca be used by all methods here
                string hl7Value = ShieldsExpressLinkUtility.getHL7ValueSEL(message,
                                                 messageGroupInstances,
                                                 messageGroups,
                                                 wsProperty);

                switch (type)
                {
                    case ObjectType.Location:
                        location = getLocation(wsProperty, hl7Value, location);
                        shieldsAppObject = location;
                        break;
                    case ObjectType.Patient:
                        patient = getPatient(wsProperty, hl7Value, patient);
                        shieldsAppObject = patient;
                        break;
                    case ObjectType.Organization:
                        organization = getOrganization(wsProperty, hl7Value, organization);
                        shieldsAppObject = organization;
                        break;
                    case ObjectType.Exam:
                        exam = getExam(wsProperty, hl7Value, exam);
                        shieldsAppObject = exam;
                        break;
                    case ObjectType.Report:
                        report = getReport(wsProperty, message, hl7Value, report);
                        report.ReportText = getReportText(message,true);
                        shieldsAppObject = report;
                        break;
                    case ObjectType.Referring:
                        referring = getReferringDoctor(wsProperty, hl7Value, referring);
                        shieldsAppObject = referring;
                        break;
                    case ObjectType.Radiologist:
                        radiologist = getReadingDoctor(wsProperty, hl7Value, radiologist);
                        shieldsAppObject = radiologist;
                        break;
                    default:
                        break;
                }
            });

            return (T)shieldsAppObject;
        }

        private static WSShieldsApps.Location getLocation(WebservicePropertySet wsProperty, string hl7Value, WSShieldsApps.Location referringLocation)
        {
                switch (wsProperty.name)
                {
                    case ShieldsExpressLinkDAO.PROPERTY_LOCATION_STREET_ONE:
                        referringLocation.StreetOne = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_LOCATION_STREET_TWO:
                        referringLocation.StreetTwo = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_LOCATION_CITY:
                        referringLocation.City = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_LOCATION_STATE:
                        referringLocation.State = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_LOCATION_ZIP:
                        referringLocation.ZipCode = hl7Value;
                        break;
                    default:
                        break;
                }

            return referringLocation;
        }

        private static WSShieldsApps.Patient getPatient(WebservicePropertySet wsProperty, string hl7Value,WSShieldsApps.Patient patient)
        {
                switch (wsProperty.name)
                {
                    case ShieldsExpressLinkDAO.PROPERTY_PATIENT_FIRST_NAME:
                        patient.FirstName = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_PATIENT_LAST_NAME:
                        patient.LastName = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_PATIENT_MIDDLE_NAME:
                        patient.MiddleName = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_PATIENT_MEDICAL_RECORD_NO:
                        patient.MedicalRecordNo = hl7Value;
                        break;
                    case ShieldsExpressLinkDAO.PROPERTY_PATIENT_DATE_OF_BIRTH:
                        patient.DateOfBirth = hl7DateToRealDate(hl7Value);
                        break;
                    default:
                        break;
                }

                return patient;
        }

        private static WSShieldsApps.Organization getOrganization(WebservicePropertySet wsProperty, string hl7Value, WSShieldsApps.Organization organization)
        {
            switch (wsProperty.name)
            {
                case ShieldsExpressLinkDAO.PROPERTY_ORGANIZATION_NAME:
                    organization.OrganizationName = hl7Value;
                    break;        
                default:
                    break;
            }

            return organization;
        }

        private static WSShieldsApps.Exam getExam(WebservicePropertySet wsProperty, string hl7Value, WSShieldsApps.Exam exam)
        {
            switch (wsProperty.name)
            {
                case ShieldsExpressLinkDAO.PROPERTY_EXAM_ACCESSION_NO:
                    exam.AccessionNo = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_EXAM_CODE_NAME:
                    exam.ExamCodeName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_EXAM_TYPE_NAME:
                    exam.ExamTypeName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_EXAM_SIDE_NAME:
                    exam.ExamSideName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_EXAM_STATUS_NAME:
                    exam.ExamStatusName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_EXAM_DATE:
                    exam.ExamDate = hl7DateToRealDateTime(hl7Value);
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_EXAM_DATE_REQUESTED:
                    exam.DateRequested = hl7DateToRealDateTime(hl7Value);
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_SOURCE_OF_REFERRAL:
                    exam.SourceOfReferralName = hl7Value;
                    break;
                default:
                    break;
            }

            return exam;
        }

        private static WSShieldsApps.Report getReport(WebservicePropertySet wsProperty, HL7Message message, string hl7Value, WSShieldsApps.Report report)
        {
            switch (wsProperty.name)
            {
                case ShieldsExpressLinkDAO.PROPERTY_REPORT_OBSERVATION_DATE:
                    report.ObservationDate = hl7DateToRealDateTime(hl7Value);
                    break;
                default:
                    break;
            }
            return report;
        }

        private static WSShieldsApps.Doctor getReferringDoctor(WebservicePropertySet wsProperty, string hl7Value, WSShieldsApps.Doctor referring)
        {
            switch (wsProperty.name)
            {
                case ShieldsExpressLinkDAO.PROPERTY_REFERRING_FIRST_NAME:
                    referring.FirstName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_REFERRING_LAST_NAME:
                    referring.LastName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_REFERRING_MIDDLE_NAME:
                    referring.MiddleName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_REFERRING_NPI:
                    referring.Npi = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_REFERRING_SUFFIX:
                    referring.Suffix = hl7Value;
                    break;
                default:
                    break;
            }

            return referring;
        }

        private static WSShieldsApps.Doctor getReadingDoctor(WebservicePropertySet wsProperty, string hl7Value, WSShieldsApps.Doctor radiologist)
        {
            switch (wsProperty.name)
            {
                case ShieldsExpressLinkDAO.PROPERTY_RADIOLOGIST_FIRST_NAME:
                    radiologist.FirstName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_RADIOLOGIST_LAST_NAME:
                    radiologist.LastName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_RADIOLOGIST_MIDDLE_NAME:
                    radiologist.MiddleName = hl7Value;
                    break;
                case ShieldsExpressLinkDAO.PROPERTY_RADIOLOGIST_NPI:
                    radiologist.Npi = hl7Value;
                    break;
                default:
                    break;
            }

            return radiologist;
        }

        public static bool isReportAddendum(HL7Message hl7Message)
        {
            List<Segment> segments = hl7Message.segments.FindAll(s => s.fields[ZERO_ELEMENT].value == SEGMENT_TYPE_OBX);
            string obxData = BLANK;
            bool isAddendum = false;
            try
            {
                segments.ForEach(delegate(Segment segment)
                {
                    if (isAddendum)
                    {
                        // just leave the for - loop
                        return;
                    }

                    obxData = segment.fields[REPORT_TEXT_ELEMENT].value;
                    if (Regex.IsMatch(obxData, HAS_ADDENDUM_PATTERN))
                    {
                        isAddendum = true;

                        // exit foreach
                        return;                       
                    }                    
                });
            }
            catch (Exception ex)
            {
                // log error
                ErrorLogger.LogError(ex, "isReportAddendum(String obxData)", obxData);
            }

            return isAddendum;
        }

        public static int getNumberOfReportEntries(HL7Message hl7Message)
        {
            int reportEntries = ZERO;
            List<Segment> obxSegments = new List<Segment>();

            // get all obx segments
            obxSegments = hl7Message.segments.FindAll(s => s.fields[ZERO_ELEMENT].value == SEGMENT_TYPE_OBX);

            // get max number in iteration field
            reportEntries = obxSegments.Max(x => Convert.ToInt32(x.fields[FIRST_ELEMENT].value));

            return reportEntries;
        }

        public static string getAddendumReportText(HL7Message hl7Message, WSShieldsApps.Report report)
        {
            bool readAddendumText = false;
            string html = BLANK;
            string obxData = BLANK;
            List<Segment> segments = new List<Segment>();
            List<String> reportText = new List<String>();
            // only return obx segment types
            segments = hl7Message.segments.FindAll(s => s.fields[ZERO_ELEMENT].value == SEGMENT_TYPE_OBX);

            // for each of those obx segments, process report
            segments.ForEach(delegate(Segment segment)
            {
                try
                {
                    obxData = segment.fields[REPORT_TEXT_ELEMENT].value;

                    if (Regex.IsMatch(obxData, ADDENDUM_PATTERN_MATCH_BEGIN))
                    {
                        // override the addendum report obx date
                        var addendumObservationDate = segment.fields[REPORT_OBX_DTTM].components[FIRST_ELEMENT].value;
                        // set the obx date 
                        report.ObservationDate = hl7DateToRealDateTime(addendumObservationDate);
                        // update the addendum text flag to true
                        readAddendumText = true;

                    }
                    else if (Regex.IsMatch(obxData, ADDENDUM_PATTERN_MATCH_END))
                    {
                        readAddendumText = false;

                        // still add the end text
                        reportText.Add(obxData); 
                    }

                    // if we read, we add to the list of text
                    if (readAddendumText)
                    {
                        reportText.Add(obxData);                    
                    }                   
                }
                catch (Exception ex)
                {
                    // get the message control id to notify user of error
                    string messageControlId = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                        SegmentType.MSH,
                                                                        MESSAGE_HEADER_CONTROL_ID);
                    // make a note about it
                    obxData = "Report Segment Cutoff, please resend report, message control id:" + messageControlId;

                    // log error
                    ErrorLogger.LogError(ex, "getAddendumReportText(HL7Message hl7Message)", obxData);

                    // add report text
                    reportText.Add(obxData);
                }
            });

            html += REPORT_HTML_HEADER;
            html += String.Join(HTML_LINE_BREAK, reportText.ToArray());
            html += REPORT_HTML_FOOTER;

            return html;
        }

        public static string getAddendumReportText(HL7Message hl7Message, WSShieldsApps.Report report, int addendumId)
        {
            bool readAddendumText = false;
            string html = BLANK;
            string obxData = BLANK;
            int addendumPosition = ZERO;
            List<Segment> segments = new List<Segment>();
            List<String> reportText = new List<String>();
            // only return obx segment types
            segments = hl7Message.segments.FindAll(s => s.fields[ZERO_ELEMENT].value == SEGMENT_TYPE_OBX);

            // for each of those obx segments, process report
            segments.ForEach(delegate(Segment segment)
            {
                try
                {
                    obxData = segment.fields[REPORT_TEXT_ELEMENT].value;
                    addendumPosition = Convert.ToInt32(segment.fields[REPORT_ITERATION_ELEMENT].value);

                    if (Regex.IsMatch(obxData, ADDENDUM_PATTERN_MATCH_BEGIN))
                    {
                        if (addendumPosition == addendumId)
                        { 
                            // override the addendum report obx date
                            var addendumObservationDate = segment.fields[REPORT_OBX_DTTM].components[FIRST_ELEMENT].value;
                            // set the obx date 
                            report.ObservationDate = hl7DateToRealDateTime(addendumObservationDate);
                            // update the addendum text flag to true
                            readAddendumText = true;
                        }

                    }
                    else if (Regex.IsMatch(obxData, ADDENDUM_PATTERN_MATCH_END))
                    {
                        readAddendumText = false;

                        // still add the end text
                        reportText.Add(obxData);
                    }

                    // if we read, we add to the list of text
                    if (readAddendumText)
                    {
                        reportText.Add(obxData);
                    }
                }
                catch (Exception ex)
                {
                    // get the message control id to notify user of error
                    string messageControlId = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                        SegmentType.MSH,
                                                                        MESSAGE_HEADER_CONTROL_ID);
                    // make a note about it
                    obxData = "Report Segment Cutoff, please resend report, message control id:" + messageControlId;

                    // log error
                    ErrorLogger.LogError(ex, "getAddendumReportText(HL7Message hl7Message)", obxData);

                    // add report text
                    reportText.Add(obxData);
                }
            });

            html += REPORT_HTML_HEADER;
            html += String.Join(HTML_LINE_BREAK, reportText.ToArray());
            html += REPORT_HTML_FOOTER;

            return html;
        }

        private static string getReportText(HL7Message hl7Message)
        {
            string html = BLANK;
            string obxData = BLANK;
            List<Segment> segments = new List<Segment>();
            List<String> reportText = new List<String>();
            // only return obx segment types
            segments = hl7Message.segments.FindAll(s => s.fields[ZERO_ELEMENT].value == SEGMENT_TYPE_OBX);
            
            // for each of those obx segments, process report
            segments.ForEach(delegate(Segment segment) 
            {
                // get the segment type
                string segmentType = segment.fields[ZERO_ELEMENT].value;

                try
                {
                    obxData = segment.fields[REPORT_TEXT_ELEMENT].value;
                    reportText.Add(obxData);
                }
                catch (Exception ex)
                {                        
                    // get the message control id to notify user of error
                    string messageControlId = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                        SegmentType.MSH,
                                                                        MESSAGE_HEADER_CONTROL_ID);
                    // make a note about it
                    obxData = "Report Segment Cutoff, please resend report, message control id:" + messageControlId;

                    // log error
                    ErrorLogger.LogError(ex, "getReportText(HL7Message hl7Message)", obxData);

                    // add report text
                    reportText.Add(obxData);
                }      
            });
     
            html += REPORT_HTML_HEADER;
            html += String.Join(HTML_LINE_BREAK, reportText.ToArray());
            html += REPORT_HTML_FOOTER;

            return html;

        }

        private static string getReportText(HL7Message hl7Message, bool noAddendum)
        {
            // the random chance someone overrides then puts false instead of true
            if (!noAddendum)
            {
                // do static function w/o override
                return getReportText(hl7Message);
            }

            // signal the end of capturing text
            bool endOfReport = false;

            string html = BLANK;
            string obxData = BLANK;
            string obxReportType = BLANK;
            string obxIteration = BLANK;

            List<Segment> segments = new List<Segment>();
            List<String> reportText = new List<String>();
            // only return obx segment types
            segments = hl7Message.segments.FindAll(s => s.fields[ZERO_ELEMENT].value == SEGMENT_TYPE_OBX);

            // for each of those obx segments, process report
            segments.ForEach(delegate(Segment segment)
            {
                // get the segment type
                string segmentType = segment.fields[ZERO_ELEMENT].value;

                try
                {
                    obxData = segment.fields[REPORT_TEXT_ELEMENT].value;
                    obxReportType = segment.fields[REPORT_TYPE_ELEMENT].value;
                    obxIteration = segment.fields[REPORT_ITERATION_ELEMENT].value;

                    // if we find addendum pattern, we then stop 
                    if (obxReportType == ADDENDUM && obxIteration != ORIGINAL_REPORT_ITERATION)
                    {
                        endOfReport = true;
                    }

                    if (!endOfReport)
                    {
                        reportText.Add(obxData);
                    }                 
                }
                catch (Exception ex)
                {
                    // get the message control id to notify user of error
                    string messageControlId = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                        SegmentType.MSH,
                                                                        MESSAGE_HEADER_CONTROL_ID);
                    // make a note about it
                    obxData = "Report Segment Cutoff, please resend report, message control id:" + messageControlId;

                    // log error
                    ErrorLogger.LogError(ex, "getReportText(HL7Message hl7Message)", obxData);

                    // add report text
                    reportText.Add(obxData);
                }
            });

            html += REPORT_HTML_HEADER;
            html += String.Join(HTML_LINE_BREAK, reportText.ToArray());
            html += REPORT_HTML_FOOTER;

            return html;

        }

        private static string getHL7ValueSEL(HL7Message message,
                                            List<MessageGroupInstance> messageGroupInstances,
                                            List<MessageGroup> messageGroups, 
                                            WebservicePropertySet wsProperty
                                            )
        {
            // get the ms groups
            List<MessageGroup> msGroups
                = ConfigurationUtility.GetIncomingWebserviceMessageGroup(wsProperty, messageGroups);

            // get segment string type
            MessageGroupInstance messageGroupInstance
                = messageGroupInstances.Find(i => i.id == msGroups[ZERO].messageGroupInstanceId);

            // get the message position for this specific column
            List<Configuration> configMessagePoisition
                = ConfigurationDAO.GetMessagePosition(messageGroupInstance);

            // get the coordinates for the message item
            string messageCoordinates = HL7MessageUtility.getItemCoordinates(configMessagePoisition);

            // get segment string type
            string segmentType = configMessagePoisition[ZERO_ELEMENT].segmentType.name;

            // get the value for the database inside of the value position
            string hl7Value = HL7MessageUtility.getValueByPosition(
                                                message,
                                                HL7MessageUtility.getSegmentType(segmentType),
                                                messageCoordinates);

            return hl7Value;
               
        }


        private static DateTime hl7DateToRealDate(string hl7Value)
        {
            // make new date now to know if the value was blank
            DateTime newDate = DateTime.Now;

            // if it's blank make fake dob
            if (hl7Value == BLANK || hl7Value == null)
            {
                newDate = DateTime.ParseExact("01/01/1900", "mm/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                string year  = hl7Value.Substring(0, 4);
                string month = hl7Value.Substring(4, 2);
                string day   = hl7Value.Substring(6, 2);

                string tempDate = string.Concat(month, "/", day, "/", year, " 00:00:00 AM");
                // make new date
                newDate = Convert.ToDateTime(tempDate);
            }

            return newDate;
        }

        private static DateTime hl7DateToRealDateTime(string hl7Value)
        {
            // make new date now to know if the value was blank
            DateTime newDate = DateTime.Now;

            try
            {
                // replace unknown "^" character
                if (hl7Value.Contains('^'))
                {
                    hl7Value = hl7Value.Replace("^", "");
                }


                // if it's blank make fake dob
                if (hl7Value == BLANK || hl7Value == null)
                {
                    newDate = DateTime.ParseExact("01/01/1900", "mm/dd/yyyy", System.Globalization.CultureInfo.CurrentCulture);
                }
                else
                {
                    // split the object up
                    string year     = hl7Value.Substring(0, 4);
                    string month    = hl7Value.Substring(4, 2);
                    string day      = hl7Value.Substring(6, 2);
                    string hour     = hl7Value.Substring(8, 2);
                    string minute   = hl7Value.Substring(10, 2);

                    // convert meridian
                    string meridian = (Convert.ToInt32(hour) > 12) ? "PM" : "AM";
                    // get the new dttm

                    string tempDate = string.Concat(month, "/", day, "/", year," ",
                                                    hour,":",minute," ",meridian);
                    // make new date
                    newDate = Convert.ToDateTime(tempDate);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return newDate;

        }

        public static bool handleCheckApplicationRejected(string serviceMessage)
        {
            bool isRejected = false;

            List<string> applicationConditions = new List<string>()
            {                           
                WSShieldsApps.ENUMMESSAGE.NO_ENTERPRISE_ID.ToString(),
                WSShieldsApps.ENUMMESSAGE.NO_EXAM_ID.ToString(),
                WSShieldsApps.ENUMMESSAGE.NO_INTERNAL_MRN.ToString(),
                WSShieldsApps.ENUMMESSAGE.NO_NPI.ToString(),
                WSShieldsApps.ENUMMESSAGE.UNKNOWN.ToString(),
                WSShieldsApps.ENUMMESSAGE.OBJECT_NULL.ToString(),
                WSShieldsApps.ENUMMESSAGE.FAILURE.ToString(),
                WSShieldsApps.ENUMMESSAGE.ORGANIZATION_INSERT_FAILED.ToString(),
                WSShieldsApps.ENUMMESSAGE.PATIENT_INSERT_FAILED.ToString()
            };

            foreach (String messageType in applicationConditions)
            {
                if (serviceMessage.Contains(messageType))
                {
                    isRejected = true;
                    break;
                }
            }

            return isRejected;
        }
    }
}
