using System;
using VisualCronAPI;
using System.Diagnostics;
using System.Linq;
using VisualCron;

namespace VisualCronMonitor
{
    public static class VisualCronHandler
    {
        private static IVisualCronEventHandler EventHandler { get; set; }

        public static void Run()
        {
            EventHandler = new VisualCronEventHandler();

            var serverAddresses = System.Configuration.ConfigurationManager.AppSettings["Servers"]
                .Split(',').Where(s => !s.ToLower()
                .Contains("off")).Select(s => s.Trim());

            foreach (var address in serverAddresses)
            {
                ConnectAndGetUsers(address);
            }
        }

        private static void ConnectAndGetUsers(string address)
        {
            var username = System.Configuration.ConfigurationManager.AppSettings["VisualCronUserName"];
            var password = System.Configuration.ConfigurationManager.AppSettings["VisualCronPassword"];


            using (var client = new Client { LogToFile = false })
            {
                var connection = new Connection
                {
                    Address = address,
                    UseADLogon = true,
                    Port = 16444,
                    ConnectionType = Connection.ConnectionT.Remote,
                    UserName = username,
                    PassWord = password

                };

                try
                {
                    var server = client.Connect(connection);
                    EventHandler.UpdateMonitoringTimeMessage(server);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Address: {0}, UseAdLogon: {1}, Port: {2}, ConnectionType: {3}, User: {4}, Password: {5}, Message: {6}, StackTrace: {7}",
                        connection.Address, connection.UseADLogon, connection.Port, connection.ConnectionType, connection.UserName, connection.PassWord, ex.Message, ex.StackTrace
                        ));
                }
            }
        }
    }
}
