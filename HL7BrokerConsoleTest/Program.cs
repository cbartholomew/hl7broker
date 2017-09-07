using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HL7BrokerSuite.Sys.Model;
using HL7BrokerSuite.Sys.DAO;
using HL7BrokerSuite.App.DAO;
using HL7BrokerSuite.App.Model;
using System.IO;
using System.Net;
using System.Net.Sockets;
using HL7BrokerSuite.Utility;


namespace HL7BrokerConsoleTest
{
    class Program
    {

              // Incoming data from the client.
    public static string data = null;

        static void Main(string[] args)
        {
            //TestService();
            makeNewMessageGroups();
            //TestSocket();
            //TestSocketListener();
            //newWebServiceInstanceAndTables();
            //TestBrokerUpdates();
            //TestConfigLoad();
            //TestOBX();
            //TestCopyApplicationConfigDatabase();
            //TestUpdate();
        }
        public static void TestUpdate()
        {
            BrokerDAO.UpdateAppBrokerProperty(new Broker() { id = 2, interfaceStatusId = 3 },BrokerDAO.Property.Status);
        }

        public static void TestCopyApplicationConfigDatabase()
        {
            string directionType = "OUTGOING";
            string communicationType = "DATABASE";
            int toApplicationIdentity = 24;
            int fromApplicationIdentity = 1;

            // get all applications
            List<Configuration> applicationList = ConfigurationDAO.GetApplications(new Application() { id = fromApplicationIdentity });

            // get all communuication and direction Types
            Configuration masterConfigration = ConfigurationDAO.GetAllConfigurations();
            
            // get the specific application direction and comm you want
            Configuration fromApplication  = applicationList.Find(a => a.communicationType.name == communicationType 
                                                                        && a.directionType.name == directionType);

            
            // communication translation identities
            int toDirectionTypeId = 
                masterConfigration.directionTypes.Find(d => d.name == fromApplication.directionType.name).id;
            int toCommunicationTypeId =
                masterConfigration.communicationTypes.Find(c => c.name == fromApplication.communicationType.name).id;

           // create a new application object with your current application identity
            Application toApplication = new Application()
            {
                id = toApplicationIdentity
            };

            // insert a new communication with your exsisting application 
            Communication toCommunication = 
                CommunicationDAO.PostUpdate(new Communication() { applicationId = toApplication.id,
                                                                  communicationTypeId = toCommunicationTypeId,
                                                                  directionTypeId = toDirectionTypeId
                                                                });


            // get the database_instance information (credential id, name, server, ip) of the communication identity.
            DatabaseInstance fromDatabaseInstance =
                masterConfigration.databaseInstances.Find(i => i.communicationId == fromApplication.communication.id);

            // get database_instance id of the copy from insert into new database_instance with info prior step
            DatabaseInstance toDatabaseInstance = new DatabaseInstance();

            // copy individual values as not top copy the reference (we need it later)
            toDatabaseInstance = fromDatabaseInstance.ShallowCopy();

            // override the communication id w/ the "to" communication id
            toDatabaseInstance.id = 0;              
            toDatabaseInstance.communicationId = toCommunication.id;
            // insert new database instance - get the id from this request
            toDatabaseInstance = DatabaseInstanceDAO.PostUpdate(toDatabaseInstance);

            // get all database tables from the fromDatabaseInstance 
            List<Configuration> fromDatabaseTables = ConfigurationDAO.GetDatabaseTables(fromDatabaseInstance);

            // get the database table relations
            List<DatabaseTableRelation> databaseTableRelations = masterConfigration.databaseTableRelations;

            // create a new database relation object
            DatabaseTableRelation databaseTableRelation = new DatabaseTableRelation() 
            {
                id = 0,
                requiresIdentity = true,
                sourceDatabaseTableId = -1,
                targetDatabaseTableId = -1
            };

            // foreach table that belongs to the from database_instance, get the from database_table information
            fromDatabaseTables.ForEach(delegate(Configuration configurationTable) 
            {
                // extract the database table from the configuration
                DatabaseTable fromDatabaseTable = new DatabaseTable()
                {
                    id = configurationTable.databaseTable.id,
                    databaseInstanceId = fromDatabaseInstance.id,
                    name = configurationTable.databaseTable.name
                };

                // create new database table
                DatabaseTable toDatabaseTable = new DatabaseTable()
                {
                    databaseInstanceId = toDatabaseInstance.id,
                    name = fromDatabaseTable.name
                };
                            
                // insert new table into database_table with fromDatabaseTable information but use toDatabaseInstanceId
                toDatabaseTable = DatabaseTableDAO.PostUpdate(toDatabaseTable);

                // check for prior source relation
                if (ConfigurationUtility.IsDatabaseTableRelation(configurationTable.databaseTable, databaseTableRelations))
                {
                    databaseTableRelation.sourceDatabaseTableId = toDatabaseTable.id;
                }

                // check for prior target relation
                if (ConfigurationUtility.IsDatabaseTableRelation(configurationTable.databaseTable, databaseTableRelations, true))
                {
                    databaseTableRelation.targetDatabaseTableId = toDatabaseTable.id;
                }

                // based on the fromDatabaseTable get all column sets
                List<Configuration> fromColumnSets = ConfigurationDAO.GetDatabaseColumns(fromDatabaseTable);

                // foreach columnset that belongs to fromDatabaseColumn copy all information (except for the fromDatabaseTableId)
                fromColumnSets.ForEach(delegate(Configuration configurationColumnSet) 
                {
                    // define the column set
                    ColumnSet fromColumnSet = new ColumnSet();
                    ColumnSet toColumnSet = new ColumnSet();

                    // get the column set from the configuration list
                    fromColumnSet = configurationColumnSet.columnSet;
                    fromColumnSet.databaseTableId = configurationColumnSet.databaseTable.id;

                    // do a shallow copy of its properties and override the ones you need
                    toColumnSet = fromColumnSet.ShallowCopy();
                    toColumnSet.id = 0;
                    toColumnSet.databaseTableId = toDatabaseTable.id;

                    // insert new toColumnSet using new toDAtabaseTable.id
                    toColumnSet = ColumnSetDAO.PostUpdate(toColumnSet);                   
                }); 
            });

            // if relation source or target is not negative one - insert new relation
            if(databaseTableRelation.sourceDatabaseTableId != -1 && 
               databaseTableRelation.targetDatabaseTableId != -1)
            {
                databaseTableRelation = DatabaseTableRelationDAO.PostUpdate(databaseTableRelation);            
            }

        }
        public const string RSERVER = "RSERVER";

