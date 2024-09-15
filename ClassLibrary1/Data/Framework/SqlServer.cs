using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1.Data.Framework
{
     abstract class SqlServer
    {
        public string TableName { get; set; }

       

        SqlConnection connection;
        SqlDataAdapter adapter;

        protected BaseResult BaseResult { get; set; }
        public SqlServer(string tableName)
        {
            TableName = tableName;
            connection = new SqlConnection(Settings.GetConnectionString());
        }



        public SelectResult SelectOnlyOne(SqlCommand selectCommand)
        {
           
            DataTable dataTable = new DataTable();

            var result = new SelectResult();
            try
            {
                using (var conn = connection)
                {
                    conn.Open();
                    selectCommand.Connection = conn;

                    
                    using (var adapter = new SqlDataAdapter(selectCommand))
                    {
                       
                        adapter.Fill(0, 1, dataTable);

                        
                        if (dataTable.Rows.Count > 0)
                        {
                            result.DataTable = dataTable;
                            result.Rows = dataTable.Rows.Count;
                            result.Succeeded = true;
                        }
                        else
                        {
                            result.AddError("Geen gegevens gevonden.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               
                result.AddError("Error: " + ex.Message + " Stack Trace: " + ex.StackTrace);
            }
            return result;
        }





        public InsertResult Insert(SqlCommand insertCommand)
        {
            DataTable dataTable = new DataTable();
            InsertResult result = new InsertResult();
            try
            {
                using (var con = connection)
                {
                    con.Open();
                    insertCommand.Connection = con;


                    int rowsAffected = insertCommand.ExecuteNonQuery();
                    if (rowsAffected != 0)
                    {
                        result.Succeeded = true;
                    }
                    else
                    {
                        result.AddError("werkt niet");
                    }

                }
            }
            catch (Exception ex)
            {
                result.AddError(ex.Message);
            }
            return result;
        }





    }
}
