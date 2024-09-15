using ClassLibrary1.Business;
using ClassLibrary1.Business.Entities;
using ClassLibrary1.Data.Framework;
using ClassLibrary1.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClassLibrary1.Data.Repositories
{
    internal class AccountsData : SqlServer
    {

        private const string tableName = "Users";
        public AccountsData() : base(tableName)
        {

        }

        public SelectResult SelectCheckEmail(string email)
        {

            string query = $"select isGoogleUser from {tableName} where email = @Mail";

            using (SqlCommand command = new SqlCommand(query))
            {
                command.Parameters.Add("@Mail", SqlDbType.VarChar).Value = email;
                return base.SelectOnlyOne(command);
            }


        }







        public SelectResult CheckLogin(string email, string password)
        {
            var result = new SelectResult();
            string query = $"select name, password, isGoogleUser, EmailVerified from {tableName} where email = @Email;";
            using (SqlCommand selectCommand = new SqlCommand(query))
            {
                selectCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = email;
                

                result = base.SelectOnlyOne(selectCommand);

                if (result.Rows == 1)
                {
                    string hashedPasswordFromDatabase = result.DataTable.Rows[0]["password"].ToString();
                    bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, hashedPasswordFromDatabase);

                    if (passwordMatch)
                    {
                        result.Succeeded = true;
                    }
                    else
                    {
                        result.AddError("Invalid password");
                        result.Succeeded= false;
                    }
                }
                else
                {
                    result.AddError("User not found");
                     result.Succeeded= false;
                }
            }

            return result;
        }



        public InsertResult insertRegistration(Account account)
        {

            if (account.googleLogin == true)
            {
                StringBuilder insertQuery = new StringBuilder();

                insertQuery.Append($"Insert INTO {tableName} ");
                insertQuery.Append($"(email,name,password,isGoogleUser) Values");
                insertQuery.Append($"(@Email,@Name,@Password,@Google); ");

                using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
                {
                    insertCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = account.email;
                    insertCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = account.name;


                    insertCommand.Parameters.AddWithValue("@Password", DBNull.Value); 
                    insertCommand.Parameters.Add("@Google", SqlDbType.Bit).Value = account.googleLogin;

                    return base.Insert(insertCommand);
                }

            }
            else
            {
            
            StringBuilder insertQuery = new StringBuilder();

            insertQuery.Append($"Insert INTO {tableName} ");
            insertQuery.Append($"(email,name,password) Values");
            insertQuery.Append($"(@Email,@Name,@Password); ");

            using (SqlCommand insertCommand = new SqlCommand(insertQuery.ToString()))
            {
                insertCommand.Parameters.Add("@Email", SqlDbType.VarChar).Value = account.email;
                insertCommand.Parameters.Add("@Name", SqlDbType.VarChar).Value = account.name;

              
                insertCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = account.password;

                return base.Insert(insertCommand);
            }
            }
        }





        public InsertResult updatePassword(string email, string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            string updateQuery = "UPDATE user_login SET password = @Password WHERE email = @Email";

            using (SqlCommand command = new SqlCommand(updateQuery))
            {
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@Email", email);

            return base.Insert(command);
            }
        }

        public InsertResult InsertEmailConfirmationToken(string token, string email)
        {
            string commandText = @"
        UPDATE Users
        SET ConfirmationToken = @Token
        WHERE email = @Email;";

            using (SqlCommand updateCommand = new SqlCommand(commandText))
            {
                updateCommand.Parameters.AddWithValue("@Token", token);
                updateCommand.Parameters.AddWithValue("@Email", email);

                return base.Insert(updateCommand);
            }
        }

        public SelectResult SelectEmailConfirmationToken(string token)
        {
            SelectResult selectResult = new SelectResult();
            var query = $"SELECT ConfirmationToken FROM {tableName} WHERE ConfirmationToken = @token";

            using (SqlCommand selectCommand = new SqlCommand(query))
            {
                selectCommand.Parameters.Add("@token", SqlDbType.VarChar).Value = token;

                selectResult = base.SelectOnlyOne(selectCommand);
            }

            return selectResult;
        }


        public InsertResult SetEmailVerifiedToTrue(string token)
        {
            string updateQuery = "UPDATE Users SET EmailVerified = 1 WHERE ConfirmationToken = @Token";

            using (SqlCommand command = new SqlCommand(updateQuery))
            {
                command.Parameters.AddWithValue("@Token", token);

                return base.Insert(command);  
            }
        }


    }
}
