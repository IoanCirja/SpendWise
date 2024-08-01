using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiContracts;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class TransactionPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public TransactionPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }

        //locators
        private IWebElement TransactionName => WaitForElement(By.Id("transaction-name"));
        private IWebElement TransactionSelectCategory => WaitForElement(By.Id("transaction-category"));
        private IWebElement TransactionAmount => WaitForElement(By.Id("transaction-amount"));
        private By TransactionCategoryOptions => By.Id("transaction-category-select");
        private IWebElement TransactionSaveButton => WaitForElement(By.Id("transaction-save"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public bool IsTransactionPageDisplayed()
        {
            try
            {
                return TransactionName.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public CurrentPlanPage FillInTransactionForm(TransactionsContract transaction) 
        {
            TransactionName.SendKeys(transaction.name);
            TransactionSelectCategory.Click();
            var options = browser.FindElements(TransactionCategoryOptions);

            //wait
            Thread.Sleep(3000);
            foreach (var option in options)
            {
                if (option.Text.Equals(transaction.category, StringComparison.OrdinalIgnoreCase))
                {
                    option.Click();
                    break;
                }
            }
            string value = transaction.amount.ToString();
            TransactionAmount.SendKeys(value);

            TransactionSaveButton.Click();

            return new CurrentPlanPage(browser);
            
        }


    }
}
