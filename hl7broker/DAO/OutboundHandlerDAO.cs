using HL7Broker.Model;
using HL7Broker.Model.HL7;
using HL7Broker.Utility;
using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.App.Model;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HL7Broker.DAO
{
    public class OutboundHandlerDAO : Generic
    {
        public static void handleProcessingForOutboundHandler(Configuration masterConfig, OutboundHandler outboundHandler)
        {
            // to control some basic CPU handling settings
            Config outboundConfig = new Config();

            // load each of the configurations into their own objects
            List<Application> applications = masterConfig.applications;
            List<Communication> communications = masterConfig.communications;
            List<WebserviceObject> webserviceObjects = masterConfig.webserviceObjects;
            List<WebserviceInstance> webserviceInstances = masterConfig.webserviceInstances;
            List<WebservicePropertySet> webserviceProperties = masterConfig.webservicePropertySets;
            List<MessageGroup> messageGroups = masterConfig.messageGroups;
            List<MessageGroupInstance> messageGroupInstances = masterConfig.messageGroupInstances;

            // continue to read the table
            while (true)
            {
                applications.ForEach(delegate(Application app)
                {
                    if (app.name == outboundHandler.getApplicationName())
                    {
                        // get the unprocessed message count for the application
                        List<MessageBucket> brokerInformation
                            = MessageBucketDAO.GetUnprocessedMessageHeaderInstancesByApplication(app);

                        // set the MOD from config file.
                        var options = new ParallelOptions { 
                            MaxDegreeOfParallelism = outboundConfig.MaxDegreeOfParallelism 
                        };
                        // parallel For each 
                        Parallel.ForEach(brokerInformation, options, broker =>
                        {
                            // get the message header and message
                            MessageHeaderInstance messageHeaderInstance = broker.messageHeaderInstance;
                            Message message = broker.message;

                            // srub the input of bad stuff
                            string hl7Scrubbed = HL7MessageUtility.scrubHL7MessageForParse(message.hl7Raw);

                            // make the hl7 message
                            HL7Message hl7Message = HL7MessageDAO.getMessage(hl7Scrubbed);

                            // locally retrieve the communication object from memory
                            Communication communication
                                = ConfigurationUtility.GetIncomingWebserviceCommunication(app, communications);

                            // locally retrieve the webservice instance object from memory
                            WebserviceInstance webserviceInstance
                                = ConfigurationUtility.GetIncomingWebserviceInstance(communication, webserviceInstances);

                            // locally retrieve the web service objects from memory
                            List<WebserviceObject> wsObjects
                                = ConfigurationUtility.GetIncomingWebserviceObjects(webserviceInstance, webserviceObjects);

                            // determine the message type
                            Generic.MessageType messageType
                                = HL7MessageUtility.getMessageType(hl7Message, hl7Scrubbed);

                            switch (messageType)
                            {
                                // to handle a new message add message type then its own handler
                                case Generic.MessageType.ADT:
                                    break;
                                case Generic.MessageType.ORM:
                                    handleProcessingForORM(wsObjects,
                                                           webserviceProperties,
                                                           hl7Message,
                                                           messageGroupInstances,
                                                           messageGroups,
                                                           app,
                                                           message);
                                    break;
                                case Generic.MessageType.ORU:
                                    handleProcessingForORU(wsObjects,
                                                           webserviceProperties,
                                                           hl7Message,
                                                           messageGroupInstances,
                                                           messageGroups,
                                                           app,
                                                           message);
                                    break;
                                case Generic.MessageType.SIU:
                                    break;
                                case Generic.MessageType.UNKNOWN:
                                    break;
                                default:
                                    break;
                            }

                            // update the table to processed
                            MessageBucketDAO.UpdateProcessedFlagAndMessageLog(messageHeaderInstance, message, true);

                            // update broker stats
                            handleBrokerStatUpdate(message, communication);
                        });
                    }
                });

                // provide blocking if there is no data to process.
                Thread.Sleep(1000);
            }           
        }

        public static void handleBrokerStatUpdate(Message message, Communication communication)
        {
            Broker myBrokerUpdate = new Broker();
            myBrokerUpdate =
                ConfigurationUtility.GetBrokerByApplicationAndCommunication(communication, myBrokerUpdate);
            myBrokerUpdate.lastMessageId = message.id;
            myBrokerUpdate.lastMessageDTTM = DateTime.Now;
            BrokerDAO.UpdateAppBrokerProperty(myBrokerUpdate, BrokerDAO.Property.Stats);

            // update the inbound one because it won't for some reason work on the inbound service
            myBrokerUpdate.id = 2;
            BrokerDAO.UpdateAppBrokerProperty(myBrokerUpdate, BrokerDAO.Property.Stats);
        }

        public static void handleProcessingForORM(List<WebserviceObject> wsObjects, 
                                                  List<WebservicePropertySet> webserviceProperties,
                                                  HL7Message hl7Message,
                                                  List<MessageGroupInstance> messageGroupInstances,
                                                  List<MessageGroup> messageGroups,
                                                  Application app,
                                                  Message message)       
        {
            string serviceMessage = BLANK;

            // make SEL exam object
            WSShieldsApps.Exam exam 
                = new WSShieldsApps.Exam();
            WSShieldsApps.Location location 
                = new WSShieldsApps.Location();
            WSShieldsApps.Patient patient 
                = new WSShieldsApps.Patient();
            WSShieldsApps.Organization organization
                = new WSShieldsApps.Organization();
            WSShieldsApps.Doctor referring 
                = new WSShieldsApps.Doctor();

            // for each object - for each property set for that object - handle accordingly
            wsObjects.ForEach(delegate(WebserviceObject wsObject)
            {
                List<WebservicePropertySet> wsProperties
                    = ConfigurationUtility.GetIncomingWebservicePropertySets(wsObject, webserviceProperties);

                if (app.name == RSERVER)
                {
                    if (ShieldsExpressLinkDAO.LOCATION == wsObject.name)
                    {
                        location = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Location>(
                                                            ShieldsExpressLinkUtility.ObjectType.Location,
                                                            hl7Message, messageGroupInstances,
                                                            messageGroups,
                                                            wsProperties);


                    }
                    else if (ShieldsExpressLinkDAO.PATIENT == wsObject.name)
                    {
                        patient = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Patient>(
                                                                ShieldsExpressLinkUtility.ObjectType.Patient,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);

                    }
                    else if (ShieldsExpressLinkDAO.ORGANIZATION == wsObject.name)
                    {
                        organization = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Organization>(
                                                                ShieldsExpressLinkUtility.ObjectType.Organization,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);


                    }
                    else if (ShieldsExpressLinkDAO.EXAM == wsObject.name)
                    {
                        exam = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Exam>(
                                                                ShieldsExpressLinkUtility.ObjectType.Exam,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);
                    }
                    else if (ShieldsExpressLinkDAO.REFERRING == wsObject.name)
                    {
                        referring = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Doctor>(
                                                                ShieldsExpressLinkUtility.ObjectType.Referring,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);
                    }

                }
            });

            // exam object
            exam.Location = location;
            exam.Doctor = referring;
            exam.Organization = organization;
            exam.Patient = patient;

            // insert exam
            serviceMessage += ShieldsExpressLinkDAO.PutExam(exam);
            
            // message for the service
            handleAckForService(hl7Message, message, serviceMessage);

            // message log handler
            handleMessageLog(message);
        
        }

        public static void handleProcessingForORU(List<WebserviceObject> wsObjects,
                                                  List<WebservicePropertySet> webserviceProperties,
                                                  HL7Message hl7Message,
                                                  List<MessageGroupInstance> messageGroupInstances,
                                                  List<MessageGroup> messageGroups,
                                                  Application app,
                                                  Message message)
        {
            string serviceMessage = BLANK;

            // make SEL exam object
            WSShieldsApps.Exam exam
                = new WSShieldsApps.Exam();
            WSShieldsApps.Location location
                = new WSShieldsApps.Location();
            WSShieldsApps.Patient patient
                = new WSShieldsApps.Patient();
            WSShieldsApps.Organization organization
                = new WSShieldsApps.Organization();
            WSShieldsApps.Doctor referring
                = new WSShieldsApps.Doctor();
            // make SEL report object
            WSShieldsApps.Report report 
                = new WSShieldsApps.Report();
            WSShieldsApps.Doctor radiologist 
                = new WSShieldsApps.Doctor();

            // for each object - for each property set for that object - handle accordingly
            wsObjects.ForEach(delegate(WebserviceObject wsObject)
            {
                List<WebservicePropertySet> wsProperties
                    = ConfigurationUtility.GetIncomingWebservicePropertySets(wsObject, webserviceProperties);

                if (app.name == RSERVER)
                {
                    if (ShieldsExpressLinkDAO.LOCATION == wsObject.name)
                    {
                        location = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Location>(
                                                            ShieldsExpressLinkUtility.ObjectType.Location,
                                                            hl7Message, messageGroupInstances,
                                                            messageGroups,
                                                            wsProperties);


                    }
                    else if (ShieldsExpressLinkDAO.PATIENT == wsObject.name)
                    {
                        patient = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Patient>(
                                                                ShieldsExpressLinkUtility.ObjectType.Patient,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);

                    }
                    else if (ShieldsExpressLinkDAO.ORGANIZATION == wsObject.name)
                    {
                        organization = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Organization>(
                                                                ShieldsExpressLinkUtility.ObjectType.Organization,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);


                    }
                    else if (ShieldsExpressLinkDAO.EXAM == wsObject.name)
                    {
                        exam = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Exam>(
                                                                ShieldsExpressLinkUtility.ObjectType.Exam,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);
                        

                    }
                    else if (ShieldsExpressLinkDAO.REFERRING == wsObject.name)
                    {
                        referring = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Doctor>(
                                                                ShieldsExpressLinkUtility.ObjectType.Referring,
                                                                hl7Message, messageGroupInstances,
                                                                messageGroups,
                                                                wsProperties);
                    }
                    else if (ShieldsExpressLinkDAO.REPORT == wsObject.name)
                    {
                         report = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Report>(
                                                                        ShieldsExpressLinkUtility.ObjectType.Report,
                                                                        hl7Message, messageGroupInstances,
                                                                        messageGroups,
                                                                        wsProperties);                    
                    }
                    else if (ShieldsExpressLinkDAO.RADIOLOGIST == wsObject.name)
                    {
                         radiologist = ShieldsExpressLinkUtility.getShieldsAppObject<WSShieldsApps.Doctor>(
                                                                        ShieldsExpressLinkUtility.ObjectType.Radiologist,
                                                                        hl7Message, messageGroupInstances,
                                                                        messageGroups,
                                                                        wsProperties);
                         // set as radiologist 
                         radiologist.Type = RADIOLOGIST_TYPE;
                         radiologist.EnterpriseId = ENTERPRISE_IDENTITY;
                    }
                }                                   
            });
           
            // set exam object
            exam.Location       = location;
            exam.Doctor         = referring;
            exam.Organization   = organization;
            exam.Patient        = patient;

            // make a new exam object
            WSShieldsApps.Exam exam2Object = new WSShieldsApps.Exam();

            // pass the accession so we don't override
            exam2Object.AccessionNo = exam.AccessionNo;

            // get the updated exam id only
            exam.ExamId = ShieldsExpressLinkDAO.GetExamByAccessionNo(exam2Object).ExamId;
            
            // handle radiologist
            serviceMessage += ShieldsExpressLinkDAO.PutDoctor(radiologist);

            // handle exam
            serviceMessage += ShieldsExpressLinkDAO.PutExam(exam);

            // report needs exam id associated to it
            report.ExamId = exam.ExamId;

            // report needs radiologist id associated to it
            report.RadiologistId = radiologist.DoctorId;

            // handle orignal report 
            serviceMessage += ShieldsExpressLinkDAO.PostReport(report);

            // handle addendum text only
            if (ShieldsExpressLinkUtility.isReportAddendum(hl7Message))
            {
                // for each report text starting at "2" (1 == orignal report) get all addendums after
                for (int reportIteration = 2, n = ShieldsExpressLinkUtility.getNumberOfReportEntries(hl7Message); 
                    reportIteration <= n; 
                    reportIteration++)
                {
                    // set the addendum flag
                    report.IsAddendum = true;

                    // re-define report text and observation date
                    report.ReportText = ShieldsExpressLinkUtility.getAddendumReportText(hl7Message, report, reportIteration);

                    // handle inserting of addendum report 
                    serviceMessage += ShieldsExpressLinkDAO.PostReport(report, reportIteration);
                }
            }

            // message for the service
            handleAckForService(hl7Message, message, serviceMessage);

            // message log handler
            handleMessageLog(message);
        }

        public static void handleMessageLog(Message message)
        {
            // update the message log with the result
            MessageLog messageLog = new MessageLog()
            {
                messageId = message.id,
                messageLogTypeId = (int)MessageLogDAO.MessageLogType.WEB_SERVICE,
                createdDttm = DateTime.Now
            };

            // send to message log
            MessageLogDAO.insertIntoMessageLog(messageLog);
        }

        public static void handleAckForService(HL7Message hl7Message, Message message, string ackResponseMessage)
        {
            // get the message control id to build the ack
            string messageControlId = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                           SegmentType.MSH,
                                                                           MESSAGE_HEADER_CONTROL_ID);
            // get the message date stamp
            string messageDateStamp = HL7MessageUtility.getValueByPosition(hl7Message,
                                                                           SegmentType.MSH,
                                                                           MESSAGE_DATE_STAMP_POSITION);

            // get the correct ack response
            bool isRejected = ShieldsExpressLinkUtility.handleCheckApplicationRejected(ackResponseMessage);

            // if the ack message is rejected, apply act type 
            AcknowledgementDAO.AcknowledgementType ackMessageEnum 
                = (isRejected) ? AcknowledgementDAO.AcknowledgementType.AR:
                                 AcknowledgementDAO.AcknowledgementType.AA;

            // get ack response
            string strAckResponse = HL7MessageUtility.getAck(ackMessageEnum.ToString(),
                                                             messageControlId,
                                                             messageDateStamp,
                                                             MESSAGE_HEADER_APPLICATION_NAME,
                                                             MESSAGE_HEADER_FACILITY_NAME,
                                                             ackResponseMessage);

            // build ack response
            Acknowledgement ackResponse = new Acknowledgement()
            {
                acknowledgementTypeId = (int)ackMessageEnum,
                messageId = message.id,
                raw = strAckResponse,
                createdDttm = DateTime.Now
            };

            // insert into ack
            AcknowledgementDAO.insertIntoAcknowledgement(ackResponse);
        }

    }
}
