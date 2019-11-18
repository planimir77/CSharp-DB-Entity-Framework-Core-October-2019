using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Channels;
using _01.InitialSetup;

namespace _05.ChangeTownNamesCasing
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                connection.Open();

                string country = Console.ReadLine();

                var rowsAffected = UpdateTownNamesToUppercaseByCountry(country, connection);

                if (rowsAffected > 0)
                {
                    Console.WriteLine($"{rowsAffected} town names were affected.");
                    PrintTowns(connection, country);
                }
                else
                {
                    Console.WriteLine("No town names were affected.");
                }

                connection.Close();
            }
        }

        private static void PrintTowns(SqlConnection connection, string country)
        {
            string cmdText = $"SELECT t.Name FROM Towns t JOIN Countries c ON t.CountryCode = c.Id WHERE c.Name = '{country}'";

            using (SqlCommand command = new SqlCommand(cmdText,connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    var towns = new List<string>();
                    while (reader.Read())
                    {
                        towns.Add((string)reader[0]);
                    }

                    Console.WriteLine($"[{String.Join(", ",towns)}]");
                }
            }
        }

        private static int UpdateTownNamesToUppercaseByCountry(string country, SqlConnection connection)
        {
            string cmdText =
                $"UPDATE Towns SET NAME = UPPER(Name) WHERE Id IN(SELECT T.Id FROM Towns t JOIN Countries c ON t.CountryCode = c.Id WHERE c.Name = '{country}')";

            using (SqlCommand command = new SqlCommand(cmdText, connection))
            {
                return command.ExecuteNonQuery();
            }
        }
    }
}