        public static void TestHL7Message()
        { 
            
        
        }

        public static void TestOBX()
        {
            StringBuilder sb = new StringBuilder();
            List<string> output = new List<string>();

            //StringBuilder output = new StringBuilder();
            string pathToFile = @"C:\Users\chrisb\Documents\ApplicationDevelopment\Working\HL7Broker.root\HL7Broker\HL7Broker\Messages\ORU.hl7";
            try
            {
                byte[] hl7Message = File.ReadAllBytes(pathToFile);

                foreach (byte character in hl7Message)
                {
                    sb.Append(Convert.ToChar(character));
                }
            }
            catch (Exception ex)
            {
                sb.Append("");
                throw ex;
            }

            List<string> message = sb.ToString().Split(Convert.ToChar(13)).ToList();

            message.ForEach(delegate(String segment) 
            {
                List<string> segmentedLines = segment.Split('|').ToList();

                if (segmentedLines[0] == "OBX")
                { 
                    // split by component (^)
                    
                    // get text in OBX[5]

                    // use output.add(<sometext>)                
                }                                              
            });

             // join output with BR
            string html = String.Join("<BR>", output.ToArray());

            // append html to both ends of html string
        
        }

        public static void TestConfigLoad()
        {
            Configuration masterConfig = ConfigurationDAO.GetAllConfigurations();

            List<Application> applications = masterConfig.applications;
            List<Communication> communications = masterConfig.communications;
            List<WebserviceObject> webserviceObjects = masterConfig.webserviceObjects;
            List<WebserviceInstance> webserviceInstances = masterConfig.webserviceInstances;
            List<WebservicePropertySet> webserviceProperties = masterConfig.webservicePropertySets;
            List<MessageGroup> messageGroups = masterConfig.messageGroups;

            applications.ForEach(delegate(Application app)
            {
                if (app.name != RSERVER)
                    return;

                // get the unprocessed message count for the application
                List<MessageBucket> brokerInformation 
                    = MessageBucketDAO.GetUnprocessedMessageHeaderInstancesByApplication(app);

                brokerInformation.ForEach(delegate(MessageBucket broker)
                {
                    // get the message header and message
                    MessageHeaderInstance messageHeaderInstance = broker.messageHeaderInstance;
                    Message message = broker.message;

                    // locally retrieve the communication object from memory
                    Communication communication
                        = ConfigurationUtility.GetIncomingWebserviceCommunication(app, communications);
                    // locally retrieve the webservice instance object from memory
                    WebserviceInstance webserviceInstance
                        = ConfigurationUtility.GetIncomingWebserviceInstance(communication, webserviceInstances);
                    // locally retrieve the web service objects from memory
                    List<WebserviceObject> wsObjects
                        = ConfigurationUtility.GetIncomingWebserviceObjects(webserviceInstance, webserviceObjects);

                    // for each object - for each property set for that object - handle accordingly
                    wsObjects.ForEach(delegate(WebserviceObject wsObject)
                    {
                        Console.WriteLine("OBJECT:" + wsObject.name);

                        List<WebservicePropertySet> wsProperties
                            = ConfigurationUtility.GetIncomingWebservicePropertySets(wsObject, webserviceProperties);

                        wsProperties.ForEach(delegate(WebservicePropertySet wsProperty)
                        {
                            Console.WriteLine("Property:" + wsProperty.name);

                            List<MessageGroup> msGroups
                                = ConfigurationUtility.GetIncomingWebserviceMessageGroup(wsProperty, messageGroups);

                            Console.WriteLine("Group Count:" + msGroups.Count);
                        });
                    });
                    // update the table to processed
                    // BrokerDAO.UpdateProcessedFlagAndMessageLog(messageHeaderInstance, message, true);
                });              
            });
        }

