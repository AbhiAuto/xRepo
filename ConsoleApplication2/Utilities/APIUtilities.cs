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
        public static string strGetReqParms;
        public static string dirPath = WebUtilities.getDirPath();
        public static HttpResponseMessage strResponse;
        private static string strToken;
        public static string strReponseCont;

        internal static string getRequestURL(string strParam)
        {
            strAPIUrl = ConfigurationManager.AppSettings[strParam];
            Console.WriteLine("API Url is :" + strAPIUrl);
            return strAPIUrl;
        }

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
        internal static void getReqRes(string strParam)
        {
            string strURL = strAPIUrl;
            strGetReqParms = "?" + strGetReqParms;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(strURL);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));
            strToken = getAuthToken();
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + strToken);

            // List data response.
            strResponse = client.GetAsync(strGetReqParms).Result;
            //Dispose once all HttpClient calls are complete. This is not necessary if the containing object will be disposed of; for example in this case the HttpClient instance will be disposed automatically when the application terminates so the following call is superfluous.
            client.Dispose();
            string strStatusCode = strResponse.StatusCode.ToString();
            strReponseCont = strResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            if (strStatusCode != "OK")
            {
                Assert.Fail("Find the reponse from the API as below" + strStatusCode + strReponseCont);
            }

        }

        private static string getAuthToken()
        {
            // Create a request using a URL that can receive a post.   
            WebRequest request = WebRequest.Create("URL");
            // Set the Method property of the request to POST.  
            request.Method = "POST";
            // Create POST data and convert it to a byte array.  
            string postData = "data";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            // Set the ContentType property of the WebRequest.  
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.  
            request.ContentLength = byteArray.Length;
            // Get the request stream.  
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();
            // Get the response.  
            WebResponse response = request.GetResponse();
            // Display the status.  
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
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

            JObject jsonObj = JObject.Parse(responseFromServer);
            string strToken = (string)jsonObj["access_token"];
            return strToken;
        }
        public static void fetchValFromResAndVal(Table strTableData)
        {
            JArray jsonArr = JArray.Parse(strReponseCont);

            TableRows tblRows = strTableData.Rows;
            List<TableRow> tblCells = tblRows.ToList();
            ICollection<string> strHeader = strTableData.Header;
            string[] colHeader = strHeader.ToArray();
            int strCountCol = colHeader.Length;
            int strCountRow = tblRows.Count;

            for (int i = 0; i < strCountRow; i++)
            {
                for (int j = 0; j < strCountCol; j++)
                {
                    string strColVal = colHeader[j];
                    string strRowVal = tblRows[i][strColVal];
                    string strActRespVal = (string)jsonArr[i][strColVal];
                    if (!strRowVal.Equals(strActRespVal))
                    {
                        Assert.Fail("The expected value is : " + strRowVal + " The actual is : " + strActRespVal);
                    }
                }
            }
        }
    }
}
