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
        public static void sqlConnect()
        {
            string queryString = "SELECT * FROM [AvidPayTransaction].[static].[tPaymentProcessingStatusType] where PaymentProcessingStatusTypeID in (14,27);";
            SqlConnection con = new SqlConnection("Server=azrfswdvs02; Integrated Security=True;");
            con.Open();
            SqlCommand command = new SqlCommand(queryString, con);
            SqlDataReader reader = command.ExecuteReader();
            try
            {
                while(reader.Read())
                {
                    
                }
            }
            finally
            {
                reader.Close();
            }
            con.Close();
        }
    }
}