        public static void TestBrokerUpdates()
        {
            List<Application> applications = ApplicationDAO.Get();

            applications.ForEach(delegate(Application application) {

                if (application.name != "RSERVER")
                    return;

                List<MessageBucket> brokerInformation
                    = MessageBucketDAO.GetUnprocessedMessageHeaderInstancesByApplication(application);
                //List<Broker> brokerInformation
                      //= BrokerDAO.GetProcessedMessageHeaderInstancesByApplication(application);

                brokerInformation.ForEach(delegate(MessageBucket broker) {
                    
                    // get the message header and message
                    MessageHeaderInstance messageHeaderInstance = broker.messageHeaderInstance;
                    Message message = broker.message;

                    // update the table to processed
                    MessageBucketDAO.UpdateProcessedFlagAndMessageLog(messageHeaderInstance, message, true);        
                    //BrokerDAO.ReprocessMessage(messageHeaderInstance.messageControlId);                
                });
            });
        }
        public static void TestSocketListener()
        {

            // Data buffer for incoming data.
            byte[] bytes = new Byte[6000];

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the 
            // host running the application.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 12000);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and 
            // listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.
                    while (true)
                    {
                        bytes = new byte[6000];
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        break;
                    }

                    // Show the data on the console.
                    Console.WriteLine("Text received : {0}", data);

                    // Echo the data back to the client.
                    byte[] msg = Encoding.ASCII.GetBytes(File.ReadAllText("TestAck.txt",Encoding.ASCII));

        

                    handler.Send(msg);

                    
                    //handler.Shutdown(SocketShutdown.Both);
                    //handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        
        
        }
        public static void TestSocket()
        { 
         // Data buffer for incoming data.
        byte[] bytes = new byte[1024];

        // Connect to a remote device.
        try {
            // Establish the remote endpoint for the socket.
            // This example uses port 11000 on the local computer.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress,9999);

            // Create a TCP/IP  socket.
            Socket sender = new Socket(AddressFamily.InterNetwork, 
                SocketType.Stream, ProtocolType.Tcp );

            // Connect the socket to the remote endpoint. Catch any errors.
            try {
                sender.Connect(remoteEP);

                Console.WriteLine("Socket connected to {0}",
                    sender.RemoteEndPoint.ToString());

                // Encode the data string into a byte array.
                byte[] msg = Encoding.ASCII.GetBytes(File.ReadAllText("TestAck.txt", Encoding.ASCII));

                // Send the data through the socket.
                int bytesSent = sender.Send(msg);

                // Receive the response from the remote device.
                //int bytesRec = sender.Receive(bytes);
                //Console.WriteLine("Echoed test = {0}",
                    //Encoding.ASCII.GetString(bytes,0,bytesRec));

                // Release the socket.
                //sender.Shutdown(SocketShutdown.Both);
                //sender.Close();
                
            } catch (ArgumentNullException ane) {
                Console.WriteLine("ArgumentNullException : {0}",ane.ToString());
            } catch (SocketException se) {
                Console.WriteLine("SocketException : {0}",se.ToString());
            } catch (Exception e) {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }

        } catch (Exception e) {
            Console.WriteLine( e.ToString());
        }
        
        
        }
        public static void TestService()
        {            
            // obtain a list of applications
            List<Application> applications = ApplicationDAO.Get();
            
            // go through each application, get configurations
            foreach (Application app in applications)
	        {
                // get the configuration
                List<Configuration> configurations = ConfigurationDAO.GetApplications(app);
	        }
                          
        }

