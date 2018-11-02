﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            string strDebugDir = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            string strDirPath = strDebugDir.Replace("\\bin\\Debug\\", "");
            return strDirPath;
        }

        public static string decryptString(string strPassword)
        {
            byte[] data = Convert.FromBase64String(strPassword);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        internal static void selectLtbVal(IWebDriver driver, string listBxVal, string listBxName)
        {
            if (listBxName.Equals("customer"))
            {
                driver.FindElement(By.XPath("//*[@id='toolbar']//span[@class='k-dropdown-wrap k-state-default']/input")).Clear();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                               
                driver.FindElement(By.XPath("//*[@id='toolbar']//span[@class='k-dropdown-wrap k-state-default']/input")).SendKeys(listBxVal);
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            }
        }

        internal static void enterDatVal(IWebDriver driver, string dateVals)
        {
            string[] splitDateVal = dateVals.Split(',');
            driver.FindElement(By.Id("start-date-picker")).Clear();
            driver.FindElement(By.Id("start-date-picker")).SendKeys(splitDateVal[0]);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.Id("end-date-picker")).Clear();
            driver.FindElement(By.Id("end-date-picker")).SendKeys(splitDateVal[1]);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        public class Constants
        {
            public const int ImplicitWaitTime = 20;
            public const int PageLoadWaitTime = 90;
            public const int DefaultWaitInSeconds = 5;
            public const int DefaultPollingWaitInMilliSeconds = 200;

        }

    }
}