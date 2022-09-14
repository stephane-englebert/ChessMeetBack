using DalChessMeet.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DalChessMeet.Repositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly string connectionString
        = @"server=(localdb)\MSSQLLocalDB;Initial Catalog = ChessDB; Integrated Security = True;";

        public IEnumerable<Entities.Member> GetMembers()
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM [Members]";
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                yield return new Entities.Member {
                    Id = (int)reader["id"],
                    Role = (string)reader["role"],
                    Pseudo = (string)reader["pseudo"],
                    Email = (string)reader["email"],
                    Birthdate = (DateTime)reader["birthdate"],
                    Gender = (string)reader["gender"],
                    Elo = (int)reader["elo"]
                };
            }
        }

        public Entities.MemberDetails GetMemberById(int id)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM [Members] WHERE id=@id";
            cmd.Parameters.AddWithValue("id",id);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return new Entities.MemberDetails {
                    Id = (int)reader["id"],
                    Role = (string)reader["role"],
                    Pseudo = (string)reader["pseudo"],
                    Email = (string)reader["email"],
                    Birthdate = (DateTime)reader["birthdate"],
                    Gender = (string)reader["gender"],
                    Elo = (int)reader["elo"]
                };
            };
            return null;
        }

        public Entities.MemberDetails GetMemberByCredentials(string name, string pwd)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE password = @pwd AND (pseudo = @name OR email = @name)";
            cmd.Parameters.AddWithValue("pwd", pwd);
            cmd.Parameters.AddWithValue("name",name);
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                return new Entities.MemberDetails
                {
                    Id = (int)reader["id"],
                    Role = (string)reader["role"],
                    Pseudo = (string)reader["pseudo"],
                    Email = (string)reader["email"],
                    Birthdate = (DateTime)reader["birthdate"],
                    Gender = (string)reader["gender"],
                    Elo = (int)reader["elo"]
                };
            }
            return null;
        }
        public void AddMember(Entities.Member member)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"INSERT INTO [dbo].[Members](role, pseudo, email, password, birthdate, gender, elo) VALUES(@role,@pseudo,@email,@password,@birthdate,@gender,@elo)";
            cmd.Parameters.AddWithValue("role", member.Role);
            cmd.Parameters.AddWithValue("pseudo", member.Pseudo);
            cmd.Parameters.AddWithValue("email", member.Email);
            cmd.Parameters.AddWithValue("password", member.Password);
            cmd.Parameters.AddWithValue("birthdate", member.Birthdate);
            cmd.Parameters.AddWithValue("gender", member.Gender);
            cmd.Parameters.AddWithValue("elo", member.Elo);
            cmd.ExecuteNonQuery();
        }

        public bool ExistEmail(string email)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE email=@email";
            cmd.Parameters.AddWithValue("email", email);
            bool emailFound = false;
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { emailFound = true; }
            return emailFound;
        }

        public bool ExistPseudo(string pseudo)
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM [Members] WHERE pseudo=@pseudo";
            cmd.Parameters.AddWithValue("pseudo", pseudo);
            bool pseudoFound = false;
            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read()) { pseudoFound = true; }
            return pseudoFound;
        }
    }
}
