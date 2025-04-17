using System;
using System.ServiceProcess;

namespace FileMonitoringWindows
{
    static class Program
    {
        static void Main()
        {
            var service = new FileMonitoringService();

            if (Environment.UserInteractive)
            {
                // Run as console application (Debug mode)
                service.StartDebugMode();

                Console.WriteLine("Service is running in debug mode. Press any key to stop...");
                Console.ReadKey();

                service.StopDebugMode();
            }
            else
            {
                // Run as a Windows Service
                ServiceBase.Run(new ServiceBase[] { service });
            }
        }
    }
}
