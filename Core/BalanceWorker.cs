using Budget_management_back_end.Models;
using MySqlConnector;

namespace Budget_management_back_end.Core
{
    internal class BalanceWorker
    {
        private readonly IConfiguration Configuration;

        internal BalanceWorker(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal Balance AddBalance(long entityId, Balance balance)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new Balance();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal List<Balance> GetBalanceByFinanceEntityId(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new List<Balance>();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal List<Currency> GetCurrency()
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new List<Currency>();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool UpdateBalance(long id)
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

        internal bool DeleteBalance(long id)
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
