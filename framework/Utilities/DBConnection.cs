using NUnit.Framework;
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
        public static string sqlQuery;
        public static SqlConnection con;

        //Connecting to db and fecthing the value from db for the sql query
        public static string sqlConnect(string queryString)
        {            
            //sql query string
            //string queryString = "SELECT * FROM [AvidPayTransaction].[static].[tPaymentProcessingStatusType] where PaymentProcessingStatusTypeID in (14,27);";
            //initiating Sql connection
            con = new SqlConnection("Server=azrfswdvs02; Integrated Security=True;");
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

        //To open the db Connection
        internal static void openDbCon()
        {
            try
            {
                //initiating Sql connection
                con = new SqlConnection("Server=azrfswdvs02; Integrated Security=True;");
                con.Open();
                Console.WriteLine("Database connection is open now");
            }catch(Exception e)
            {
                Assert.Fail("Database is not successfully connected" + e);
            }
        }

        internal static void genQuery(string strQuery)
        {
            if(strQuery.Equals("PaymentProcessingStatusTypeID"))
            {
                string paymentID = GenerateTxtFile.paymentID;
                sqlQuery = "SELECT ppi.PaymentProcessingStatusTypeID from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + paymentID + ");";
            }
        }

        //Fetching the value from the Db connection
        internal static void fetchStatus(string param)
        {
            try
            {
                //Executing the query string on the connected sql server
                SqlCommand command = new SqlCommand(sqlQuery, con);
                //Reading the response after the execution
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        retVal = reader[0].ToString();
                    }
                }
                finally
                {
                    reader.Close();
                }
                con.Close();
                if (retVal.Equals(param))
                {
                    Console.WriteLine("Status is :" + retVal);
                }
                else
                {
                    Assert.Fail("Status expected is :" + param + ", actual value in database is :" + retVal);
                }
            }catch(Exception e)
            {
                Assert.Fail("Failed to fetch the details from the Database : "+e);
            }
        }

        //updating the status of the payment id
        internal static void updateDb()
        {
            try
            {
                sqlQuery = "update [AvidPayTransaction].[trn].tPaymentProcessingItem set PaymentProcessingStatusTypeID = 14 where PaymentID in (704317);";
                sqlConnect(sqlQuery);
            }
            catch(Exception e)
            {
                Assert.Fail("Failed to update the database :"+e);
            }
        }
    }
}
