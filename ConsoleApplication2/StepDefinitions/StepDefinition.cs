using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using System;
using TechTalk.SpecFlow;
using OpenQA.Selenium;
using System.Threading.Tasks;
using AvidxBDDFramework.Utilities;
using System.Diagnostics;

namespace AvidxBDDFramework
{
    [Binding]
    class StepDefinition
    {
        public IWebDriver driver = BrowserManager.GetCurrentBrowserDriver();

        [Given(@"I have logged into ""(.*)"" portal")]
        public void GivenIHaveLoggedIntoPortal(string strAppName)
        {
            NavigationToUtilities.LauchUrl(driver, strAppName);
        }

        [Given(@"I navigate to ""(.*)"" page")]
        public void GivenINavigateToManagePaymentsPage(string strPageName)
        {
            NavigationToUtilities.navToPage(driver, strPageName);
        }

        [Given(@"I select the customer ""(.*)"" from ""(.*)"" listbox")]
        public void GivenISelectTheCustomerFromCustomerListbox(string listBxVal, string listBxName)
        {
            NavigationToUtilities.navToObjUtilities(driver, listBxVal, listBxName);
        }

        [Given(@"I enter ""(.*)"" in the date feilds")]
        public void GivenIEnterInDateFeilds(string dateVals)
        {
            NavigationToUtilities.navToUtilities(driver, dateVals,"Dates");
        }



        //Step defs for FTP file copy

        [Given(@"FTP folder location ""(.*)""")]
        public void GivenFTPFolderLocation(string ftpPath)
        {
            DBConnection.sqlConnect();
            //NavigationToUtilities.navToFTPUtilities(ftpPath, "ftpPath");
        }

        [When(@"I upload the file ""(.*)""")]
        public void WhenIUploadTheFile(string fileName)
        {
            NavigationToUtilities.navToFTPUtilities(fileName, "filename");
        }

        [Then(@"upload file in the FTP location should be successful")]
        public void ThenUploadFileInTheFTPLocationShouldBeSuccessful()
        {
            NavigationToUtilities.navToFTPUtilities("", "Validate");
        }

    }

}