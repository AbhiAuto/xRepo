using System;
using OpenQA.Selenium;
using NUnit.Framework;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Collections.Generic;
using AvidxBDDFramework.Utilities;

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

        internal static void navToUtilities(IWebDriver driver, string datevals, string flag)
        {
            if(flag.Equals("Dates"))
            {
                WebUtilities.enterDatVal(driver, datevals);
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
                ftpFileTransfer.setFilename(ftpval);
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

        internal static void NavToScreen(IWebDriver driver, string screenName)
        {
            if(screenName.Equals("my personal information"))
            {
                driver.FindElement(By.XPath("//a[@title='Information']")).Click();
                System.Threading.Thread.Sleep(5000);
            }
        }

    }
}
