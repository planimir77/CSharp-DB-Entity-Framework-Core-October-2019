using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using _01.InitialSetup;

namespace _07.PrintAllMinionNames
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minions = new List<string>();
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                connection.Open();

                minions = GetAllMinions(connection);
                
                int count = minions.Count;

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(minions[i]);
                    if (i < count-1)
                    {
                        Console.WriteLine(minions[count-1]);
                    }
                    count--;
                }
                
                connection.Close();

            }
        }

        private static List<string> GetAllMinions(SqlConnection connection)
        {
            var minions = new List<string>();
            var cmdText = "SELECT m.Name FROM Minions m";
            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minions.Add((string)reader[0]);
                    }
                }
            }

            return minions;
        }
    }
}
