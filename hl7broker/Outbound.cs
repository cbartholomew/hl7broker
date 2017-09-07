using HL7Broker.Model;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using HL7Broker.Utility;
using HL7Broker.DAO;
using HL7Broker.Model.HL7;

namespace HL7Broker
{
    partial class Outbound : ServiceBase
    {
        public const string EVENT_LOG_SOURCE = "HL7Broker";
        public const string EVENT_LOG = "HL7Broker";
        public const string ON_START_MSG = "HL7Broker has now started.";
        public const string ON_STOP_MSG = "HL7Broker is now stopped.";
        public const string ON_ERR = "HL7Broker has encountered an unknown error.";

        public Dictionary<int, Socket> socketDictionary { get; set; }

        public Outbound()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // log
            ErrorLogger.Log(ErrorLogger.LogType.OUTBOUND_SERVICE_STARTING, DateTime.Now.ToString());

            // get all configuration data
            List<Configuration> configs = ConfigurationDAO.GetApplications();

            // get the master configuration
            Configuration masterConfig = ConfigurationDAO.GetAllConfigurations();

            // gets the outgoing webservice configuration only
            List<Configuration> applications = ConfigurationUtility.GetIncomingWebservice(configs);
     
            applications.ForEach(delegate(Configuration appConfig) {
                OutboundHandler outboundHandler = new OutboundHandler();
                string appName = appConfig.application.name;

                // set the app name
                outboundHandler.setApplicationName(appName);

                // set the master config
                outboundHandler.setMasterConfiguration(masterConfig);

                // set the static worker
                outboundHandler.setProcessingWorker(OutboundHandlerDAO.handleProcessingForOutboundHandler);
                
                // add to queue and run
                ThreadPool.QueueUserWorkItem(new WaitCallback(startOutboundProcessing), outboundHandler);                        
            });
        }

        private void startOutboundProcessing(object state)
        {
            OutboundHandler outboundHandler = (OutboundHandler)state;
            try
            {             
                outboundHandler.Begin();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "startOutboundProcessing(object state)", outboundHandler.getApplicationName());
            }
        }

        protected override void OnStop()
        {
            // log
            ErrorLogger.Log(ErrorLogger.LogType.OUTBOUND_SERVICE_STOPPING, DateTime.Now.ToString());
        }

        public void onDebugStart()
        {
            OnStart(null);
        }

        public void onDebugStartWithMessage()        
        {
            string path = @"C:\Users\chrisb\Documents\ApplicationDevelopment\Working\HL7Broker.root\HL7Broker\HL7Broker\Messages\ORMSC_LOCATION.hl7";
            string input = HL7Message.ReadHL7FromFile(path);
            // attempt to scrub message
            string hl7Input = HL7MessageUtility.scrubHL7MessageForParse(input);

            // Get Incoming HL7 Message Configuration
            HL7Message hl7Message = HL7MessageDAO.getMessage(hl7Input);

            string value = HL7MessageUtility.getValueByPosition(hl7Message, Generic.SegmentType.PV1, "11.2");
            
        }
    }
}
