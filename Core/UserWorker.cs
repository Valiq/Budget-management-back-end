using Budget_management_back_end.Models;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Budget_management_back_end.Core
{
    internal class UserWorker
    {
        private readonly IConfiguration Configuration;

        internal UserWorker(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal User AddUser(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    

                    return new User();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal string LoginUser(string email, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
        }

        internal User GetUserByEmail(string email)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    

                    return new User();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal User GetUserByToken(string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new User();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool DeleteUserById(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        internal bool UpdateUserEmail(long id, string email)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        internal bool UpdateUserPassword(long id, string password)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        internal bool UpdateUser(User user)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    var updateFields = new List<string>();
                    var parameters = new Dictionary<string, object>();

                    if (!string.IsNullOrEmpty(user.Name))
                    {
                        updateFields.Add("Name = @Name");
                        parameters.Add("@Name", user.Name);                  
                    }

                    if (!updateFields.Any())
                        return true; // Нет полей для обновления

                    string query = $"UPDATE Users SET {string.Join(", ", updateFields)} WHERE Id = @Id";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Добавляем все параметры в команду
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                        command.Parameters.AddWithValue("@Id", user.Id);

                        int rowsAffected = command.ExecuteNonQuery();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
