using Budget_management_back_end.Models;
using Dapper;
using MySqlConnector;
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
                    grants = "'Admin',Editor'";
                    break;
                case "B":
                    grants = "'Admin','Editor','Viewer'";
                    break;
            }

            string sql = @$"SELECT User_Id FROM User_Account WHERE Account_Id = @Id 
                            AND Role_Id = (SELECT Id FROM Role WHERE Code IN ({grants}))";

            var result = connection.Query<long>(sql, new { Id = accountId }).ToList();

            bool flag = false;
            role = string.Empty;

            foreach (var userId in result)
                if (CheckToken(connection, token, userId))
                {
                    flag = true;

                    sql = @"SELECT Name FROM Role WHERE Id IN 
                            (SELECT Role_Id FROM User_Account WHERE Account_Id = @AccountId AND User_Id = @UserId)";

                    role = connection.QueryFirst<string>(sql, new { AccountId = accountId, UserId = userId});

                    break;
                }

            return flag;
        }

        internal bool AddTransaction(TransactionRequest request, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (HaveGrants(connection, request.accountId, token, "A", out string role))
                    {
                        UserResponse user = GetUser(connection, token);

                        string sql = @"INSERT INTO Transaction (Account_Id, Balance_From_Id, Balance_To_Id, Sum, Date_Time, User_Name, User_Email, User_Role) 
                                       VALUES (Account_Id = @AccountId, 
                                               Balance_From_Id = @BalanceFromId, 
                                               Balance_To_Id = @BalanceToId, 
                                               Sum = @Sum, 
                                               Date_Time = @DateTime, 
                                               User_Name = @UserName, 
                                               User_Email = @UserEmail, 
                                               User_Role = @UserRole)";

                        TransactionAudit transaction = new TransactionAudit() 
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

                        connection.Execute(sql, transaction);

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

        internal List<TransactionAudit> GetAccountTransaction(long accountId, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (HaveGrants(connection, accountId, token, "B", out string role))
                    {
                        string sql = @"SELECT * FROM Transaction_Audit WHERE Account_Id = @AccountId";

                        var result = connection.Query<TransactionAudit>(sql, new { AccountId = accountId }).ToList();

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
