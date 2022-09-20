using DalChessMeet.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Repositories
{
    public class RegistrationRepository : IRegistrationRepository
    {
        private readonly string connectionString
        = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = ChessDB; Integrated Security = True;";

        public void AddRegistration(Entities.Registration registration)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [Registrations](player_id, tournament_guid) VALUES(@id, @gd)";
            cmd.Parameters.AddWithValue("id", registration.PlayerId);
            cmd.Parameters.AddWithValue("gd", registration.TournamentGuid);
            cmd.ExecuteNonQuery();
        }
        public void DeleteRegistration(Entities.Registration registration)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM [Registrations] WHERE tournament_guid = @gd AND player_id=@id";
            cmd.Parameters.AddWithValue("gd", registration.TournamentGuid);
            cmd.Parameters.AddWithValue("id", registration.PlayerId);
            cmd.ExecuteNonQuery();
        }
        public bool CheckIfRegistered(string gd, string player_id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Registrations] WHERE player_id = @player_id AND tournament_guid = @gd";
            cmd.Parameters.AddWithValue("player_id", int.Parse(player_id));
            cmd.Parameters.AddWithValue("gd", gd);
            SqlDataReader reader = cmd.ExecuteReader();
            int cpt = 0;
            while (reader.Read())
            {
                cpt++;
            }
            return cpt > 0;
        }
    }
}
