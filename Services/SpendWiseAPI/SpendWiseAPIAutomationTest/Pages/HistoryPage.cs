using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class HistoryPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public HistoryPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }
        //locators
        private IWebElement Container => WaitForElement(By.Id("history-page"));
        private IWebElement ExportButton => WaitForElement(By.ClassName("export-button"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public bool IsHistoryPageDisplayed()
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

        public HistoryPage ViewPastPlan(string value)
        {
            var planButton = browser.FindElements(By.Id("plan-button"))
                                   .FirstOrDefault(button => button.Text.Contains(value));

            if(planButton != null)
                planButton.Click();
            return new HistoryPage(browser);
        }

        public bool IsPastPlanDisplayed()
        {
            try
            {
                return ExportButton.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }

        }

        public void ExportPlan()
        {
            ExportButton.Click();
        }
    }
}
