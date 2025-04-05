using Budget_management_back_end.Models;
using Dapper;
using MySqlConnector;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Principal;
using static Budget_management_back_end.Records.Records;
using static Dapper.SqlMapper;

namespace Budget_management_back_end.Core
{
    internal class FinanceEntityWorker : AuthorizationWorker
    {
        internal FinanceEntityWorker(IConfiguration configuration) : base(configuration) { }

        private bool HaveGrants(MySqlConnection connection, long accountId, string token)
        {
            string sql = @"SELECT User_Id FROM User_Account WHERE Account_Id = @Id 
                            AND Role_Id IN (SELECT Id FROM Role WHERE Code = 'Admin' OR Code = 'Editor')";

            var result = connection.Query<long>(sql, new { Id = accountId }).ToList();

            bool flag = false;

            foreach (var userId in result)
                if (CheckToken(connection, token, userId))
                {
                    flag = true;
                    break;
                }

            return flag;
        }

        private long GetAccountId(MySqlConnection connection, long entityId)
        {
            string sql = @"SELECT Account_Id FROM Finance_Entity WHERE Id = @Id";

            return connection.QueryFirst<long>(sql, new { Id = entityId });
        }

        internal long AddFinanceEntity(long accountId, FinanceEntityRequest request, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (HaveGrants(connection, accountId, token))
                    {
                        string sql = @"INSERT INTO Finance_Entity (Account_Id, Name, Description) VALUES (@AccountId, @Name, @Description);
                                       SELECT LAST_INSERT_ID();";

                        FinanceEntity entity = new FinanceEntity()
                        {
                            AccountId = accountId,
                            Name = request.name,
                            Description = request.description
                        };

                        return connection.QuerySingle<long>(sql, entity);
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

        internal List<FinanceEntity> GetFinanceEntityByAccountId(long id)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    string sql = @"SELECT Id, Account_Id as AccountId, Name, Description FROM Finance_Entity WHERE Account_Id = @AccountId";

                    var entities = connection.Query<FinanceEntity>(sql, new { AccountId = id }).ToList();

                    sql = "SELECT Id, Finance_Entity_Id as FinanceEntityId, Currency_Id as CurrencyId, Sum FROM Balance WHERE Finance_Entity_Id = @Id";

                    for (int i = 0; i < entities.Count(); i++)
                    {
                        entities[i].Balances = connection.Query<Balance>(sql, new { entities[i].Id }).ToList();
                    }

                    return entities;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool UpdateFinanceEntity(FinanceEntity entity, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long accountId = GetAccountId(connection, entity.Id);

                    if (HaveGrants(connection, accountId, token))
                    {
                        var updateFields = new List<string>();
                        var parameters = new Dictionary<string, object>();

                        if (!string.IsNullOrEmpty(entity.Name))
                        {
                            updateFields.Add("Name = @Name");
                            parameters.Add("@Name", entity.Name);
                        }

                        if (!string.IsNullOrEmpty(entity.Description))
                        {
                            updateFields.Add("Description = @Description");
                            parameters.Add("@Description", entity.Description);
                        }

                        if (!updateFields.Any())
                            return true; // Нет полей для обновления

                        string query = $@"UPDATE Finance_Entity SET {string.Join(", ", updateFields)} WHERE Id = @Id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Добавляем все параметры в команду
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                            command.Parameters.AddWithValue("@Id", entity.Id);

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

        internal bool DeleteFinanceEntity(long id, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long accountId = GetAccountId(connection, id);

                    if (HaveGrants(connection, accountId, token))
                    {
                        string sql = @"DELETE FROM Finance_Entity WHERE Id = @Id";

                        connection.Execute(sql, new { Id = id} );

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