using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Business.Entities
{
    public class Account
    {
    //public int? userId { get; set; }
    public string email { get; set; }
    public string name { get; set; }
        public string password { get; set; }

        public bool? googleLogin { get; set; }


        public Account(string email, string name, string password, bool google)
        {
            this.email = email;
            this.name = name;
            this.password = password;
            this.googleLogin = google;
            
        }


    }
}
