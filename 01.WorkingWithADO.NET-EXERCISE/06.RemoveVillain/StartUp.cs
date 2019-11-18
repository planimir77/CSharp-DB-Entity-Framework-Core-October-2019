using System;
using System.Data.SqlClient;
using _01.InitialSetup;

namespace _06.RemoveVillain
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                connection.Open();

                int releasedMinions = ReleaseMinions(villainId, connection);

                var villainName = GetVillainName(villainId, connection);

                if (DeleteVillain(villainId, connection) > 0)
                {
                    Console.WriteLine($"{villainName} was deleted.");

                    Console.WriteLine($"{releasedMinions} minions were released.");
                }
                else
                {
                    Console.WriteLine("No such villain was found.");
                }

                connection.Close();
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection)
        {
            var villainName = $"SELECT v.Name FROM Villains v WHERE v.Id = {villainId}";

            using (SqlCommand command = new SqlCommand(villainName, connection))
            {
                return (string)command.ExecuteScalar();
            }
        }

        private static int ReleaseMinions(int villainId, SqlConnection connection)
        {
            var deleteMinionsVillains = $"DELETE MinionsVillains WHERE VillainId = {villainId}";

            using (SqlCommand command = new SqlCommand(deleteMinionsVillains, connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        private static int DeleteVillain(int villainId, SqlConnection connection)
        {
            
            var deleteVillain = $"DELETE Villains WHERE Id = {villainId}";
            
            using (SqlCommand command = new SqlCommand(deleteVillain, connection))
            {
                return command.ExecuteNonQuery();
            }
        }
    }
}
