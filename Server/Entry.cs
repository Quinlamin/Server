using System;
using System.Threading;
using MySql.Data;
using MySql.Data.MySqlClient;
//using System.Net.Http;

namespace Server
{
    internal class Entry
    {
        public static DBConnection dbCon;
        private static bool isRunning;
        static void Main(string[] args)
        {
            dbCon = DBConnection.Instance();
            dbCon.Server = "127.0.0.1";
            dbCon.DatabaseName = "gamedata";
            dbCon.UserName = "admin";
            dbCon.Password = "D-fR-wPdkeggJ21t";

            Console.Title = "Game Server";
            isRunning = true;

            

            Server.Start(26950, 50);

            Console.Read();
            try
            {
                
                Packet packet = new Packet(0);
                packet.AddString("hello", 10);
                packet.AddInt32(15311);
                Server.clientList[0].ServerSend(packet);
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
            }

            while (true)
            {

            }
            Console.Read();
            /*
            if (dbCon.IsConnect())
            {
                string username;
                while (true)
                {
                    Console.WriteLine("Please enter Username");
                    username = Console.ReadLine();
                    if (username.ToCharArray().Length < 15)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("too long");
                    }
                }
                string password;
                while (true)
                {
                    Console.WriteLine("Please enter Password");
                    password = Console.ReadLine();
                    if (password.ToCharArray().Length < 20)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("too long");
                    }
                }
                string email;
                while (true)
                {
                    Console.WriteLine("Please enter email");
                    email = Console.ReadLine();
                    if (email.ToCharArray().Length < 50)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("too long");
                    }
                }
                int newsletter;
                while (true)
                {
                    Console.WriteLine("Would you like emails");
                    string answer = Console.ReadLine();
                    if(answer == "y")
                    {
                        newsletter = 1;
                        break;
                    }
                    else
                    {
                        newsletter = 0;
                        break;
                    }
                }
                string query = "INSERT INTO `player` (`username`, `password`, `email`, `newsletter`) VALUES ( '"+username+"', '"+password+"', '"+email+"', '"+newsletter+"')";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                reader.Close();
                query = "SELECT * FROM player";

                cmd = new MySqlCommand(query, dbCon.Connection);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string someStringFromColumnZero = reader.GetInt32(0).ToString();
                    string someStringFromColumnOne = (string)reader.GetString(1);
                    Console.Write(someStringFromColumnZero + "," + someStringFromColumnOne);
                    
                }

            }



            Console.Read();
            */
        }
        

    }
}
