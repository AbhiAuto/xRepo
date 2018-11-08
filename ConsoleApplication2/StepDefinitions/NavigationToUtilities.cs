﻿using System;
using OpenQA.Selenium;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;
using AvidxBDDFramework.Utilities;
using System.Net;

namespace AvidxBDDFramework
{
    class NavigationToUtilities
    {
        private static ICollection<string> str;

        internal static void LauchUrl(IWebDriver driver, string strAppName)
        {
            try
            {
                //fetch test data from config file for base url generation
                string strDomain = WebUtilities.fetchParamValFromConfig(strAppName);
                string username = WebUtilities.fetchParamValFromConfig("username");
                string password = WebUtilities.fetchParamValFromConfig("password");
                string dePassword = WebUtilities.decryptString(password);
                string baseUrl = "http://" + username + ":" + dePassword + "@" + strDomain;
               
                //Navigating to base page
                driver.Url = baseUrl;
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
            catch (Exception e)
            {
                Assert.Fail("Failed to lauch the URL, please find the log below for more details" + e);
            }

        }

        internal static void navToObjUtilities(IWebDriver driver, string listBxVal, string listBxName)
        {
            WebUtilities.selectLtbVal(driver, listBxVal, listBxName);
        }

        internal static void navToUtilities(IWebDriver driver, string vals, string flag)
        {
            if(flag.Equals("Dates"))
            {
                WebUtilities.enterDatVal(driver, vals);
            }
            if(flag.Equals("PaymentNumber"))
            {
                WebUtilities.filterPaymentNumber(driver);
            }
            if(flag.Equals("Status"))
            {
                WebUtilities.validateStatus(driver, vals);
            }
            
        }

        internal static void navToFTPUtilities(string ftpval, string flag)
        {
            if (flag.Equals("ftpPath"))
            {
                ftpFileTransfer.setftpPath(ftpval);
            }
            else if (flag.Equals("filename"))
            {
                ftpFileTransfer.setFilename();
            }
            else if(flag.Equals("validate"))
            {
                ftpFileTransfer.valUpload(ftpval);
            }
        }

        internal static void navToPage(IWebDriver driver, string strPageName)
        {
            if (strPageName.Equals("Manage Payments"))
            {
                driver.FindElement(By.Id("manage-payments-link")).Click();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
         }

        internal static void setUrl(string apiName)
        {
            APIUtilities.setApiUrl(apiName);
        }

        internal static void fetchCustPayDt()
        {
            APIUtilities.fetchsetCustPayDt();
        }

        internal static void sendRequest(string request)
        {
            if(request.Equals("Post"))
            {
                APIUtilities.sendPostRequest();
            }
        }

        internal static void respCode(int resCode)
        {
            int statusCode = APIUtilities.statusCode;
            if (statusCode != resCode)
            {
                Assert.Fail("Stattus code is : " + resCode + ". Please find the errror details :"+ APIUtilities.jsonResArray.ToString());
            }
        }

        internal static void createRequest(string paymentNo, string amount)
        {
            APIUtilities.createJsonRequest(paymentNo,amount);
        }

        internal static void NavToScreen(IWebDriver driver, string screenName)
        {
            try
            {
                if(screenName.Equals("my personal information"))
                {
                    driver.FindElement(By.XPath("//a[@title='Information']")).Click();
                    System.Threading.Thread.Sleep(5000);
                }
            }
            catch(Exception e)
            {
                Assert.Fail("Failed to click on my personal information link, please find the screenshot for more details " + e);
            }

        }

    }
}
