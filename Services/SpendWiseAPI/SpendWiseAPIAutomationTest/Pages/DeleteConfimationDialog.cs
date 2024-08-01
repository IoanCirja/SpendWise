using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class DeleteConfimationDialog
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public DeleteConfimationDialog(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }

        //locators
        private IWebElement CancelButton => WaitForElement(By.Id("cancel"));
        private IWebElement ConfirmButton => WaitForElement(By.Id("confirm"));
        private IWebElement Title => WaitForElement(By.Id("confirm-deletion"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public BudgetPlanPage ConfirmDeletePlan()
        {
            ConfirmButton.Click();
            return new BudgetPlanPage(browser);
        }

        public bool IsDeleteConfimationDialogDisplayed()
        {
            try {
                return Title.Displayed;
            }
            catch (NoSuchElementException) {
                return false;
            }
        }
    }
}
