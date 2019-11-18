using System;
using _01.InitialSetup;
using System.Data.SqlClient;

namespace _02.VillainNames
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                connection.Open();

                string minionsInfo = "  SELECT v.Name, COUNT(mv.VillainId) AS MinionsCount  " +
                                     "    FROM Villains AS v                                " +
                                     "    JOIN MinionsVillains AS mv ON v.Id = mv.VillainId " +
                                     "GROUP BY v.Id, v.Name                                 " +
                                     "  HAVING COUNT(mv.VillainId) > 3                      " +
                                     "ORDER BY COUNT(mv.VillainId)";

                using (SqlCommand command = new SqlCommand(minionsInfo,connection))
                {
                    
                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Console.WriteLine($"{dataReader[0]} - {dataReader[1]}");
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
