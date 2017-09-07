using HL7BrokerSuite.Sys.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7Broker.Model
{
    public class OutboundHandler
    {
        private Worker messageProcessingWorker;        
        private string applicationName { get; set; }
        private Configuration masterConfiguration;

        // create a delegate to handle the incoming stream
        public delegate void Worker(Configuration masterConfiguration, OutboundHandler outboundHandler);

        public void setProcessingWorker(Worker worker)
        {
            this.messageProcessingWorker = worker;        
        }

        public string getApplicationName()
        {
            return this.applicationName;
        }

        public void setApplicationName(string appName)
        {
            this.applicationName = appName;
        }

        public void setMasterConfiguration(Configuration masterConfig)
        {
            masterConfiguration = masterConfig;
        }

        public void Begin()
        {
            this.messageProcessingWorker(this.masterConfiguration, this);        
        }
    }
}
