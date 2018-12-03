using System;
using System.Configuration;
using System.Text;
using System.Threading;
using AvidxBDDFramework.Resources;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AvidxBDDFramework.Utilities
{
    static class WebUtilities
    {
        
        public static string ConfigParamValReturn(this string strParamFind)
        {
            return ConfigurationManager.AppSettings["Browser"];
        }

        internal static string fetchParamValFromConfig(string strParam)
        {
            return ConfigurationManager.AppSettings[strParam];
        }

        public static string getDirPath()
        {
            string strDirPath=null;
            try
            {
                string strDebugDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                strDirPath = strDebugDir.Replace("\\bin\\Debug\\", "");
                
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
            return strDirPath;
        }

        public static string decryptString(string strPassword)
        {
            byte[] data = Convert.FromBase64String(strPassword);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        internal static void selectLtbVal(IWebDriver iDriver, string listBxVal, string listBxName)
        {
             AvidPayUIObjects pageObj = new AvidPayUIObjects();
            
            try
            {
                if (listBxName.Equals("customer"))
                {
                    listBxVal = GenerateTxtFile.customerName;
                    if(listBxVal!=null)
                    {
                        if(pageObj.customerInputObj.Displayed)
                        {
                            pageObj.customerInputObj.Clear();
                            iDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                            pageObj.customerInputObj.SendKeys(listBxVal);
                            iDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                            Thread.Sleep(1000);

                        }
                    }
                    else
                    {
                        Assert.Fail("Customer Name is null");
                    }
                }
            }
            catch(Exception e)
            {
                Assert.Fail("Failed to enter the value in customer list, please find the screenshot for more details " + e);
            }
        }

        internal static void filterPaymentNumber(IWebDriver driver)
        {
            AvidPayUIObjects pageObj = new AvidPayUIObjects();
            try
            {
                if (pageObj.paymentNumberObj.Displayed)
                {
                    
                    pageObj.paymentNumberObj.Click();

                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(40);
                    string paymentNo = GenerateTxtFile.checkNo;

                    //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                    //var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageObj.paymentNoInputObj));
                    Thread.Sleep(3000);
                    pageObj.paymentNoInputObj.SendKeys(paymentNo);
                    Thread.Sleep(3000);
                    pageObj.filterButton.Click();
                    Thread.Sleep(3000);
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                }
            }
            catch(Exception e)
            {
                Assert.Fail("Failed to enter the value in Payment  Number filter, please find the screenshot for more details "+e);
            }
        
        }

        internal static void validateStatus(IWebDriver driver, string vals)
        {
            AvidPayUIObjects pageObj = new AvidPayUIObjects();
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5000));
                //var myElement = wait.Until(x => x.FindElement(By.XPath("//*[@id='grid']//table/tbody/tr/td[11]")));
                var myElement = wait.Until(x => pageObj.tableRowObj.Displayed);

                if(pageObj.tableRowObj.Displayed)
                {
                    string actualStatus = pageObj.tableRowObj.Text;
                    if(!actualStatus.Equals(vals))
                    {
                        Assert.Fail("Status is not : " + vals + ", is :" + actualStatus);
                    }
                    else
                    {
                        Console.WriteLine("Status is : " + actualStatus);
                    }
                }

            }catch(Exception e)
            {
                Assert.Fail("Failed to fetch the status: " +e);
            }
        }

        internal static void enterDatVal(IWebDriver iDriver, string dateVals)
        {
            try
            {
                AvidPayUIObjects pageObj = new AvidPayUIObjects();
                string[] splitDateVal = dateVals.Split(',');
                
                Thread.Sleep(1000);

                if (pageObj.startDateObj.Displayed)
                {
                    var wait = new WebDriverWait(iDriver, TimeSpan.FromSeconds(240));
                    var clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageObj.startDateObj));
                    wait = new WebDriverWait(iDriver, TimeSpan.FromSeconds(240));
                    clickableElement = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(pageObj.endDateObj));
                    
                    pageObj.startDateObj.Click();
                    iDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                    pageObj.startDateObj.Clear();
                    pageObj.startDateObj.SendKeys(splitDateVal[0]);
                    iDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                    Thread.Sleep(1000);

                    pageObj.endDateObj.Click();
                    iDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                    pageObj.endDateObj.Clear();
                    pageObj.endDateObj.SendKeys(splitDateVal[1]);
                    iDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                }
            }
            catch (TimeoutException te)
            {
                Assert.Fail("Failed to enter the date. Please find more details: " + te);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to enter the date. Please find more details: " + e);
            }
        }

    }
}
