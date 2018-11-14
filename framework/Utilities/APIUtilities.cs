using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using TechTalk.SpecFlow;

namespace AvidxBDDFramework.Utilities
{
    class APIUtilities
    {
        public static string strAPIUrl;
        public static string reqString;
        public static JArray jsonResArray;
        public static JObject jsonResObject;
        public static string strGetReqParms;
        public static string dirPath = WebUtilities.getDirPath();
        public static HttpStatusCode statuscode;
        public static int statusCode;
        public static HttpWebResponse response;
        public static Stream dataStream;

        //internal static string getRequestURL(string strParam)
        //{
        //    //fetching the value of Api url from the testdata.config configuration file and setting into a variable
        //    strAPIUrl = ConfigurationManager.AppSettings[strParam];
        //    Console.WriteLine("API Url is :" + strAPIUrl);
        //    return strAPIUrl;
        //}

        internal static void SetReqParams(string strParam)
        {

            if (strGetReqParms == null)
            {
                strGetReqParms = strParam;
            }
            else
            {
                strGetReqParms = "," + strParam;
            }
        }

        public class DataObject
        {
            public string Name { get; set; }
        }
        
        //This is for sending a post request
        private static void post(string jsonData)
        {
            try
            {
                // Create a request using a URL that can receive a post.   
                HttpWebRequest request =(HttpWebRequest)WebRequest.Create(strAPIUrl);
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Create POST data and convert it to a byte array.  
                byte[] byteArray = Encoding.UTF8.GetBytes(jsonData);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/json";
                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;

                string username = WebUtilities.fetchParamValFromConfig("username");
                string password = WebUtilities.fetchParamValFromConfig("password");
                string dePassword = WebUtilities.decryptString(password);
                CredentialCache credcache = new CredentialCache();
                credcache.Add(new Uri(strAPIUrl), "NTLM", new NetworkCredential(username, dePassword));
                request.Credentials = credcache;

                // Get the request stream.  
                dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();
                // Get the response.  
                response = (HttpWebResponse)request.GetResponse();
                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                statuscode = ((HttpWebResponse)response).StatusCode;
                statusCode = (int)statuscode;

                // Get the stream containing content returned by the server.  
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
                // Clean up the streams.  
                reader.Close();
                dataStream.Close();
                response.Close();

                jsonResArray = JArray.Parse(responseFromServer);
            }
            catch(WebException webEx)
            {
                try
                {
                    if (webEx.Response != null)
                    {
                        response = (HttpWebResponse)webEx.Response;
                        statuscode = ((HttpWebResponse)response).StatusCode;
                        statusCode = (int)statuscode;

                        // Get the stream containing content returned by the server.  
                        dataStream = response.GetResponseStream();
                        // Open the stream using a StreamReader for easy access.  
                        StreamReader reader = new StreamReader(dataStream);
                        // Read the content.  
                        string responseFromServer = reader.ReadToEnd();
                        // Display the content.  
                        Console.WriteLine(responseFromServer);
                        // Clean up the streams.  
                        reader.Close();
                        dataStream.Close();
                        response.Close();

                        if (statusCode == 200 || statusCode == 400)
                        {
                            jsonResArray = JArray.Parse(responseFromServer);
                        }
                        else
                        {
                            jsonResObject = JObject.Parse(responseFromServer);
                        }
                    }
                }catch(Exception e)
                {
                    Assert.Fail("Failed to send the Post request" + e);
                }  
            }

        }

        internal static void fetchValFromJsonResp(string jsonKey, string exkeyVal)
        {
            try
            {
                //fetching value from the response if the key is in Json Array
                if (jsonKey.Contains("["))
                {
                    string[] splitjsonKey = jsonKey.Split('.');
                    string jsonArrName = splitjsonKey[0];
                    jsonArrName = jsonArrName.Replace("[0]",""); 
                    string jsonKeyName = splitjsonKey[1];
                    JToken jToken = jsonResArray.Children()[jsonArrName].First();
                    JArray jArray = JArray.Parse(jToken.ToString());
                    foreach (JObject obj in jArray.Children<JObject>())
                    {
                        foreach (JProperty prop in obj.Properties())
                        {
                            string key = prop.Name;
                            if (key.Equals(jsonKeyName))
                            {
                                string actkeyValue = prop.Value.ToString();
                                if (!actkeyValue.Equals(exkeyVal))
                                {
                                    Assert.Fail("Expected: " + exkeyVal + ", actual is : " + actkeyValue);
                                }
                                else
                                {
                                    Console.WriteLine("Succesfully fetched the value from reponse");
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Assert.Fail("Failed to fetch the value from response: " + e);
            }
        }

        //To create json request specific to AvivdXchange Api
        internal static void createJsonRequest(string paymentNo, string amount)
        {
            string sqlQueryString = "SELECT PaymentID FROM [AvidPayTransaction].[trn].[tPayment]  where ONSTnumber in ("+ paymentNo + ");";
            string paymentID = DBConnection.sqlConnect(sqlQueryString);

            sqlQueryString = "SELECT ppi.PaymentProcessingStatusTypeID from AvidPayTransaction.trn.tPayment p"
                        + " inner join AvidPayTransaction.trn.tPaymentProcessingItem ppi on p.PaymentID = ppi.PaymentID"
                        + " inner join AvidPayTransaction.pfl.tUser u on u.UserID = p.UserID"
                        + " inner join AvidPayTransaction.pfl.tOrganization o on o.OrganizationID = u.OrganizationID"
                        + " where p.PaymentID in (" + paymentID + ");";
            string val = DBConnection.sqlConnect(sqlQueryString);
            if (val.Equals("14"))
            {
                sqlQueryString = "update [AvidPayTransaction].[trn].tPaymentProcessingItem set PaymentProcessingStatusTypeID = 27 where PaymentID in ("+ paymentID + ");";
            }

            reqString = "{\"" + paymentNo + "\":" + amount + "}";
        }

        //To call the post request
        internal static void sendPostRequest()
        {
            post(reqString);
        }

        //To fetch the customer details from Db
        internal static void fetchsetCustPayDt()
        {
            reqString = GenerateTxtFile.Fetchdetailsfromdb();
        }

        public static void fetchValFromResAndVal(Table strTableData)
        {
            string strActRespVal = (string)jsonResArray[0][1];
            //Assert.Fail("The expected value is : " + strRowVal + " The actual is : " + strActRespVal);
            
        }

        public static void setApiUrl(string apiName)
        {
            strAPIUrl = ConfigurationManager.AppSettings[apiName];
            if(strAPIUrl!=null)
            {
                Console.WriteLine("Successfully fetched the API Url");
                Console.WriteLine("API Url is : " + strAPIUrl);
            }
            else
            {
                Assert.Fail("Failed to fetch the Url");
            }
        }
    }
}
