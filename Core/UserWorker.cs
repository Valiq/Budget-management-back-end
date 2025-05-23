﻿using Budget_management_back_end.Models;
using Dapper;
using MySqlConnector;
using Org.BouncyCastle.Asn1.Ocsp;
using static Budget_management_back_end.Records.Records;
using static BCrypt.Net.BCrypt;

namespace Budget_management_back_end.Core
{
    internal class UserWorker : AuthorizationWorker
    {
        internal UserWorker(IConfiguration configuration) : base(configuration) { }

        private long GetUser(MySqlConnection connection, string email)
        {
            string sql = @"SELECT id FROM User WHERE Email = @Email";

            var result = connection.Query<long>(sql, new { Email = email}).ToList();

            if (result.Count > 0)
                return result[0];
            else
                return -1;
        }

        internal long AddUser(UserRequest request)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    long id = GetUser(connection, request.email);

                    if (id == -1)
                    {
                        var hashedPassword = HashPassword(request.password);

                        User user = new User()
                        {
                            Name = request.name,
                            Email = request.email,
                            Password = hashedPassword,
                            CreateDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                        };

                        string sql = @"INSERT INTO User (Name, Email, Password, Create_Date) VALUES(@Name, @Email, @Password, @CreateDate);
                                       SELECT LAST_INSERT_ID();";

                        id = connection.QuerySingle<long>(sql, user);
                    }

                    return id;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }
        }

        internal string LoginUser(Login request)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    string sql = @"SELECT * FROM User WHERE Email = @Email";

                    User user = connection.QueryFirst<User>(sql, new { Email = request.email });

                    if (!Verify(request.password, user.Password))
                        return string.Empty;

                    sql = @"INSERT INTO User_Audit (User_Name, User_Email, Login_Date) VALUES (@UserName, @UserEmail, @LoginDate)";

                    UserAudit audit = new UserAudit() 
                    { 
                        UserName = user.Name,
                        UserEmail = user.Email,
                        LoginDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                    };

                    connection.Execute(sql, audit);

                    sql = @"UPDATE User SET Token = @Token WHERE Id = @Id";

                    string token = Guid.NewGuid().ToString();

                    connection.Execute(sql, new { Token = HashPassword(token), user.Id });

                    return token;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
        }

        internal UserResponse GetUserByEmail(string email)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    string sql = @"SELECT id, name, email FROM User WHERE Email = @Email";

                    var result = connection.QueryFirst<UserResponse>(sql, new { Email = email});

                    return result;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal UserResponse GetUserByToken(string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    var users = connection.Query<User>("SELECT id, name, email, token FROM User");

                    foreach (var user in users)
                    {
                        if (Verify(token, user.Token))
                        {
                            return new UserResponse(user.Id, user.Name, user.Email);
                        }
                    }

                    return null;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        internal bool DeleteUserById(long id, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (CheckToken(connection, token, id))
                    {
                        string sql = @"DEETE FROM User WHERE Id = @Id";

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

        internal string UpdateUserEmail(long id, string email, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (CheckToken(connection, token, id))
                    {
                        string sql = @"UPDATE User SET Email = @Email, Token = @Token WHERE Id = @Id";

                        string newToken = Guid.NewGuid().ToString();

                        connection.Execute(sql, new { Email = email, Token = HashPassword(newToken), Id = id });

                        return newToken;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
        }

        internal string UpdateUserPassword(long id, UpdatePasswordRequest request, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (CheckToken(connection, token, id))
                    {
                        string sql = @"SELECT Password FROM User WHERE Id = @Id";

                        string currentPassword = connection.QueryFirst<string>(sql, new { Id = id });

                        if (Verify(request.currentPassword, currentPassword)) 
                        {
                            sql = @"UPDATE User SET Password = @Password, Token = @Token WHERE Id = @Id";

                            string newToken = Guid.NewGuid().ToString();

                            connection.Execute(sql, new { Password = HashPassword(request.newPassword), Token = HashPassword(newToken), Id = id });

                            return newToken;
                        }
                    }

                    return string.Empty;
                }
                catch (Exception ex)
                {
                    return string.Empty;
                }
            }
        }

        internal bool UpdateUser(User user, string token)
        {
            using (MySqlConnection connection = new MySqlConnection(Configuration.GetValue<string>("ConnectionString")))
            {
                try
                {
                    connection.Open();

                    if (CheckToken(connection, token, user.Id))
                    {
                        var updateFields = new List<string>();
                        var parameters = new Dictionary<string, object>();

                        if (!string.IsNullOrEmpty(user.Name))
                        {
                            updateFields.Add("Name = @Name");
                            parameters.Add("@Name", user.Name);
                        }

                        if (!updateFields.Any())
                            return true; // Нет полей для обновления

                        string query = $@"UPDATE User SET {string.Join(", ", updateFields)} WHERE Id = @Id";

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            // Добавляем все параметры в команду
                            foreach (var param in parameters)
                            {
                                command.Parameters.AddWithValue(param.Key, param.Value);
                            }
                            command.Parameters.AddWithValue("@Id", user.Id);

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
    }
}
