using System;
using System.Data.SqlClient;
using _01.InitialSetup;
using System.Linq;

namespace _04.AddMinion
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            var minionInfo = Console.ReadLine()?.Split();

            var minionName = minionInfo?[1];
            var minionAge = minionInfo?[2];
            var minionTown = minionInfo?[3];

            var villainName = Console.ReadLine()?.Split().ElementAt(1);

            using (SqlConnection connection = new SqlConnection(Configuration.ConnectionStringToMinionsDB))
            {
                connection.Open();

                var townId = GetTownId(connection, minionTown);
                var villainId = GetVillainName(connection, villainName);
                var minionId = GetMinion(connection, minionName, minionAge, townId);
                AssignMinionsToVillains(connection, villainId, minionId, minionName, villainName);

                connection.Close();
            }
        }

        private static void AssignMinionsToVillains(SqlConnection connection, int villainId, int minionId, 
                                                    string minionName, string villainName)
        {
            string query = "INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (@villainId, @minionId)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@villainId", villainId);
                command.Parameters.AddWithValue("@minionId", minionId);
                if (!AssignMinionsToVillainsExist(connection, villainId, minionId))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}.");
                }
            }
        }

        private static bool AssignMinionsToVillainsExist(SqlConnection connection, int villainId, int minionId)
        {
            string query = $"SELECT COUNT(*) FROM MinionsVillains WHERE MinionId = {villainId} AND VillainId = {minionId}";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if ((int)command.ExecuteScalar() > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static int GetTownId(SqlConnection connection, string minionTown)
        {
            string townIdSql = "SELECT Id FROM Towns WHERE Name = @townName";

            using (SqlCommand command = new SqlCommand(townIdSql, connection))
            {
                command.Parameters.AddWithValue("@townName", minionTown);

                if (command.ExecuteScalar() == null)
                {
                    InsertTownInDB(connection, minionTown);
                }

                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertTownInDB(SqlConnection connection, string minionTown)
        {
            var query = "INSERT INTO Towns (Name) VALUES (@townName)";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@townName", minionTown);
                command.ExecuteNonQuery();
                Console.WriteLine($"Town {minionTown} was added to the database.");
            }
        }

        private static int GetMinion(SqlConnection connection, string minionName, string minionAge, int townId)
        {
            string minionSql = "SELECT Id FROM Minions WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(minionSql, connection))
            {
                command.Parameters.AddWithValue("@Name", minionName);

                if (command.ExecuteScalar() == null)
                {
                    InsertMinion(connection, minionName, minionAge, townId);
                }
                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertMinion(SqlConnection connection, string minionName, string minionAge, int townId)
        {
            string query = "INSERT INTO Minions (Name, Age, TownId) VALUES (@name, @age, @townId)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@name", minionName);
                command.Parameters.AddWithValue("@age", minionAge);
                command.Parameters.AddWithValue("@townId", townId);

                command.ExecuteNonQuery();
            }
        }

        private static int GetVillainName(SqlConnection connection, string villainName)
        {
            string villainSql = "SELECT Id FROM Villains WHERE Name = @Name";

            using (SqlCommand command = new SqlCommand(villainSql, connection))
            {
                command.Parameters.AddWithValue("@Name", villainName);

                if (command.ExecuteScalar() == null)
                {
                    InsertVillainInDB(connection, villainName);
                }
                return (int)command.ExecuteScalar();
            }
        }

        private static void InsertVillainInDB(SqlConnection connection, string villainName)
        {
            string query = "INSERT INTO Villains (Name, EvilnessFactorId)  VALUES (@villainName, 4)";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@villainName", villainName);
                command.ExecuteNonQuery();
                Console.WriteLine($"Villain {villainName} was added to the database.");
            }
        }
    }
}
