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

        [Given(@"I select the ""(.*)"" from the customer listbox")]
        public void GivenISelectTheCustomerFromCustomerListbox(string listBxName)
        {
            NavigationToUtilities.navToObjUtilities(driver,"", listBxName);
            Console.WriteLine("-----Selected customer----");
        }

        [Given(@"I enter ""(.*)"" in the date feilds")]
        public void GivenIEnterInDateFeilds(string dateVals)
        {
            NavigationToUtilities.navToUtilities(driver, dateVals,"Dates");
            Console.WriteLine("-----Entered date values----");
        }

        [When(@"I filter the result with the payment number")]
        public void WhenIFilterTheResultWithThePaymentNumber()
        {
            NavigationToUtilities.navToUtilities(driver, "", "PaymentNumber");
        }

        [Then(@"I should see the status of payment number is ""(.*)""")]
        public void ThenIShouldSeeTheStatusOfPaymentNumberIs(string status)
        {
            NavigationToUtilities.navToUtilities(driver, status, "Status");
        }

        [When(@"the user waits for ""(.*)"" seconds")]
        public void WhenTheUserWaitsForSeconds(int secs)
        {
            NavigationToUtilities.waitForSecs(secs);
        }

       



        //Step defs for FTP file copy

        [Given(@"FTP folder location ""(.*)""")]
        public void GivenFTPFolderLocation(string ftpPath)
        {
            NavigationToUtilities.navToFTPUtilities(ftpPath, "ftpPath");
        }

        [When(@"I upload the generated BAI2 file")]
        public void WhenIUploadTheGeneratedFile()
        {
            NavigationToUtilities.navToFTPUtilities("", "filename");
        }

        [Then(@"upload file in the FTP location should be successful")]
        public void ThenUploadFileInTheFTPLocationShouldBeSuccessful()
        {
            NavigationToUtilities.navToFTPUtilities("", "Validate");
        }

        [Then(@"the generated file extension should change to ""(.*)""")]
        public void ThenTheGeneratedFileExtensionShouldChangeTo(string fileExt)
        {
            NavigationToUtilities.navToFTPUtilities(fileExt,"validateFileExt");
        }

        [Then(@"the file extension should not change to""(.*)""")]
        public void ThenTheFileExtensionShouldNotChangeTo(string p0)
        {
        NavigationToUtilities.navToFTPUtilities("", "validateInvfilename");
        }



        //Step defs for Rest Api

        [Given(@"I have ""(.*)"" api")]
        public void GivenIHaveApi(string apiName)
        {
            NavigationToUtilities.setUrl(apiName);
        }

        [Given(@"I have request json with payment number and amount")]
        public void GivenIHaveJsonWithAnd()
        {
            NavigationToUtilities.fetchCustPayDt();
        }

        [When(@"I send a ""(.*)"" request")]
        public void WhenISendARequest(string request)
        {
            NavigationToUtilities.sendRequest(request);
        }

        [Then(@"the response should be ""(.*)""")]
        public void ThenTheResponseShouldBe(int resCode)
        {
            NavigationToUtilities.respCode(resCode);
        }

        [Given(@"I have request json with valid '(.*)' and used '(.*)'")]
        [Given(@"I have request json with e-Payment Check Cleared '(.*)' and '(.*)'")]
        public void GivenIHaveRequestJsonWithInvalidAnd(string paymentNo, string amount)
        {
            NavigationToUtilities.createRequest(paymentNo, amount);
        }

       
        [Then(@"the reponse ""(.*)"" should contain ""(.*)""")]
        public void ThenTheReponseShouldContain(string jsonKey, string keyVal)
        {
            NavigationToUtilities.fetchValFromResp(jsonKey, keyVal);
        }



        //StepDef for Database validations
        [Given(@"I have AvidPayTransaction database")]
        public void GivenIHaveAvidPayTransactionDatabase()
        {
            NavigationToUtilities.navToDbConnection("","openDbCon");
        }

        [When(@"I query for ""(.*)"" status")]
        public void WhenIQueryForPaymentProcessingStatusTypeIDStatus(string queryFor)
        {
            NavigationToUtilities.navToDbConnection(queryFor, "genQuery");
        }

        [Then(@"I should see the status as ""(.*)""")]
        public void ThenIShouldSeeTheStatusAs(string status)
        {
            NavigationToUtilities.navToDbConnection(status, "status");
        }


        //StepDef for windows services
        [Given(@"I have list of Windows services")]
        public void GivenIHaveListOfWindowsServices()
        {
            NavigationToUtilities.navToWindowsUtilities("", "listofservices");
        }

        [When(@"I check for the status of ""(.*)"" and ""(.*)""")]
        public void WhenICheckForTheStatusOfAnd(string serviceNameOne, string serviceNameTwo)
        {
            string conServicename = serviceNameOne + ":" + serviceNameTwo;
            NavigationToUtilities.navToWindowsUtilities(conServicename, "servicenames");
        }

        [Then(@"the status should be ""(.*)""")]
        public void ThenTheStatusShouldBe(string serviceStatus)
        {
            NavigationToUtilities.navToWindowsUtilities(serviceStatus, "servicestatus");
        }

        //Upload an Invalid BAI2 file
        [When(@"I upload the invalid BAI2 file")]
        public void WhenIUploadTheInvalidBAIFile()
        {
            NavigationToUtilities.navToFTPUtilities("", "Invfilename");
        }

        //Steps for Validate Payment Amount in the BAI2 file
        [Given(@"I have genrocket generated BAI2 File")]
        public void GivenIHaveaBAI2File()
        {
            NavigationToUtilities.navToWindowsUtilities("", "ftpGenfileLocation");
        }

        [When(@"I open the BAI2 file and review Payment Amount")]
        public void WhenIOpenTheFileAndReviewPaymentAmount()
        {
            NavigationToUtilities.navToWindowsUtilities("", "OpenFile");
        }

        [Then(@"Payment Amount should be greater than zero")]
        public void ThenPaymentAmountShouldBeGreaterThatZero()
        {
        NavigationToUtilities.navToWindowsUtilities("", "ValPayAmount");
        }
    }
}