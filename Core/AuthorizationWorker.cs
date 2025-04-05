using Budget_management_back_end.Models;
using Dapper;
using MySqlConnector;
using static BCrypt.Net.BCrypt;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Core
{
    public class AuthorizationWorker
    {
        protected readonly IConfiguration Configuration;

        internal AuthorizationWorker(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected UserResponse GetUser(MySqlConnection connection, string token)
        {
            try
            {
                var users = connection.Query<User>("SELECT id, name, email, token FROM User");

                foreach (var user in users)
                {
                    if (Verify(token, user.Token))
                    {
                        return new UserResponse(user.Id, user.Name, user.Email);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected bool CheckToken(MySqlConnection connection, string token, long userId)
        {
            try
            {
                string sql = @"SELECT * FROM User WHERE Id = @Id";

                User user = connection.QueryFirst<User>(sql, new { Id = userId });

                if (Verify(token, user.Token))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
