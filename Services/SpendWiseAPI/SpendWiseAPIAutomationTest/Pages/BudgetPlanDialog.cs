using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiContracts;
using System.Transactions;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class BudgetPlanDialog
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public BudgetPlanDialog(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(new SystemClock(), browser, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10));
        }

        //locators
        private IWebElement CategoryInputs => WaitForElement(By.ClassName("category-inputs"));
        private IWebElement CategoryInput => WaitForElement(By.ClassName("category-input"));
        private IWebElement SaveButton => WaitForElement(By.Id("save"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public CurrentPlanPage FillBudgetPlanDialog()
        {

            var categoryInputFields = browser.FindElements(By.Id("category-input"));

            int inputFieldsCount = categoryInputFields.Count;

            //random number 
            Random rnd = new Random();

            for (int i = 0; i < inputFieldsCount; i++)
            {
                //wait
                Thread.Sleep(2000);
                string value = rnd.Next(100, 1000).ToString();
                categoryInputFields[i].Click();
                categoryInputFields[i].Clear();
                categoryInputFields[i].SendKeys((value));
            }

            SaveButton.Click();

            return new CurrentPlanPage(browser);
        }

        public bool IsBudgetPlanDialogDisplayed()
        {
            try
            {
                return CategoryInputs.Displayed;
            }
            catch (NoSuchElementException) { 
                return false;
            }
        }

        
    }
}
