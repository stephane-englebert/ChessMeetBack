using DalChessMeet.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private readonly string connectionString
        = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = ChessDB; Integrated Security = True;";
        public IEnumerable<Entities.Tournament> GetTournaments()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT TOP(10) * FROM [Tournaments] WHERE status != 'closed' ORDER BY updated_at DESC";
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new Entities.Tournament
                {
                    Guid = (string)reader["guid"],
                    Name = (string)reader["name"],
                    Place = (string)reader["place"],
                    PlayersMin = (int)reader["players_min"],
                    PlayersMax = (int)reader["players_max"],
                    EloMin = (int)reader["elo_min"],
                    EloMax = (int)reader["elo_max"],
                    Categories = (string)reader["categories"],
                    Status = (string)reader["status"],
                    CurrentRound = (int)reader["current_round"],
                    WomenOnly = (bool)reader["women_only"],
                    EndRegistration = (DateTime)reader["end_registration"],
                    CanRegister = true,
                    IsRegistered = false,
                    CountPlayers = HowManyPlayersRegisteredByTournament((string)reader["guid"])
                    };
            }
        }

        private int HowManyPlayersRegisteredByTournament(string g)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM [Registrations] WHERE tournament_guid = @gd";
            cmd.Parameters.AddWithValue("gd", g);
            int cpt = (int)cmd.ExecuteScalar();
            return cpt;
        }
        public IEnumerable<Entities.Tournament> GetTournamentsByUserId(int userId)
        {
            using SqlConnection connectionGuid = new SqlConnection(connectionString);
            connectionGuid.Open();
            using SqlCommand cmdGuid = connectionGuid.CreateCommand();
            cmdGuid.CommandText = @"SELECT r.tournament_guid FROM [Registrations] r JOIN [Tournaments] t ON r.tournament_guid = t.guid WHERE t.status != 'closed' AND r.player_id = @user_id";
            cmdGuid.Parameters.AddWithValue("user_id", userId);
            using SqlDataReader readerGuid = cmdGuid.ExecuteReader();
            List<string> userGuids = new List<string>(); 
            while (readerGuid.Read())
            {
                userGuids.Add((string)readerGuid["tournament_guid"]);
            }
            connectionGuid.Close();

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT TOP(10) * FROM [Tournaments] WHERE status != 'closed' ORDER BY updated_at DESC";
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string gd = (string)reader["guid"];
                bool userRegistered = userGuids.Exists(str => str == gd);
                bool canRegister = this.CanRegister(reader, userId, userRegistered);
                yield return new Entities.Tournament
                {
                    Guid = (string)reader["guid"],
                    Name = (string)reader["name"],
                    Place = (string)reader["place"],
                    PlayersMin = (int)reader["players_min"],
                    PlayersMax = (int)reader["players_max"],
                    EloMin = (int)reader["elo_min"],
                    EloMax = (int)reader["elo_max"],
                    Categories = (string)reader["categories"],
                    Status = (string)reader["status"],
                    CurrentRound = (int)reader["current_round"],
                    WomenOnly = (bool)reader["women_only"],
                    EndRegistration = (DateTime)reader["end_registration"],
                    CanRegister = canRegister,
                    IsRegistered = userRegistered,
                    CountPlayers = HowManyPlayersRegisteredByTournament((string)reader["guid"])
                };
            }
        }

        private Boolean CanRegister(System.Data.SqlClient.SqlDataReader data, int userId, Boolean isRegistered)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT birthdate,gender,elo FROM [Members] WHERE id=@id";
            cmd.Parameters.AddWithValue("id",userId);
            using SqlDataReader readerUser = cmd.ExecuteReader();
            int userElo = -1;
            string userGender = "";
            DateTime userBirthdate = DateTime.Now;
            while (readerUser.Read())
            {
                userElo = (int)readerUser["elo"];
                userGender = (string)readerUser["gender"];
                userBirthdate = (DateTime)readerUser["birthdate"];
            }
            if(userElo < 0) { return false; }
            int userAge = GetAge(userBirthdate,(DateTime)data["end_registration"]);
            string userCategory = "";
            if (userAge < 18) { userCategory = "junior"; }
            if (userAge >= 18 && userAge < 60) { userCategory = "senior"; }
            if (userAge >= 60) { userCategory = "veteran"; }
            string categ = (string)data["categories"];
            string[] categories = categ.Split(',');
            int categoryCount = 0;
            foreach (string category in categories){ if (userCategory == category) { categoryCount++; }}
            if ((string)data["status"] != "waitingForPlayers"){ return false; }
            if ((DateTime)data["end_registration"] < DateTime.Now){ return false; }
            if (isRegistered){ return false; }
            if ((int)data["players_min"] == (int)data["players_max"]){ return false; }
            if (categoryCount == 0) { return false; }
            if ((int)data["elo_max"] > 0 && userElo > (int)data["elo_max"]) { return false; }
            if ((int)data["elo_min"] > 0 && userElo < (int)data["elo_min"]) { return false; }
            if ((bool)data["women_only"] && userGender == "male") { return false; }
            return true;
        }
        private int GetAge(DateTime birthdate, DateTime endRegistrations)
        {
            int year_birthdate = birthdate.Year;
            int year_registration = endRegistrations.Year;
            return Math.Abs(year_registration - year_birthdate);
        }

        public Entities.TournamentDetails GetTournamentByGuid(string g)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            Entities.TournamentDetails details = new Entities.TournamentDetails();
            using SqlCommand cmd2 = connection.CreateCommand();
            cmd2.CommandText = @"SELECT * FROM [Tournaments] WHERE guid=@gd";
            cmd2.Parameters.AddWithValue("gd", g);
            using SqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                details.Guid = (string)reader2["guid"];
                details.Name = (string)reader2["name"];
                details.Place = (string)reader2["place"];
                details.PlayersMin = (int)reader2["players_min"];
                details.PlayersMax = (int)reader2["players_max"];
                details.EloMin = (int)reader2["elo_min"];
                details.EloMax = (int)reader2["elo_max"];
                details.Categories = (string)reader2["categories"];
                details.Status = (string)reader2["status"];
                details.CurrentRound = (int)reader2["current_round"];
                details.WomenOnly = (bool)reader2["women_only"];
                details.EndRegistration = (DateTime)reader2["end_registration"];
            }
            return details;
        }

        public IEnumerable<Entities.MemberDetails> GetTournamentPlayersDetails(string g)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members]m JOIN [Registrations]r ON m.id = r.player_id WHERE tournament_guid=@gd";
            cmd.Parameters.AddWithValue("gd", g);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new Entities.MemberDetails {
                    Id = (int)reader["id"],
                    Role = (string)reader["role"],
                    Pseudo = (string)reader["pseudo"],
                    Email = (string)reader["email"],
                    Birthdate = (DateTime)reader["birthdate"],
                    Gender =(string)reader["gender"],
                    Elo = (int)reader["elo"]
                };
            }
        }
        public void AddTournament(Entities.Tournament tournament)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [Tournaments](guid,name,place,players_min,players_max,elo_min,elo_max,categories,status,current_round,women_only,created_at,end_registration,updated_at) 
            VALUES(
                @guid,
                @name,
                @place,
                @players_min,
                @players_max,
                @elo_min,
                @elo_max,
                @categories,
                @status,
                @current_round,
                @women_only,
                @created_at,
                @end_registration,
                @updated_at
            )";
            cmd.Parameters.AddWithValue("guid", tournament.Guid);
            cmd.Parameters.AddWithValue("name", tournament.Name);
            cmd.Parameters.AddWithValue("place", tournament.Place);
            cmd.Parameters.AddWithValue("players_min", tournament.PlayersMin);
            cmd.Parameters.AddWithValue("players_max", tournament.PlayersMax);
            cmd.Parameters.AddWithValue("elo_min", tournament.EloMin);
            cmd.Parameters.AddWithValue("elo_max", tournament.EloMax);
            cmd.Parameters.AddWithValue("categories", tournament.Categories);
            cmd.Parameters.AddWithValue("status", tournament.Status);
            cmd.Parameters.AddWithValue("current_round", tournament.CurrentRound);
            cmd.Parameters.AddWithValue("women_only", tournament.WomenOnly);
            cmd.Parameters.AddWithValue("created_at", tournament.CreatedAt);
            cmd.Parameters.AddWithValue("end_registration", tournament.EndRegistration);
            cmd.Parameters.AddWithValue("updated_at", tournament.UpdatedAt);
            cmd.ExecuteNonQuery();
        }

        public bool ExistGuid(Guid g)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT COUNT(*) FROM [Tournaments] WHERE guid = @gd";
            cmd.Parameters.AddWithValue("gd", g);
            int cpt = (int)cmd.ExecuteScalar();
            return (cpt > 0);
        }

        public void DeleteTournament(string g)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"DELETE FROM [Tournaments] WHERE guid = @gd AND status='waitingForPlayers'";
            cmd.Parameters.AddWithValue("gd", g);
            cmd.ExecuteNonQuery();
        }
    }
}
