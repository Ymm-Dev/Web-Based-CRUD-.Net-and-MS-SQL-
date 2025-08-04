using Microsoft.Data.SqlClient;
using Project_Manager.Models;

namespace Project_Manager.Services
{
    public class ProjectService
    {
        private readonly string _connectionString;

        public ProjectService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // ADDING OR CREATING MY NEW PROJECT
        public void AddProject(ProjectInfo project)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                INSERT INTO Projects (
                    ClientId, ProjectTitle, Description, StartDate, Deadline, Status
                )
                VALUES (
                    @ClientId, @Title, @Description, @StartDate, @Deadline, @Status
                )";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@ClientId", project.ClientId);
            cmd.Parameters.AddWithValue("@Title", project.Title);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(project.Description) ? DBNull.Value : project.Description);
            cmd.Parameters.AddWithValue("@StartDate", project.StartDate ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Deadline", project.Deadline ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(project.Status) ? DBNull.Value : project.Status);

            cmd.ExecuteNonQuery();
        }

        // GET ALL MY PROJECTS W/ SEARCH
        public List<ProjectInfo> GetProjects(string? searchTerm = null)
        {
            List<ProjectInfo> projects = new();

            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                SELECT 
                    p.ProjectId, p.ProjectTitle, p.Description, p.StartDate, p.Deadline, p.Status,
                    c.ClientId, c.FirstName, c.LastName
                FROM Projects p
                JOIN Clients c ON p.ClientId = c.ClientId
                WHERE (
                    @SearchTerm IS NULL OR
                    p.ProjectTitle LIKE @Search OR
                    p.Description LIKE @Search OR
                    p.Status LIKE @Search OR
                    c.FirstName LIKE @Search OR
                    c.LastName LIKE @Search
                )";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@SearchTerm", string.IsNullOrWhiteSpace(searchTerm) ? DBNull.Value : searchTerm);
            cmd.Parameters.AddWithValue("@Search", $"%{searchTerm}%");

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                ProjectInfo project = new()
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                    Title = reader.GetString(reader.GetOrdinal("ProjectTitle")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? null : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    Deadline = reader.IsDBNull(reader.GetOrdinal("Deadline")) ? null : reader.GetDateTime(reader.GetOrdinal("Deadline")),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                    ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
                    ClientName = $"{reader.GetString(reader.GetOrdinal("FirstName"))} {reader.GetString(reader.GetOrdinal("LastName"))}"
                };

                projects.Add(project);
            }

            return projects;
        }

        // GET PROJECT BY ID (for EditING)
        public ProjectInfo? GetProjectById(int id)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                SELECT 
                    p.ProjectId, p.ClientId, p.ProjectTitle, p.Description, 
                    p.StartDate, p.Deadline, p.Status,
                    c.FirstName + ' ' + c.LastName AS ClientName
                FROM Projects p
                JOIN Clients c ON p.ClientId = c.ClientId
                WHERE p.ProjectId = @Id";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);

            using SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new ProjectInfo
                {
                    Id = reader.GetInt32(reader.GetOrdinal("ProjectId")),
                    ClientId = reader.GetInt32(reader.GetOrdinal("ClientId")),
                    Title = reader.GetString(reader.GetOrdinal("ProjectTitle")),
                    Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader.GetString(reader.GetOrdinal("Description")),
                    StartDate = reader.IsDBNull(reader.GetOrdinal("StartDate")) ? null : reader.GetDateTime(reader.GetOrdinal("StartDate")),
                    Deadline = reader.IsDBNull(reader.GetOrdinal("Deadline")) ? null : reader.GetDateTime(reader.GetOrdinal("Deadline")),
                    Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? null : reader.GetString(reader.GetOrdinal("Status")),
                    ClientName = reader.IsDBNull(reader.GetOrdinal("ClientName")) ? null : reader.GetString(reader.GetOrdinal("ClientName"))
                };
            }

            return null;
        }

        // UPDATING MY PROJECT
        public void UpdateProject(ProjectInfo project)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = @"
                UPDATE Projects SET
                    ClientId = @ClientId,
                    ProjectTitle = @Title,
                    Description = @Description,
                    StartDate = @StartDate,
                    Deadline = @Deadline,
                    Status = @Status
                WHERE ProjectId = @Id";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@Id", project.Id);
            cmd.Parameters.AddWithValue("@ClientId", project.ClientId);
            cmd.Parameters.AddWithValue("@Title", project.Title);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrWhiteSpace(project.Description) ? DBNull.Value : project.Description);
            cmd.Parameters.AddWithValue("@StartDate", (object?)project.StartDate ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Deadline", (object?)project.Deadline ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", string.IsNullOrWhiteSpace(project.Status) ? DBNull.Value : project.Status);

            cmd.ExecuteNonQuery();
        }

        // DELETING MY PROJECT
        public void DeleteProject(int id)
        {
            using SqlConnection connection = new(_connectionString);
            connection.Open();

            string query = "DELETE FROM Projects WHERE ProjectId = @Id";

            using SqlCommand cmd = new(query, connection);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
