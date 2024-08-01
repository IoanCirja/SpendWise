using Ocelot.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class EditBudgetPlanDialog
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public EditBudgetPlanDialog(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }
        //locators
        private IWebElement DeleteButton => WaitForElement(By.ClassName("delete-btn"));

        private IWebElement CancelButton => WaitForElement(By.ClassName("cancel-btn"));
        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public DeleteConfimationDialog GoToDeleteConfimationDialog()
        {
            DeleteButton.Click();
            return new DeleteConfimationDialog(browser);
        }

        public bool IsEditBudgetPlanPageDisplayed()
        {
            try {
                return DeleteButton.Displayed;
            }
            catch(NoSuchElementException) { 
                return false;
            }
        }
    }
}
