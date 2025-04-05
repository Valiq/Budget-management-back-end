using Budget_management_back_end.Models;
using Dapper;
using MySqlConnector;
using System.Transactions;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Core
{
    public class TransactionWorker : AuthorizationWorker
    {
        internal TransactionWorker(IConfiguration configuration) : base(configuration) { }

        private bool HaveGrants(MySqlConnection connection, long accountId, string token, string state, out string role)
        {
            string grants = string.Empty;

            switch (state)
            {
                case "A":
                    grants = "'Admin','Editor'";
                    break;
                case "B":
                    grants = "'Admin','Editor','Viewer'";
                    break;
            }

            string sql = @$"SELECT User_Id FROM User_Account WHERE Account_Id = @Id 
                            AND Role_Id IN (SELECT Id FROM Role WHERE Code IN ({grants}))";

            var result = connection.Query<long>(sql, new { Id = accountId }).ToList();

            bool flag = false;
            role = string.Empty;

            foreach (var userId in result)
                if (CheckToken(connection, token, userId))
                {
                    flag = true;

                    sql = @"SELECT Name FROM Role WHERE Id IN 
                            (SELECT Role_Id FROM User_Account WHERE Account_Id = @AccountId AND User_Id = @UserId)";

                    role = connection.QueryFirst<string>(sql, new { AccountId = accountId, UserId = userId });

                    break;
                }

            return flag;
        }

        internal bool AddTransaction(TransactionRequest request, string token)
        {
            if (request.balanceFromId == null && request.balanceToId == null)
                return false;

            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                connection.Open();

                try
                {
                    if (HaveGrants(connection, request.accountId, token, "A", out string role))
                    {
                        string sql;

                        if (request.balanceFromId is not null)
                        {
                            sql = @"UPDATE Balance SET Sum = Sum - @Sum WHERE Id = @Id";
                            connection.Execute(sql, new { request.Sum, Id = request.balanceFromId });
                        }

                        if (request.balanceToId is not null)
                        {
                            sql = @"UPDATE Balance SET Sum = Sum + @Sum WHERE Id = @Id";
                            connection.Execute(sql, new { request.Sum, Id = request.balanceToId });
                        }

                        UserResponse user = GetUser(connection, token);

                        sql = @"INSERT INTO Transaction_Audit (Account_Id, Balance_From_Id, Balance_To_Id, Sum, Date_Time, User_Name, User_Email, User_Role) 
                                       VALUES (@AccountId, 
                                               @BalanceFromId, 
                                               @BalanceToId, 
                                               @Sum, 
                                               @DateTime, 
                                               @UserName, 
                                               @UserEmail, 
                                               @UserRole)";

                        TransactionAudit audit = new TransactionAudit()
                        {
                            AccountId = request.accountId,
                            BalanceFromId = request.balanceFromId,
                            BalanceToId = request.balanceToId,
                            Sum = request.Sum,
                            DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                            UserName = user.name,
                            UserEmail = user.email,
                            UserRole = role
                        };

                        connection.Execute(sql, audit);

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

        internal List<TransactionResponse> GetAccountTransaction(long accountId, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (HaveGrants(connection, accountId, token, "B", out string role))
                    {
                        string sql = @"SELECT 
                                        ta.Id as id,
                                        ta.Sum as sum,
                                        (SELECT Code FROM Currency where Id IN (SELECT Currency_Id from Balance where Id = ta.Balance_To_Id OR Id = ta.Balance_From_Id)) as Currency,
                                        ta.Date_Time as date,
                                        ta.User_Name as userName,
                                        ta.User_Email as userEmail,
                                        ta.User_Role as userRole,
                                        (SELECT Name FROM Finance_Entity where Id = (SELECT Finance_Entity_ID from Balance where Id = ta.Balance_From_Id)) as fromEntity,
                                        (SELECT Name FROM Finance_Entity where Id = (SELECT Finance_Entity_ID from Balance where Id = ta.Balance_To_Id)) as toEntity
                                      FROM 
                                        Transaction_Audit ta
                                      WHERE ta.Account_Id = @AccountId;";

                        var result = connection.Query<TransactionResponse>(sql, new { AccountId = accountId }).ToList();

                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }
    }
}
