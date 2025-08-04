using Microsoft.Data.SqlClient;
using Project_Manager.Models;

namespace Project_Manager.Services
{
    public class ClientService
    {
        private readonly string _connectionString;

        public ClientService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // ADDING CREATING MY NEW CLIENT
        public void AddClient(ClientInfo client)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                INSERT INTO Clients (
                    FirstName, LastName, Email, PhoneNumber, Company
                )  
                VALUES (
                    @FirstName, @LastName, @Email, @PhoneNumber, @Company
                )";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@FirstName", client.Fname);
            cmd.Parameters.AddWithValue("@LastName", client.Lname);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrWhiteSpace(client.PhoneNum) ? DBNull.Value : client.PhoneNum);
            cmd.Parameters.AddWithValue("@Company", string.IsNullOrWhiteSpace(client.Company) ? DBNull.Value : client.Company);

            cmd.ExecuteNonQuery();
        }

        // GETTING CLIENTS WITH SEARCHING 
        public List<ClientInfo> GetClients(string? searchTerm = null)
        {
            List<ClientInfo> clients = new();

            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                SELECT
                    ClientId,
                    FirstName,
                    LastName,
                    Email,
                    PhoneNumber,
                    Company
                FROM Clients
                WHERE (
                    @SearchTerm IS NULL OR 
                    FirstName LIKE @Search OR 
                    LastName LIKE @Search OR    
                    Email LIKE @Search
                )";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@SearchTerm", string.IsNullOrWhiteSpace(searchTerm) ? DBNull.Value : searchTerm);
            cmd.Parameters.AddWithValue("@Search", $"%{searchTerm}%");

            using SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ClientInfo client = new()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ClientId")),
                    Fname = reader.GetString(reader.GetOrdinal("FirstName")),
                    Lname = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PhoneNum = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                        ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Company = reader.IsDBNull(reader.GetOrdinal("Company"))
                        ? null : reader.GetString(reader.GetOrdinal("Company"))
                };

                clients.Add(client);
            }

            return clients;
        }

        // GETTING MY CLIENT BY ID
        public ClientInfo? GetClientById(int id)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                SELECT ClientId, FirstName, LastName, Email, PhoneNumber, Company
                FROM Clients
                WHERE ClientId = @Id";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ClientInfo
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ClientId")),
                    Fname = reader.GetString(reader.GetOrdinal("FirstName")),
                    Lname = reader.GetString(reader.GetOrdinal("LastName")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    PhoneNum = reader.IsDBNull(reader.GetOrdinal("PhoneNumber"))
                        ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Company = reader.IsDBNull(reader.GetOrdinal("Company"))
                        ? null : reader.GetString(reader.GetOrdinal("Company"))
                };
            }

            return null;
        }

        // UPDATING MY CLIENT
        public void UpdateClient(ClientInfo client)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                UPDATE Clients SET
                    FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    PhoneNumber = @PhoneNumber,
                    Company = @Company
                WHERE ClientId = @ClientId";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@FirstName", client.Fname);
            cmd.Parameters.AddWithValue("@LastName", client.Lname);
            cmd.Parameters.AddWithValue("@Email", client.Email);
            cmd.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrWhiteSpace(client.PhoneNum) ? DBNull.Value : client.PhoneNum);
            cmd.Parameters.AddWithValue("@Company", string.IsNullOrWhiteSpace(client.Company) ? DBNull.Value : client.Company);
            cmd.Parameters.AddWithValue("@ClientId", client.Id);

            cmd.ExecuteNonQuery();
        }

        // DELETING MY CLIENT
        public void DeleteClient(int id)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = "DELETE FROM Clients WHERE ClientId = @ClientId";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@ClientId", id);

            cmd.ExecuteNonQuery();
        }
    }
}
