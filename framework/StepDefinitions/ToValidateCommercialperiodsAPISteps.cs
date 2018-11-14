using System;
using TechTalk.SpecFlow;

namespace FinOpsAutomation.StepDefinitions
{
    [Binding]
    public class ToValidateCommercialperiodsAPISteps
    {
        [Given(@"I have ""(.*)"" APIs")]
        public void GivenIHaveAPIs(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Given(@"with paramater ""(.*)"" value ""(.*)""s")]
        public void GivenWithParamaterValueS(string p0, int p1)
        {
            ScenarioContext.Current.Pending();
        }
        
        [When(@"I send a ""(.*)"" requests")]
        public void WhenISendARequests(string p0)
        {
            ScenarioContext.Current.Pending();
        }
        
        [Then(@"the response should contain the following detailss:")]
        public void ThenTheResponseShouldContainTheFollowingDetailss(Table table)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
