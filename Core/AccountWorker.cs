using Budget_management_back_end.Models;
using MySqlConnector;

namespace Budget_management_back_end.Core
{
    internal class AccountWorker
    {
        private readonly IConfiguration Configuration;

        internal AccountWorker(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal Account AddAccount(Account account)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new Account();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool AddUserAccount(long id, string email)
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

        internal Account GetAccount(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new Account();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal List<User> GetUserAccount(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new List<User>();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal List<Role> GetRole()
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new List<Role>();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool UpdateAccount(Account account)
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

        internal bool DeleteAccount(long id)
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

        internal bool DeleteUserAccountByEmail(long id, string email)
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
    }
}
