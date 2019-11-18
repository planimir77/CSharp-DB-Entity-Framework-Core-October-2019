using System;
using _01.InitialSetup;
using System.Data.SqlClient;
using System.Text;

namespace _03.MinionNames
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                connection.Open();

                int villainId = int.Parse(Console.ReadLine());

                string villainName = GetVillainName(connection, villainId);

                string minions = GetMinions(connection, villainId);

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {villainName}");
                    Console.WriteLine(minions);
                }

                connection.Close();
            }
        }

        private static string GetMinions(SqlConnection connection, int villainId)
        {
            string query = "SELECT ROW_NUMBER() OVER(ORDER BY m.Name) as RowNum, "+
                           "         m.Name,                                     "+
                           "         m.Age                                       "+
                           "    FROM MinionsVillains AS mv                       "+
                           "    JOIN Minions As m ON mv.MinionId = m.Id          "+
                           "   WHERE mv.VillainId = @Id                          "+
                           "ORDER BY m.Name";

            StringBuilder sb = new StringBuilder();

            using (SqlCommand command = new SqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@Id", villainId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    int rowNumber = 1;

                    if (!reader.HasRows)
                    {
                        sb.AppendLine("(no minions)");

                    }
                    else
                    {
                        while (reader.Read())
                        {
                            sb.AppendLine($"{rowNumber++}. {reader[1]} {reader[2]}");
                        }
                    }
                }
            }

            return sb.ToString().TrimEnd();
        }

        private static string GetVillainName(SqlConnection connection, int villainId)
        {
            string query = "SELECT Name FROM Villains WHERE Id = @Id";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", villainId);

                return (string)command.ExecuteScalar();
            }
        }
    }
}
