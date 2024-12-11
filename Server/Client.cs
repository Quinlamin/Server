using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    internal class Client
    {
        public float timeout = 10000;
        public Socket client;
        public Thread thisThread;
        public int pID;
        public AccountDetails accountDetails;
        bool ownsClient;
        bool waitingAck = false;
        bool running = true;
        public Client(Socket _client, bool _ownsClient)
        {
            client = _client;
            ownsClient = _ownsClient;
            ListenToClient();
        }
        public async void ServerSend(Packet packet)
        {
            byte[] data = packet.data;
            Console.WriteLine("sending data to client");
            try
            {
                _ = await client.SendAsync(new ArraySegment<byte>(data), SocketFlags.None);
                Console.WriteLine("Sent Data");
                waitingAck = true;
                var buffer = new byte[1024];
                var delayTask = Task.Delay(TimeSpan.FromMilliseconds(timeout));

                try
                {
                    var received = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);

                    waitingAck = false;
                    Console.WriteLine("received");
                    var ackPacket = new Packet(buffer);
                }
                catch (Exception)
                {
                    Console.WriteLine($"Client {pID} has timed out. Disconnecting...");
                    Server.clientList[pID] = null;
                    running = false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Client {pID} has timed out. Disconnecting...");
                Server.clientList[pID] = null;
                running = false;
            }
            
            
        }


    
        bool DelegatableSend(Packet packet)
        {
            return Server.delegatesSend[packet.packetID];
        }
        bool DelegatableRecieve(Packet packet)
        {
            return Server.delegatesReceive[packet.packetID];
        }
        async void DelegateRecieve(Packet packet)
        {

        }
        async void DelegateSend(Packet packet)
        {

        }
        public async Task ListenToClient()
        {
            while (running)
            {
                while (!waitingAck)
                {
                    var buffer = new byte[1024];
                    var received = await client.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    Packet receivedPacket = new Packet(buffer);
                    

                    if(DelegatableRecieve(receivedPacket))
                    {
                        var pack = new Packet(0);
                        pack.AddString("ack", 3);
                        _ = await client.SendAsync(new ArraySegment<byte>(pack.data), SocketFlags.None);
                    }
                    else
                    {
                        DelegateRecieve(receivedPacket);
                    }
                    

                }
            }
        }
        

        /*public async Task Connect()
        {
            try
            {
                using (var stream = client.GetStream())
                {
                    using (var sr = new StreamReader(stream))
                    {
                        using (var sw = new StreamWriter(stream))
                        {
                            byte[] inputData = new byte[256];
                            Int32 bytes = stream.Read(inputData, 0, inputData.Length);
                            string pingData = System.Text.Encoding.ASCII.GetString(inputData,0,bytes);
                            string message = "";
                            if(pingData == "con")
                            {
                                bool runToEnd = true;
                                for (int i = 0; i < Server.clientList.Count; i++)
                                {
                                    if (Server.clientList[i] == null)
                                    {
                                        Server.clientList[i] = this;
                                        runToEnd = false;
                                    }
                                }
                                if (runToEnd)
                                {
                                    Server.clientList.Add(this);
                                }
                            }
                            
                            await sw.WriteAsync(message).ConfigureAwait(false);
                            await sw.FlushAsync().ConfigureAwait(false);
                            var data = default(string);
                            while (!((data = await sr.ReadLineAsync().ConfigureAwait(false)).Equals("exit", StringComparison.OrdinalIgnoreCase)))
                            {
                                await sw.WriteLineAsync(data).ConfigureAwait(false);
                                await sw.FlushAsync().ConfigureAwait(false);
                            }
                        }
                    }
                }
            }
            finally
            {

            }
        }*/
    }
}
