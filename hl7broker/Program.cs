//#define STAGING
//#define DEVELOPMENT
// Uncomment the #undef below before going to production
//#undef STAGING
//#undef DEVELOPMENT
using HL7Broker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HL7Broker
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Setting Debug Type
        /// if config.IsDebugMode is set to true
        /// then it will use the configured ip address
        /// DO NOT USE LOCAL HOST, AKA, 127.0.0.1
        /// Debug Level:
        ///     0 - Both inbound and outbound
        ///     1 - Inbound Only
        ///     2 - Outbound Only
        /// </summary>
        static void Main()
        {
            Config config = new Config();
            Inbound inbound = new Inbound();
            Outbound outbound = new Outbound();

            #if DEBUG //|| STAGING || DEVELOPMENT
            if (config.IsDebugMode)
            {
                ErrorLogger.Log(ErrorLogger.LogType.INFORMATION, "Entering Debug Mode: " + config.DebugLevel.ToString());
                switch (config.DebugLevel)
                {
                    case 0:
                        inbound.onDebugStart();
                        outbound.onDebugStart();
                        break;
                    case 1:
                        inbound.onDebugStart();
                        break;
                    case 2:
                        outbound.onDebugStart();
                        break;
                    case 3:
                        outbound.onDebugStartWithMessage();
                        break;
                    default:
                        inbound.onDebugStart();
                        outbound.onDebugStart();
                        break;
                }
            }
            else
            {
                inbound.onDebugStart();
                outbound.onDebugStart();
            }
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);   
            #else
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
                { 
                    new Inbound(),
                    new Outbound()
                };
            ServiceBase.Run(ServicesToRun);
            #endif
        }
    }
}
