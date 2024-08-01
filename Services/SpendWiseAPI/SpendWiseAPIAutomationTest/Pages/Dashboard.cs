using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class DashboardPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public DashboardPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }
        //locators
 
        public IWebElement ViewTransactionsButton => WaitForElement(By.ClassName("view-transactions-button"));
        public IWebElement AccounSettingsButton => WaitForElement(By.Id("account-settings"));
        public IWebElement HistoryButton => WaitForElement(By.Id("history"));
        public IWebElement StatisticsButton => WaitForElement(By.Id("statistics"));
        public IWebElement CurrentPlanButton => WaitForElement(By.Id("current-plan"));


        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

/*        public TransactionPage GoToTransactionPage()
        {
            AddTransactionButton.Click();
            return new TransactionPage(browser);
        }*/

        public CategoryDetailsPage GoToCategoryDetailsPage()
        {
            ViewTransactionsButton.Click();
            return new CategoryDetailsPage(browser);
        }

        public AccountSettingsPage GoToAccountSettingsPage()
        {
            AccounSettingsButton.Click();
            return new AccountSettingsPage(browser);
        }

        public HistoryPage GoToHistoryPage()
        {
            HistoryButton.Click();
            return new HistoryPage(browser);
            
        }

        public StatisticsPage GoToStatisticsPage()
        {
            StatisticsButton.Click();
            return new StatisticsPage(browser);
        }

        public CurrentPlanPage GoToCurrentPlanPage()
        {
            CurrentPlanButton.Click();
            return new CurrentPlanPage(browser);
        }

    }
}
