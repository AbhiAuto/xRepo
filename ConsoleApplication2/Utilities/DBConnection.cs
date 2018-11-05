using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvidxBDDFramework.Utilities
{
    class DBConnection
    {
        public static string retVal;
        public static string sqlConnect(string queryString)
        {            
            //sql query string
            //string queryString = "SELECT * FROM [AvidPayTransaction].[static].[tPaymentProcessingStatusType] where PaymentProcessingStatusTypeID in (14,27);";
            //initiating Sql connection
            SqlConnection con = new SqlConnection("Server=azrfswdvs02; Integrated Security=True;");
            con.Open();
            //Executing the query string on the connected sql server
            SqlCommand command = new SqlCommand(queryString, con);
            //Reading the response after the execution
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while (reader.Read())
                {
                    retVal=reader[0].ToString();
                }
            }
            finally
            {
                reader.Close();
            }
            con.Close();
            return retVal;
        }
    }
}
