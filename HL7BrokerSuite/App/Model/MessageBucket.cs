using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HL7BrokerSuite.App.Model
{
    public class MessageBucket
    {
        public MessageHeaderInstance messageHeaderInstance { get; set; }
        public Message message { get; set; }

        public List<MessageHeaderInstance> messageHeaderInstances { get; set; }
        public List<Message> messages { get; set; }

        public enum View
        {
            GetMessageHeaderInstances
        };

        public MessageBucket()
        {
            initializeLists();
        }

        public MessageBucket(IDataRecord reader, View view)
        {
            initializeWithView(reader, view);
        }

        private void initializeWithView(IDataRecord reader, View view)
        {
            switch (view)
            {
                case View.GetMessageHeaderInstances:
                    this.messageHeaderInstance = new MessageHeaderInstance(reader, true);
                    this.message = new Message(reader, true);                    
                    break;
                default:
                    break;
            }
        }

        private void initializeLists()
        {
            this.messageHeaderInstances = new List<MessageHeaderInstance>();
            this.messages = new List<Message>();
        }
    }
}
