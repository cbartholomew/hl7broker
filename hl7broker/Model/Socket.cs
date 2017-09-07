using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;
using HL7Broker.Model.HL7;
using HL7Broker.Utility;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.Sys.Model;
using System.Threading;
using System.Collections;
using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.App.Model;
namespace HL7Broker.Model
{
    public class Socket : Generic
    {
        // constants       
        private const string ARGUMENT_NULL_EXCEPTION = "No Delegate Set - please use setMessageHandler before calling Socket.Start()";

        // accept no larger than a 10 meg message
        private const int MAX_MESSAGE_SIZE = 10485760;

        private IPAddress   ipAddress;
        private int         portNo;
        private int maxConnections;
        private TcpListener tcpListener;        
        private MessageHandler incomingHandler;
        private MessageHandler outgoingdHandler;
        private Communication incomingCommunication;
        private Communication outgoingCommunication;        
        private HL7Message hl7Message;
        private NetworkStream networkStream;
        public  Configuration masterConfiguration { get; set; }
        public Broker broker { get; set; }
        public bool isStarted { get; set; }
        public bool alreadyConnected { get; set; }
        public bool alreadyListening { get; set; }
        // create a delegate to handle the incoming stream
        public delegate void MessageHandler(string HL7Input, Communication communication, Socket socket);

        public Socket(string IpAddress, 
                      string PortNo,
                      int MaxConnections,
                      Communication incomingCommunication, 
                      Communication outgoingCommunication)
        {
            // check if it's local debug mode
            Config localConfig = new Config();

            // if debug mode is true use local interface socket
            if (localConfig.IsDebugMode)
            {
                IpAddress = localConfig.IP_ADDRESS;
                PortNo = localConfig.PORT;            
            }

            // apply ip address
            this.ipAddress = IPAddress.Parse(IpAddress);                        
            
            // apply the port
            this.portNo = Convert.ToInt32(PortNo);

            // apply the connections
            this.maxConnections = MaxConnections;
            
            // set the communication classes
            this.incomingCommunication = incomingCommunication;
            this.outgoingCommunication = outgoingCommunication;
            
            // call set broker to get/set the broker row for this interface
            setBroker();
        }

        private void setBroker()
        {
            // init new broker to use for future updates
            this.broker = new Broker();

            // set the correct broker for this interface for use across class
            List<Broker> brokers = BrokerDAO.Get();

            // get a temp broker object
            Broker tempBroker = brokers.Find(b => b.communicationId == this.incomingCommunication.id);

            // do a shallow copy to get a new copy that I can work w/ later
            this.broker = tempBroker.ShallowCopy();

            // get the current process id of the thread
            this.broker.processId = GenericUtility.GetCurrentManagedThreadId();

            BrokerDAO.UpdateAppBrokerProperty(this.broker, BrokerDAO.Property.Process);

            BrokerDAO.UpdateAppBrokerProperty(this.broker, BrokerDAO.Property.Stats);
        }

        private void setNetworkStream(NetworkStream stream)
        {
            networkStream = stream;        
        }

        public NetworkStream getNetworkStream()
        {
            return networkStream;
        }

        public void setIncomingMessageHandler(MessageHandler messageHandler)
        {
            this.incomingHandler = messageHandler;
        }

        public void setOutgoingMessageHandler(MessageHandler messageHandler)
        {
            this.outgoingdHandler = messageHandler;
        }

        public void setHL7Message(HL7Message hl7Message)
        {
            this.hl7Message = hl7Message;
        }

        public HL7Message getHL7Message()
        {
            return hl7Message;
        }

        public void Start()
        {
            // based on the communication app id - get a list of applications that match this comm id
            List<Configuration> configurations = 
                ConfigurationDAO.GetApplications(new Application() { 
                    id = this.incomingCommunication.applicationId });

            // now get the app settings that are associated to this comm and app id
            ApplicationSetting appSetting = 
                configurations.Find(c => c.communication.id == this.incomingCommunication.id)
                .applicationSetting;

            // changing this item in the db means you need to bounce the service
            if (appSetting.disabled)
            { 
                return;
            }

            // changing this item in the db means you need to bounce the service
            if (appSetting.autoStart)
            { 
                startListener();
            }

            // anything in the while loop doesn't need to be bounced by the service
            while (true)
            {
                // set the broker each loop
                setBroker();

                // check if it's running i.e. waiting/connected
                this.isStarted =
                    (this.broker.interfaceStatusId == BrokerDAO.WAITING ||
                     this.broker.interfaceStatusId == BrokerDAO.CONNECTED);

                if (this.isStarted)
                {
                    // if the system is already listening (from autoStart Flag) 
                    // begin accepting messages
                    if (this.alreadyListening)
                    {
                        // begin feeding the messages in
                        run();
                    }
                    else
                    {
                        // otherwise, start the listener (i.e. waiting --> connected)
                        startListener();
                    }
                }
                else
                {
                    // if the system was already listening, and it's in "stopped"
                    // then this means that someone shutdown the socket - so stop the listener
                    if (this.alreadyListening)
                    {
                        stopListener();
                    }
                }
                // provide blocking on the thread if nothing is going on
                Thread.Sleep(1000);
            }       
        }

