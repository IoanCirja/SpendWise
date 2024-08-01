using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSpendWise.Pages;
using WebApiContracts;
using System.Transactions;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class CategoryDetailsPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public CategoryDetailsPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }

        //locators
        public IWebElement ReturnButton => WaitForElement(By.ClassName("back-button"));
        public IWebElement FilterCategory => WaitForElement(By.Id("filter-by-category"));

        public By FilterCategoryOptions => By.Id("filter-category-select");

        public IWebElement SortBy => WaitForElement(By.Id("sort-by"));
        public IWebElement SortByDateAsc => WaitForElement(By.Id("dateAsc"));
        public IWebElement SortByDateDesc => WaitForElement(By.Id("dateDesc"));
        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public CurrentPlanPage ReturnToCurrentPlan()
        { 
            ReturnButton.Click();
            return new CurrentPlanPage(browser);
        }

        public bool IsReturnButtonDisplayed()
        {
            try {
                return ReturnButton.Displayed;
            }
            catch(NoSuchElementException) 
            { 
                return false; 
            }
        }

        public CategoryDetailsPage FilterByCategory(TransactionsContract filter)
        {
            FilterCategory.Click();
            var options = browser.FindElements(FilterCategoryOptions);
            //wait
            Thread.Sleep(3000);
            foreach (var option in options)
            {
                if (option.Text.Equals(filter.category, StringComparison.OrdinalIgnoreCase))
                {
                    option.Click();
                    break;
                }
            }

            return new CategoryDetailsPage(browser);
        }
        public CategoryDetailsPage SortByDateAscending()
        {
            SortBy.Click();
            SortByDateAsc.Click();
            return new CategoryDetailsPage(browser);
        }

        public CategoryDetailsPage SortByDateDescending()
        {
            SortBy.Click();
            SortByDateDesc.Click();
            return new CategoryDetailsPage(browser);
        }

    }
}
