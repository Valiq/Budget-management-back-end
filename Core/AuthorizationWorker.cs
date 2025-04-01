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
                connection.Open();

                string sql = @"SELECT id, email, name FROM User WHERE Token = @Token";

                var result = connection.Query<UserResponse>(sql, new { Token = HashPassword(token) }).ToList();

                if (result.Count() > 0)
                    return result[0];
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal bool CheckToken(MySqlConnection connection, string token, long userId)
        {
            try
            {
                connection.Open();

                string sql = @"SELECT Id FROM User WHERE Token = @Token";

                var result = connection.Query<long>(sql, new { Token = HashPassword(token) }).ToList();

                if (result.Count() > 0 && result[0] == userId)
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
