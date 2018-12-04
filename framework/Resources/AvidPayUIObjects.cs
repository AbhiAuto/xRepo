using AvidxBDDFramework.Utilities;
using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvidxBDDFramework.Resources
{
    class AvidPayUIObjects
    {
       
        public AvidPayUIObjects()
        {
            PageFactory.InitElements(BrowserManager.driver, this);
        }

        [FindsBy(How = How.Id, Using = "manage-payments-link")]
        public IWebElement managePayLink { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='toolbar']//span/input[@placeholder='Select customer']")]
        public IWebElement customerInputObj { get; set; }

        [FindsBy(How = How.XPath, Using = "//table/thead/tr/th[@data-title='Payment Number']/a[@class='k-grid-filter']")]
        public IWebElement paymentNumberObj { get; set; }

        [FindsBy(How = How.XPath, Using = "//form/div[1]/input[1]")]
        public IWebElement paymentNoInputObj { get; set; }

        [FindsBy(How = How.XPath, Using = "//button[text()='Filter']")]
        public IWebElement filterButton { get; set; }
                
        [FindsBy(How = How.XPath, Using = "//*[@id='grid']//table/tbody/tr/td[11]")]
        public IWebElement tableRowObj { get; set; }


        [FindsBy(How = How.Name, Using = "start-date-picker")]
        public IWebElement startDateObj { get; set; }

        [FindsBy(How = How.Name, Using = "end-date-picker")]
        public IWebElement endDateObj { get; set; }
       
    }
}
       
    

 