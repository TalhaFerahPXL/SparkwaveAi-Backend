using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Mail
{
    static class MailServer
    {


        const string mailServer = "live.smtp.mailtrap.io";
        const int portNumber = 587;
        const string userName = "";
        const string password = "";

        public static string MailFrom => "info@demomailtrap.com";

        public static SmtpClient GetSmtpClient()
        {
            var smtpClient = new SmtpClient(mailServer, portNumber)
            {
                Credentials = new System.Net.NetworkCredential()
                {
                    UserName = userName,
                    Password = password
                },
                EnableSsl = true
            };
            return smtpClient;
        }

    }
}
