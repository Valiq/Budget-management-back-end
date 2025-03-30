using Budget_management_back_end.Models;
using MySqlConnector;

namespace Budget_management_back_end.Core
{
    internal class FinanceEntityWorker
    {
        private readonly IConfiguration Configuration;

        internal FinanceEntityWorker(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        internal FinanceEntity AddFinanceEntity(long accountId, FinanceEntity entity)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new FinanceEntity();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal List<FinanceEntity> GetFinanceEntityByAccountId(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {


                    return new List<FinanceEntity>();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool UpdateFinanceEntity(long id)
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

        internal bool DeleteFinanceEntity(long id)
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
