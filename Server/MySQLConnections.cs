using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    static class MySQLConnections
    {
        public static MySqlConnection connection;


        public static void Initialiser()
        {
            Console.WriteLine("Initialising SQL Connection");
            string connectionString = "datasource=127.0.0.1;port=3306;username=root;password=;database=spaceeconomy;";
            connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();

                MySqlDataReader reader = new MySqlCommand("select * from users",connection).ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader.GetInt32(0)} {reader.GetString(1)} {reader.GetString(2)} ");
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
