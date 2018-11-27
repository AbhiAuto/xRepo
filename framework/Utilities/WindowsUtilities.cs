using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AvidxBDDFramework.Utilities
{
    class WindowsUtilities
    {
        public static ServiceController[] services;
        public static string servicename, PayAmount, Line4Text, bai2GenPath;
        public static bool PayAmozero;

        //Fetching the list of windows services and storing into a variable
        internal static void getListOfWindowsServices()
        {
            try
            {
                //Storing all the services available in the windows system
                services = ServiceController.GetServices();
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to fetch the service details : " + e);
            }

        }

        internal static void setserviceNames(string val)
        {
            servicename = val;
        }

        // To validate the AvidX service status
        internal static void validateServiceStatus(string val)
        {
            int i = 0;
            string[] splitServiceName = null;
            try
            {
                if (servicename.Contains(":"))
                {
                    splitServiceName = servicename.Split(':');
                    i = splitServiceName.Length;
                }
                for (int j = 0; j < i; j++)
                {
                    servicename = splitServiceName[j];
                    foreach (ServiceController service in services)
                    {
                        if (service.ServiceName.Equals(servicename))
                        {
                            Console.WriteLine("Service Name :" + service.ServiceName);
                            string servicStatus = service.Status.ToString().Trim();
                            if (servicStatus.Equals(val))
                            {
                                Console.WriteLine("Service status :" + service.Status);
                            }
                            else
                            {
                                Assert.Fail(service.ServiceName + " status is : " + service.Status);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Assert.Fail("Failed to check the status of service : " + e);
            }
        }

        internal static void readTextFile()
        {
            int count = 1;
            foreach (string line in File.ReadLines(bai2GenPath, Encoding.UTF8))
            {
                string checkText = line;
                checkText = checkText.Substring(0, 2);
                if (checkText == "16")
                {
                    Line4Text = line;
                }
                count++;
            }
            PayAmount = Line4Text.Substring(3, Line4Text.Length - 3);
            PayAmount = PayAmount.Substring(PayAmount.IndexOf(",") + 1, PayAmount.Length - (PayAmount.IndexOf(",") + 1));
            PayAmount = PayAmount.Substring(0, PayAmount.IndexOf(","));
            //checking each character           
            PayAmozero = PayAmount.Any(c => c >= '1' && c <= '9');
        }

        internal static void ValPayAmount()
        {
            if (PayAmozero)
            {
                Assert.IsTrue(true, "Payment Amount is diferent than Zero");
            }
            else
            {
                Assert.False(true, "Payment Amount is Zero");
            }

        }

        internal static void setftpGenFileLocation()
        {
            bai2GenPath = GenerateTxtFile.genFilePath;
        }
    }
 }
