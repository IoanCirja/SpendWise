using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class CurrentPlanPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public CurrentPlanPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(new SystemClock(), browser, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10));
        }
        //locators
        private IWebElement AddPlanButton => WaitForElement(By.ClassName("details-button"));

        private IWebElement Container => WaitForElement(By.ClassName("container"));
        private IWebElement CancelPlanButton => WaitForElement(By.ClassName("cancel-button"));
        public IWebElement AddTransactionButton => WaitForElement(By.Id("transaction"));

        //methods
        public bool IsPlanActive()
        {
            bool NoPlanMessage = IsElemetPresent(By.ClassName("no-plan-message"));
            if (NoPlanMessage)
            {
                IWebElement Here = browser.FindElement(By.Id("here"));
                Here.Click();
                return false;
            }
            else { 
                return true;
            }
        }

        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        private bool IsElemetPresent(By by)
        {
            try 
            {
                browser.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false; 
            }
        }

        public bool IsCurrentPlanPageDisplayed()
        {
            try
            {
                return Container.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public CancelPlanDialog CancelCurrentPlan()
        {
            CancelPlanButton.Click();
            return new CancelPlanDialog(browser);
        }

        public CreatePlanDialog CreateNewPlan()
        {
            AddPlanButton.Click();
            return new CreatePlanDialog(browser);
        }

        public TransactionPage GoToTransactionPage()
        {
            AddTransactionButton.Click();
            return new TransactionPage(browser);
        }

    }
}
