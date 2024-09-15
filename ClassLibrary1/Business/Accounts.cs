using ClassLibrary1.Data.Framework;
using ClassLibrary1.Data.Repositories;
using ClassLibrary1.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1.Mail;
using System.Security.Cryptography;

namespace ClassLibrary1.Business
{
    public static class Accounts
    {
        public static SelectResult CheckIfEmailExist(string mail)
        {

            AccountsData accountsData = new AccountsData();

            SelectResult result = accountsData.SelectCheckEmail(mail);

            return result;

        }


        public static InsertResult AddAccounts(string email, string name, string password, bool? google)
        {


            if (google == true)
            {
                Account account = new Account(email, name, null, true);

                AccountsData accountData = new AccountsData();

                account.email = email;
                account.name = name;
                account.googleLogin = true;


                InsertResult result = accountData.insertRegistration(account);
                return result;

            }
            else
            {
                Account account = new Account(email, name, password, false);


                AccountsData accountData = new AccountsData();

                account.email = email;
                account.name = name;


                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
                account.password = hashedPassword;

                InsertResult result = accountData.insertRegistration(account);
                return result;
            }


        }






        public static SelectResult CheckLoginCredentials(string email, string password)
        {
            AccountsData accountsData = new AccountsData();
            SelectResult result = accountsData.CheckLogin(email, password);
            return result;
        }



        public static InsertResult changePassword(string email, string password)
        {
            AccountsData accountsData = new AccountsData();
            InsertResult result = accountsData.updatePassword(email, password);
            return result;

        }


        public static InsertResult InsertEmailConfirmationToken(string token, string email)
        {
            AccountsData accountsData = new AccountsData();
            InsertResult result = accountsData.InsertEmailConfirmationToken(token, email);
            return result;
        }


        public static SelectResult SelectEmailConfirmationToken(string token)
        {
            AccountsData accountsData = new AccountsData();
            SelectResult result = accountsData.SelectEmailConfirmationToken(token);
            return result;
        }


        public static InsertResult UpdateEmailVerifiedStatus(string token)
        {
            AccountsData accountData = new AccountsData();
            InsertResult result = accountData.SetEmailVerifiedToTrue(token);
            return result;
        }

    }
}