        public static void makeNewMessageGroups()
        {

            List<String> lines = new List<string>();

            lines = File.ReadAllLines(@"C:\Users\chrisb\Source\Repos\HL7Broker\HL7BrokerConsoleTest\NewMessageGroupForWebService.txt").ToList();

            lines.ForEach(delegate(String line) {

                // skip comments
                if (line.Contains("##"))
                    return;

                string[] elementSplit = line.Split('|');

                // gives the first part
                string[] tempMessageGroupInstance = elementSplit[0].Split(',');

                Console.WriteLine("Inserting Message Group Instance");
                MessageGroupInstance messageGroupInstance
                    = MessageGroupInstanceDAO.PostUpdate(new MessageGroupInstance()
                    {
                        messageTypeId = Convert.ToInt32(tempMessageGroupInstance[0]),
                        segmentTypeId = Convert.ToInt32(tempMessageGroupInstance[1]),
                        description   =  tempMessageGroupInstance[2]
                    });

                if (messageGroupInstance.id != -1 || messageGroupInstance.id != 0)
                {
                    List<String> tempMessageGroup = elementSplit[1].Split(',').ToList();

                    tempMessageGroup.ForEach(delegate(String tempElement) {

                        Console.WriteLine("Inserting Message Group");

                        string[] tempInnerElement = tempElement.Split('^');

                        MessageGroupDAO.PostUpdate(new MessageGroup()
                        {
                            messageGroupInstanceId = messageGroupInstance.id,
                            messagePartId = Convert.ToInt32(tempInnerElement[0]),
                            position = Convert.ToInt32(tempInnerElement[1])
                        });                        
                    });

                    Console.WriteLine("Inserting Message Group Property or Column Set");

                    // gives the first part
                    string[] tempColumnSet = elementSplit[2].Split(',');
                    /*
                    ColumnSet columnSet = new ColumnSet()
                    {
                        databaseTableId = Convert.ToInt32(tempColumnSet[0]),
                        name = tempColumnSet[1],
                        messageGroupInstanceId = messageGroupInstance.id,
                        isPrimaryKey = Convert.ToBoolean(tempColumnSet[2]),
                        columnDataType = tempColumnSet[3]
                    };

                    ColumnSetDAO.PostUpdate(columnSet);
                    */

                    WebservicePropertySet webservicePropertySet = new WebservicePropertySet()
                    {
                        webserviceObjectId = Convert.ToInt32(tempColumnSet[0]),
                        name = tempColumnSet[1],
                        messageGroupInstanceId = messageGroupInstance.id,
                        columnDataType = tempColumnSet[2]
                    };

                    WebservicePropertySetDAO.PostUpdate(webservicePropertySet);

                }    
            });
            Console.Read();
        }

