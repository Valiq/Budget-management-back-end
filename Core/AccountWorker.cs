using Budget_management_back_end.Models;
using Dapper;
using MySqlConnector;
using System.Security.Principal;
using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Core
{
    internal class AccountWorker: AuthorizationWorker
    {
        internal AccountWorker(IConfiguration configuration):base(configuration){}

        private long GetAccountAdmin(MySqlConnection connection, long accountId)
        {
            string sql = @"SELECT User_Id FROM User_Account WHERE Account_Id = @Id 
                            AND Role_Id = (SELECT Id FROM Role WHERE Code = 'Admin')";

            return connection.QueryFirst<long>(sql, new { Id = accountId });
        }

        internal long AddAccount(long userId, AccountRequest request, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (CheckToken(connection, token, userId))
                    {
                        Account account = new Account()
                        {
                            Name = request.name,
                            Description = request.description,
                            CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };

                        string sql = @"INSERT INTO Account (Name, Description, Create_Date) VALUES (@Name)
                                       SELECT LAST_INSERT_ID();";

                        long id = connection.QuerySingle<long>(sql, account);

                        sql = @"SELECT Id FROM Role WHERE Code = Admin";

                        long roleId = connection.QueryFirst<long>(sql);

                        sql = @"INSERT INTO User_Account (User_Id, Account_Id, Role_Id) VALUES (@UserId, @AccountId, @RoleId)";

                        connection.Execute(sql, new { UserId = userId, AccountId = id, RoleId = roleId});

                        return id;
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

        internal bool AddUserAccount(long id, string email, string role, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long userId = GetAccountAdmin(connection, id);

                    if (CheckToken(connection, token, userId))
                    {
                        string sql = @"SELECT Id FROM Role WHERE Code = @Role";

                        long roleId = connection.QueryFirst<long>(sql);

                        sql = @"SELECT Id FROM User WHERE Email = @Email";

                        userId = connection.QueryFirst<long>(sql, new { Email = email });

                        sql = @"INSERT INTO User_Account (User_Id, Account_Id, Role_Id) VALUES (@UserId, @AccountId, @RoleId)";

                        connection.Execute(sql, new { UserId = userId, AccountId = id, RoleId = roleId});

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

        internal Account GetAccount(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    string sql = @"SELECT Id, Name, Description, Create_Date FROM Account WHERE Id = @id";

                    return connection.QueryFirst<Account>(sql, new { Id = id});
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal List<Account> GetUserAccount(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    string sql = @"SELECT Id, Name, Description, Create_Date as CreateDate FROM Account WHERE Id IN 
                                   (SELECT Account_Id FROM User_Account WHERE User_Id = @Id)";

                    return connection.Query<Account>(sql, new { Id = id }).ToList();
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
                    connection.Open();

                    string sql = @"SELECT Id, Name, Code FROM Role";

                    return connection.Query<Role>(sql).ToList();
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool UpdateAccount(Account account, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long userId = GetAccountAdmin(connection, account.Id);

                    if (CheckToken(connection, token, userId))
                    {
                        var updateFields = new List<string>();
                        var parameters = new Dictionary<string, object>();

                        if (!string.IsNullOrEmpty(account.Name))
                        {
                            updateFields.Add("Name = @Name");
                            parameters.Add("@Name", account.Name);
                        }

                        if (!string.IsNullOrEmpty(account.Description))
                        {
                            updateFields.Add("Description = @Description");
                            parameters.Add("@Description", account.Description);
                        }

                        if (!updateFields.Any())
                            return true; // Нет полей для обновления

                        string query = $@"UPDATE Account SET {string.Join(", ", updateFields)} WHERE Id = @Id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Добавляем все параметры в команду
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                            command.Parameters.AddWithValue("@Id", account.Id);

                            int rowsAffected = command.ExecuteNonQuery();
                        }

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

        internal bool DeleteAccount(long id, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long userId = GetAccountAdmin(connection, id);

                    if (CheckToken(connection, token, userId))
                    {
                        string sql = @"DELETE FROM Account WHERE Id = @Id";

                        connection.Execute(sql, new { Id = id});

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

        internal bool DeleteUserAccountByEmail(long id, string email, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long userId = GetAccountAdmin(connection, id);

                    if (CheckToken(connection, token, userId))
                    {
                        string sql = @"SELECT Id FROM User WHERE Email = @Email";

                        userId = connection.QueryFirst<long>(sql, new { Email = email });

                        sql = @"DELETE FROM User_Account WHERE User_Id = @UserId";

                        connection.Execute(sql, new { UserId = userId });

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
