using Budget_management_back_end.Models;
using MySqlConnector;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Core
{
    internal class BalanceWorker : AuthorizationWorker
    {
        internal BalanceWorker(IConfiguration configuration) : base(configuration) { }

        internal long AddBalance(long entityId, BalanceRequest request)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    Balance balance = new Balance()
                    {
                        FinanceEntityId = entityId,
                        CurrencyId = request.currencyId,
                        Sum = request.sum
                    };

                    return 0;
                }
                catch (Exception ex)
                {
                    return -1;
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
