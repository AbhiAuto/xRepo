using AvidxBDDFramework.Utilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.IO;
using TechTalk.SpecFlow;


namespace AvidxBDDFramework.Utilities
{
    [Binding]
    class BrowserManager
    {
        public static IWebDriver driver;
        public static string dirPath = WebUtilities.getDirPath();


        public static IWebDriver GetCurrentBrowserDriver()
        {
            return driver;
        }

        public static IWebDriver GetBrowserDriver()
        {

            if (driver != null)
            {
                return driver;
            }

            var browser = WebUtilities.ConfigParamValReturn("Browser");


            if (!string.IsNullOrEmpty(browser))
            {
                switch (browser)
                {
                    case "Firefox":

                        driver = getFirefoxDriver();
                        break;

                    case "IE":

                        driver = getIEDriver();
                        break;


                    case "Chrome":

                        driver = getChromeDriver();
                        break;

                    default:
                        //if there is no browser details is configured
                        Assert.Fail("Please add the configuration for " + browser + " in the BrowserManger");
                        break;
                }
            }
            //if no browser key value is provided in the app.config
            else
            {
                Assert.Fail("No browser input key is provided, please check and verify");
            }

            //driver.Manage().Timeouts().ImplicitlyWait = TimeSpan.FromSeconds(Constants.ImplicitWaitTime);
            //driver.Manage().Timeouts().SetPageLoadTimeout = TimeSpan.FromSeconds(Constants.PageLoadWaitTime);

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Window.Maximize();
            return driver;
        }

        public static IWebDriver getChromeDriver()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("safebrowsing.enabled", true);
            chromeOptions.AddUserProfilePreference("credentials_enable_service", false);
            chromeOptions.AddUserProfilePreference("profile.password_manager_enabled", false);
            chromeOptions.AddArgument("no-sandbox");
            return new ChromeDriver(chromeOptions);

            //ChromeOptions chromeOptions = new ChromeOptions();
            //chromeOptions.BinaryLocation = dirPath + "Chrome/Application/chrome.exe";
            //driver  = new ChromeDriver(dirPath +"/Drivers/Chrome/",chromeOptions);
            //return driver;
        }

        internal static IWebDriver tearDownAllBrowserSession(IWebDriver driver)
        {
            if (driver != null)
            {
                driver.Quit();
                driver.Dispose();
                driver = null;
            }
            BrowserManager.driver = null;
            return driver;
        }

        public static IWebDriver getIEDriver()
        {
            var internetExplorerOptions = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                IgnoreZoomLevel = true,
                EnableNativeEvents = true
            };

            return Directory.Exists(@"c:\deploy")
           ? new InternetExplorerDriver(@"c:\deploy", internetExplorerOptions)
           : new InternetExplorerDriver(internetExplorerOptions);
        }

        public static IWebDriver getFirefoxDriver()
        {
            throw new NotImplementedException();
        }
    }
}
