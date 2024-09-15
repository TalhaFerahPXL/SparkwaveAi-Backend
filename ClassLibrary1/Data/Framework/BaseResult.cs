using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Data.Framework
{
    public abstract class BaseResult
    {
        public int Rows { get; set; }

        private List<string> errors = new List<string>();
        public DataTable DataTable { get; set; }
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors => errors;
        public void AddError(string error)
        {
            errors.Add(error);
        }
    }
}
