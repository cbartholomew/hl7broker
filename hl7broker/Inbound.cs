using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using HL7Broker.Model;
using HL7Broker.DAO;
using HL7Broker.Utility;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Utility;
using System.Threading;

namespace HL7Broker
{
    partial class Inbound : ServiceBase
    {
        public Dictionary<int, Socket> socketDictionary { get; set; }

        public const string EVENT_LOG_SOURCE = "HL7Broker";
        public const string EVENT_LOG = "HL7Broker";
        public const string ON_START_MSG = "HL7Broker has now started.";
        public const string ON_STOP_MSG = "HL7Broker is now stopped.";
        public const string ON_ERR = "HL7Broker has encountered an unknown error.";

        public Inbound()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // log
            ErrorLogger.Log(ErrorLogger.LogType.INBOUND_SERVICE_STARTING, DateTime.Now.ToString());

            // init thread dictionary
            this.socketDictionary = new Dictionary<int, Socket>();
 
            // get all configuration data
            List<Configuration> configs = ConfigurationDAO.GetApplications();

            // get return all incoming interface configurations
            List<Configuration> intefaceConfigs
                = ConfigurationUtility.GetIncomingInterfaces(configs);

            // start the interfaces by the database config
            intefaceConfigs.ForEach(delegate(Configuration appConfig)
            {
                // get the interfaces for this communication set
                List<Configuration> interfaces
                    = ConfigurationDAO.GetInterfaceConfigration(appConfig.communication);

                // for each of the inbound interface threads, start them
                interfaces.ForEach(delegate(Configuration intConfig)
                {
                    // get the outgoing database communication id
                    Configuration brokerDBConfig
                       = ConfigurationUtility.GetOutgoingDatabasesByApplicationId(configs, appConfig.application.id);

                    // pass the db communication to the class's communication list since we 
                    intConfig.communications = new List<Communication>();
                    intConfig.communications.Add(brokerDBConfig.communication);
                    
                    // add to queue
                    ThreadPool.QueueUserWorkItem(new WaitCallback(startInterface), intConfig);
                });

            });        
        }

        private void startInterface(object state)
        {
            try
            {
                // create new configuration to get all configurations
                Configuration masterConfiguration = new Configuration();

                // get all configurations
                masterConfiguration = ConfigurationDAO.GetAllConfigurations();

                // pass configuration by reference, re-allocated the boject
                Configuration intConfig = (Configuration)state;

                // get the interface communication
                Communication incomingCommunication = intConfig.communication;

                // get the database communication from the list
                Communication outgoingCommunication = intConfig.communications[0];

                // create new socket
                Socket socket = new Socket(intConfig.interfaceThread.ipAddress,
                                           intConfig.interfaceThread.port,
                                           intConfig.interfaceThread.maxConnections,
                                           incomingCommunication,
                                           outgoingCommunication);


                // set the master configuration
                socket.masterConfiguration = masterConfiguration;

                // set the incoming handler, [Coming to the Service]
                socket.setIncomingMessageHandler(SocketDAO.handleIncomingHL7MessageToBroker);

                // set the outgoing handler, [Leaving Service]
                socket.setOutgoingMessageHandler(SocketDAO.handleOutgoingHL7MessageToBroker);

                // add the socket to the dictionary so we can access it from OnStop()
                this.socketDictionary.Add(intConfig.communication.id, socket);

                // begin listening on the socket
                socket.Start();
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "OnStart()", "See windows event log for details");
            }
           
        }

        protected override void OnStop()
        {
            // log
            ErrorLogger.Log(ErrorLogger.LogType.INBOUND_SERVICE_STOPPING, DateTime.Now.ToString());

            // stop all threads that are in the dictionary
            this.socketDictionary.ToList().ForEach(
                delegate(KeyValuePair<int, Socket> interfaceItem)
            {
                // init new thread
                Socket socket = interfaceItem.Value;
                
                // stop the socket
                socket.Stop();          
            });
        }

        public void onDebugStart()
        {
            OnStart(null);        
        }
    }
}