        public void Stop()
        {
            stopListener();
        }

        public void Restart()
        {
            // stop the client
            this.Stop();
            // start the client
            this.Start();
        }

        private void run()
        {
            // create bye buffer the size of the input
            Byte[] buffer = new Byte[MAX_MESSAGE_SIZE];

            // set connected here
            TcpClient tcpClient = this.tcpListener.AcceptTcpClient();

            if (tcpClient.Connected)
            {
                if (this.broker.interfaceStatusId != BrokerDAO.CONNECTED)
                {
                    // update interface broker status to connected since we are connected
                    this.broker.interfaceStatusId = BrokerDAO.CONNECTED;

                    // set worklist to waiting
                    BrokerDAO.UpdateAppBrokerProperty(this.broker, BrokerDAO.Property.Status);
                }
            }
            else
            {
                startWaiting();
            }

            // get the data
            String data = null;

            // get the network stream
            this.networkStream = tcpClient.GetStream();

            // define stream byte index                
            int byteIndex = 0;
            try
            {
                // define stream byte index                
                // Loop to receive all the data sent by the client. 
                while ((byteIndex = this.networkStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.UTF8.GetString(buffer, 0, byteIndex);

                    // if for some reason we can't process the message, we'll reject it (hopefully)
                    try
                    {
                        // pass it over to inbound delegate delegate
                        this.incomingHandler(data.ToString(), this.incomingCommunication, this);
                    }
                    catch (Exception e)
                    {
                        // this means we had trouble parsing the message when it came in. 
                        string ackRejected = HL7MessageUtility.getAck(AcknowledgementDAO.AcknowledgementType.AR.ToString(),
                            ERROR_CONTROL_ID,
                            DateTime.Now.ToString(),
                            MESSAGE_HEADER_APPLICATION_NAME,
                            MESSAGE_HEADER_FACILITY_NAME,
                            ERROR_MESSAGE_REJECTED);

                        // build ack response
                        AcknowledgementDAO.insertIntoAcknowledgement(new Acknowledgement()
                        {
                            id = NEG_ONE,
                            acknowledgementTypeId = (int)AcknowledgementDAO.AcknowledgementType.AE,
                            messageId = 0,
                            raw = ERROR_MESSAGE_REJECTED,
                            createdDttm = DateTime.Now
                        });

                        // pad the hl7 msesage for transfer
                        ackRejected = HL7MessageUtility.padHL7MessageForTransfer(ackRejected);

                        // write back
                        this.networkStream.Write(Encoding.UTF8.GetBytes(ackRejected), 0, ackRejected.Length);

                        // move on, don't stop
                        continue;
                    }

                    // pass it over to the outbound delegate
                    this.outgoingdHandler(data.ToString(), this.outgoingCommunication, this);

                }
            }
            catch (Exception ex)
            {
                ErrorLogger.LogError(ex, "run()");
            }
            finally 
            {
                // Shutdown and end connection
                tcpClient.Close();
                // if the user is disconnected - set to waiting
                startWaiting();
            }
            // Shutdown and end connection
            tcpClient.Close();
        }

        private void startWaiting()
        {
            // update broker status to connected
            this.broker.interfaceStatusId = BrokerDAO.WAITING;

            // set worklist to waiting
            BrokerDAO.UpdateAppBrokerProperty(this.broker, BrokerDAO.Property.Status);
        }

        private void startListener()
        {
            // start the interface
            this.tcpListener = new TcpListener(this.ipAddress, this.portNo);

            // start the interface
            this.tcpListener.Start();

            // set the listening to true
            this.alreadyListening = true;

            // update broker status to connected
            this.broker.interfaceStatusId = BrokerDAO.WAITING;

            // set worklist to waiting
            BrokerDAO.UpdateAppBrokerProperty(this.broker, BrokerDAO.Property.Status);
        }

        private void stopListener()
        {
            // end the thread
            this.tcpListener.Stop();

            // turn off the listener
            this.alreadyListening = false;

            // update broker status to connected
            this.broker.interfaceStatusId = BrokerDAO.STOPPED;

            // set worklist to waiting
            BrokerDAO.UpdateAppBrokerProperty(this.broker, BrokerDAO.Property.Status);
        }
    }
}
