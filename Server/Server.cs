using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace Server
{
    static class Server
    {
        public static Dictionary<int, bool> delegatesReceive;
        public static Dictionary<int, bool> delegatesSend;
        public static MySqlConnection connection;
        static int port;
        static int maxPlayers;

        public static List<Client> clientList;

        public static void Start(int _port, int _maxPlayers)
        {
            port = _port;
            maxPlayers = _maxPlayers;
            clientList = new List<Client>();
            delegatesReceive = new Dictionary<int, bool>() { 
                { 0, false }, // acknowledge
                { 1, true } // Login Attempt

            };
            delegatesSend = new Dictionary<int, bool>()
            {
                { 0, false }, // test
                
            };
            MainAsync();
        }
        
        static async Task MainAsync()
        {
            Console.WriteLine("Starting network server...");
            
            Socket listener = new Socket(IPAddress.Parse("127.0.0.1").AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 26950);
            listener.Bind(iPEndPoint);
            listener.Listen(100);
            Console.WriteLine("Started.");
            while (true)
            {
                var handler = await listener.AcceptAsync();

                HandleMessage(handler);
                /*
                while (true)
                {
                    var buffer = new byte[1024];

                    var received = await handler.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                    var response = Encoding.UTF8.GetString(buffer, 0, received);

                    var eom = "<|EOM|>";
                    if (response.IndexOf(eom) > -1)
                    {
                        Console.WriteLine($"Socket Server received message: /{response.Replace(eom, "")}/");

                        var ackMessage = "welcome";
                        var echoBytes = Encoding.UTF8.GetBytes(ackMessage);
                        Console.WriteLine($"Socket Server sent acknowledgement: /{ackMessage}/");
                    }



                    break;
                    

                }
                */
            }
        }

        static async Task HandleMessage(Socket handler)
        {
            while (true)
            {
                var buffer = new byte[1024];

                var received = await handler.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                Packet packetReceived = new Packet(buffer);
                
                if (packetReceived.packetID == 0)
                {

                    
                    


                    Console.WriteLine("Connecting new user");
                    bool isContinue = true;

                    int pID = -1;
                    for (int i = 0; i < clientList.Count; i++)
                    {

                        if (clientList[i] == null)
                        {
                            pID = i;
                            isContinue = false;
                            Console.WriteLine("Found open spot");
                            break;
                        }
                    }

                    if (!isContinue)
                    {
                        clientList[pID] = new Client(handler, true);


                    }
                    else
                    {
                        clientList.Add(new Client(handler, true));
                        Console.WriteLine("Created New Client Instance");
                        pID = clientList.Count - 1;
                    }


                    clientList[pID].pID = pID;



                    Packet ackPacket = new Packet(1);
                    ackPacket.AddInt32(pID);

                    
                    Console.WriteLine($"Socket Server sent acknowledgement packet: /{ackPacket.packetID}/");
                    _ = await handler.SendAsync(new ArraySegment<byte>(ackPacket.data), SocketFlags.None);


                    break;
                }
            }
            



        }

    }
}
