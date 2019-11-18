using System;
using System.Data.SqlClient;
using _01.InitialSetup;

namespace _09.IncreaseAgeStoredProcedure
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                connection.Open();

                IncreaseMinionAgeById(minionId, connection);

                PrintMinionsNameAndAge(minionId, connection);

                connection.Close();
            }
        }

        private static void PrintMinionsNameAndAge(int minionId, SqlConnection connection)
        {
            var cmdText = $"SELECT m.Name, m.Age FROM Minions M WHERE m.Id = {minionId}";

            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} - {reader[1]} years old");
                    }
                }
            }
        }

        private static void IncreaseMinionAgeById(int minionId, SqlConnection connection)
        {
            var cmdText = $"EXEC dbo.usp_GetOlder @minionId = {minionId}";

            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
