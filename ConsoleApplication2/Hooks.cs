using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using BoDi;
using OpenQA.Selenium;
using System;
using System.Text;
using System.Reflection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Tracing;
using System.IO;
using AvidxBDDFramework.Utilities;
using AvidxBDDFramework;

namespace SpecflowParallelTest
{
    [Binding]
    public class Hooks
    {
        //Global Variable for Extend report
        private static ExtentTest featureName;
        private static ExtentTest scenario;
        private static ExtentReports extent;
        private readonly IObjectContainer _objectContainer;

        //To get the current Driver and dir path
        public static IWebDriver driver;
        public static string dirPath = WebUtilities.getDirPath();
        
        //private RemoteWebDriver _driver;

        public Hooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;
        }

        [BeforeScenario]
        [Scope(Tag = "Web")]
        public static void InitializeBrowser()
        {
            driver = BrowserManager.GetBrowserDriver();
        }


        [BeforeScenario]
        [Scope(Tag = "GenerateBAI2file")]
        public static void GenerateBAI2file()
        {
            Console.WriteLine("---------------------Generating BAI2 file---------------------");
            GenerateTxtFile.generateBAI2file();
            Console.WriteLine("------------------Successfully generated BAI2 file--------------");
        }

        [BeforeTestRun]
        public static void InitializeReport()
        {
            string repoTime = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            //Append the html report file to current project path
        string reportPath = dirPath + "\\Reports\\TestRunReport\\" + repoTime + ".html";

            //Initialize Extent report before test starts
            var htmlReporter = new ExtentHtmlReporter(reportPath);
            //htmlReporter.Configuration().Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Dark;
            htmlReporter.LoadConfig(dirPath + "\\extent-config.xml");

            //Attach report to reporter
            extent = new ExtentReports();
            extent.AttachReporter(htmlReporter);
        }

        [AfterScenario]
        [Scope(Tag = "Web")]
        public static void TearDownBrowser()
        {
            //driver  = BrowserManager.tearDownAllBrowserSession(driver);
        }

        [AfterTestRun]
        public static void TearDownReportForAPI()
        {
            //Flush report once test completes
            extent.Flush();
            if(driver != null)
            {
                driver.Quit();
            }
        }
        [BeforeFeature]
        public static void BeforeFeature()
        {
            //Create dynamic feature name
            featureName = extent.CreateTest<Feature>(FeatureContext.Current.FeatureInfo.Title);
        }

        [AfterStep]
        public void InsertReportingSteps()
        {
            //scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text)
            // var stepType = ScenarioStepContext.Current.StepInfo.StepDefinitionType.ToString();
            var stepType = ScenarioStepContext.Current.StepInfo.StepInstance.Keyword.ToString().Trim();
            PropertyInfo pInfo = typeof(ScenarioContext).GetProperty("ScenarioExecutionStatus");
            MethodInfo getter = pInfo.GetGetMethod(nonPublic: true);
            object TestResult = getter.Invoke(ScenarioContext.Current, null);

            if (ScenarioContext.Current.TestError == null)
            {
               
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text);
                else if (stepType == "And")
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text);
            }
            else if (ScenarioContext.Current.TestError != null)
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                else if (stepType == "And")
                { 
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Fail(ScenarioContext.Current.TestError.Message);
                }
                if (driver != null)
                {
                    TakeScreenshot(driver);
                }
            }

            //Pending Status
            if (TestResult.ToString() == "StepDefinitionPending")
            {
                if (stepType == "Given")
                    scenario.CreateNode<Given>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "When")
                    scenario.CreateNode<When>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "Then")
                    scenario.CreateNode<Then>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");
                else if (stepType == "And")
                    scenario.CreateNode<And>(ScenarioStepContext.Current.StepInfo.Text).Skip("Step Definition Pending");

            }
        }


        [BeforeScenario]
        [Scope(Tag = "Web")]
        public void Initialize()
        {
           if (driver == null)
            {
                InitializeBrowser();
            }
        }

        [BeforeScenario]
        public void InitializeScenario()
        {
            //Create dynamic scenario name
            scenario = featureName.CreateNode<Scenario>(ScenarioContext.Current.ScenarioInfo.Title);
        }

        //To take the screenshot and attach with the failed Scenario
        private void TakeScreenshot(IWebDriver driver)
        {
            try
            {
                string fileNameBase = string.Format("error_{0}_{1}_{2}",
                                                  "0000",
                                                   "screen",
                                                    DateTime.Now.ToString("yyyyMMdd_HHmmss"));


                var artifactDirectory = Path.Combine(dirPath + @"\Reports\", "Testresults");
                if (!Directory.Exists(artifactDirectory))
                    Directory.CreateDirectory(artifactDirectory);

                string pageSource = driver.PageSource;
                string sourceFilePath = Path.Combine(artifactDirectory, fileNameBase + "_source.html");
                File.WriteAllText(sourceFilePath, pageSource, Encoding.UTF8);
                Console.WriteLine("Page source: {0}", new Uri(sourceFilePath));
                
                ITakesScreenshot takesScreenshot = driver as ITakesScreenshot;

                if (takesScreenshot != null)
                {
                    var screenshot = takesScreenshot.GetScreenshot();

                    string screenshotFilePath = Path.Combine(artifactDirectory, fileNameBase + "_screenshot.png");
                    scenario.AddScreenCaptureFromPath(screenshotFilePath);
                    //screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);
                    screenshot.SaveAsFile(screenshotFilePath, ScreenshotImageFormat.Png);

                    Console.WriteLine("Screenshot: {0}", new Uri(screenshotFilePath));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while taking screenshot: {0}", ex);
            }
        }

    }
}
