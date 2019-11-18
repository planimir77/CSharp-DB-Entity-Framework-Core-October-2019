using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using _01.InitialSetup;

namespace _08.IncreaseMinionAge
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                var minionsId = Console.ReadLine().Split().Select(int.Parse);

                connection.Open();

                UpdateMinionsAgeAndName(minionsId, connection);

                PrintMinionsNameAndAge(connection);

                connection.Close();
            }
        }

        private static void PrintMinionsNameAndAge(SqlConnection connection)
        {
            var cmdText = "SELECT m.Name, m.Age FROM Minions m";

            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} {reader[1]}");
                    }
                }
            }
        }

        private static void UpdateMinionsAgeAndName(IEnumerable<int> minionsId, SqlConnection connection)
        {
            var cmdText = $"UPDATE Minions SET Age += 1,NAME = UPPER(LEFT(Name,1))+LOWER(SUBSTRING(Name,2,LEN(Name))) WHERE Id IN({String.Join(',', minionsId)})";

            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
