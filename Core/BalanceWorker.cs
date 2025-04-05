using Budget_management_back_end.Models;
using Dapper;
using MySqlConnector;
using Org.BouncyCastle.Asn1.Ocsp;
using static Budget_management_back_end.Records.Records;
using static Dapper.SqlMapper;

namespace Budget_management_back_end.Core
{
    internal class BalanceWorker : AuthorizationWorker
    {
        internal BalanceWorker(IConfiguration configuration) : base(configuration) { }

        private bool HaveGrants(MySqlConnection connection, long entityId, string token)
        {
            string sql = @"SELECT User_Id FROM User_Account WHERE 
                            Account_Id IN (SELECT Account_Id FROM Finance_Entity WHERE Id = @Id)
                            AND Role_Id IN (SELECT Id FROM Role WHERE Code = 'Admin' OR Code = 'Editor')";

            var result = connection.Query<long>(sql, new { Id = entityId }).ToList();

            bool flag = false;

            foreach (var userId in result)
                if (CheckToken(connection, token, userId))
                {
                    flag = true;
                    break;
                }

            return flag;
        }

        private long GetEntityId(MySqlConnection connection, long balanceId)
        {
            string sql = @"SELECT Finance_Entity_ID FROM Balance WHERE Id = @Id";

            return connection.QueryFirst<long>(sql, new { Id = balanceId });
        }

        internal long AddBalance(long entityId, BalanceRequest request, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (HaveGrants(connection, entityId, token))
                    {
                        Balance balance = new Balance()
                        {
                            FinanceEntityId = entityId,
                            CurrencyId = request.currencyId,
                            Sum = request.sum
                        };

                        string sql = @"INSERT INTO Balance (Finance_Entity_Id, Currency_Id, Sum) VALUES (@FinanceEntityId, @CurrencyId, @Sum);
                                       SELECT LAST_INSERT_ID();";
                        
                        return connection.QuerySingle<long>(sql, balance);
                    }
                    else
                    {
                        return -1;
                    }
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
                    connection.Open();

                    string sql = "SELECT Id, Finance_Entity_Id as FinanceEntityId, Currency_Id as CurrencyId, Sum FROM Balance WHERE Finance_Entity_Id = @Id";

                    return connection.Query<Balance>(sql, new { Id = id}).ToList();
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
                    connection.Open();

                    string sql = "SELECT * FROM Currency";

                    return connection.Query<Currency>(sql).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool DeleteBalance(long id, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long entityId = GetEntityId(connection, id);

                    if (HaveGrants(connection, entityId, token))
                    {
                        string sql = "DELETE FROM Balance WHERE Id = @Id";

                        connection.Execute(sql, new { Id = id });

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                  
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
