
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ClassLibrary1.Data.Framework
{
    public static class Settings
    {
        public static string GetConnectionString()
        {

            return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Brisk;Integrated Security=True;Connect Timeout=30;Encrypt=False;";
        }


    } 
}