        public static void newWebServiceInstanceAndTables()
        {
            // creates a new web service instance
            WebserviceInstance webserviceInstance = new WebserviceInstance() 
            { 
                communicationId = 18,
                credentialId = 7,
                ipAddress = "172.31.100.103",
                server = "https://shcappprod.shc.shcnet.pri/WSShieldsApps/ShieldsApps_MSFTONLY.svc",
                name = "WSShieldsApps"            
            };

            // should have an id
            webserviceInstance = WebserviceInstanceDAO.PostUpdate(webserviceInstance);

            // create new web service objects
            List<String> lines = new List<string>();

            lines = File.ReadAllLines(@"C:\Users\chrisb\Documents\ApplicationDevelopment\Working\HL7Broker.root\HL7Broker\HL7BrokerConsoleTest\NewWebServiceObjects.txt").ToList();

            lines.ForEach(delegate(String line) 
            {
                WebserviceObject webserviceObject = new WebserviceObject() 
                { 
                    id = 0,
                    name = line,
                    webserviceInstanceId = webserviceInstance.id               
                };

                // post update new service object
                webserviceObject = WebserviceObjectDAO.PostUpdate(webserviceObject);
                
                // write out the identities to process the columns
                Console.WriteLine(webserviceObject.name + "->" + webserviceObject.id);            
            });

            Console.Read();
        }
        public static void makeNewMessageGroupsWebservice() 
        { 
            
        
        }

        public static void TestSys()
        {
            Console.WriteLine("APPLICATION TEST");
            ApplicationTest.Begin(new Application()
            {
                description = "TEST INSERT",
                name = "TEST_CONFIG"
            });

            Console.WriteLine("COLUMN SET TEST");
            ColumnSetTest.Begin(new ColumnSet()
            {
                name = "TEST_COLUMN",
                isPrimaryKey = false
            });

            Console.WriteLine("COMMUNICATION SET TEST");
            CommunicationTest.Begin(new Communication()
            {
                applicationId = 0
            });

            Console.WriteLine("COMMUNICATION TYPE TEST");
            CommunicationTypeTest.Begin(new CommunicationType()
            {
                name = "Test Communication Type"
            });

            Console.WriteLine("CREDENTIAL TEST");
            CredentialTest.Begin(new Credential()
            {
                username = "Test Credential",
                password = "Test Credential"
            });

            Console.WriteLine("CREDENTIAL TYPE TEST");
            CredentialTypeTest.Begin(new CredentialType()
            {
                name = "Test Type"
            });

            Console.WriteLine("DATABASE INSTANCE TEST");
            DatabaseInstanceTest.Begin(new DatabaseInstance()
            {
                name = "Database Instance Test",
                ipAddress = "0.0.0.0",
                server = "Test Server",
                communicationId = 0
            });

            Console.WriteLine("DATABASE TABLE TEST");
            DatabaseTableTest.Begin(new DatabaseTable()
            {
                name = "Database Table Instance Test"
            });

            Console.WriteLine("DIRECTION TYPE");
            DirectionTypeTest.Begin(new DirectionType()
            {
                name = "Direction Type Test"
            });

            Console.WriteLine("INTERFACE TEST");
            InterfaceTest.Begin(new Interface()
            {
                ipAddress = "0.0.0.0",
                maxConnections = 1
            });

            Console.WriteLine("MESSAGE GROUP INSTANCE");
            MessageGroupInstanceTest.Begin(new MessageGroupInstance()
            {
                description = "Message Group Instance Test"
            });

            Console.WriteLine("MESSAGE GROUP");
            MessageGroupTest.Begin(new MessageGroup()
            {
                messageGroupInstanceId = 0,
                messagePartId = 1,
                position = 1
            });

            Console.WriteLine("MESSAGE PART");
            MessagePartTest.Begin(new MessagePart()
            {
                name = "TEST",
                delimiter = '+'
            });

            Console.WriteLine("MESSAGE Type TEST");
            MessageTypeTest.Begin(new MessageType()
            {
                name = "ZZ TEST Message Type",
            });

            Console.WriteLine("SEGMENT TYPE");
            SegmentTypeTest.Begin(new SegmentType()
            {
                name = "ZZ SEGMENT Type"
            });

            Console.Read();
        
        }
       
        
    }
}
