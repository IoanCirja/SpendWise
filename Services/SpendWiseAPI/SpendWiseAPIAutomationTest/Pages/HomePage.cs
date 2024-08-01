using Domain;
using Ocelot.Infrastructure;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SpendWiseAPIAutomationTest.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSpendWise.Pages
{
    public class HomePage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public HomePage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }

        //locators
        private IWebElement LogInButton => browser.FindElement(By.Id("loginHeader"));
        private IWebElement LogOutButton => WaitForElement(By.Id("logoutHeader"));
        private IWebElement FirstPage => WaitForElement(By.Id("first_page"));
        private IWebElement DashboardButton => WaitForElement(By.ClassName("dashboard"));
        private IWebElement BudgetPlanButton => WaitForElement(By.Id("budget-plans"));


        //methods
        public LogInPage GoToLogInPage()
        {
            LogInButton.Click();

            return new LogInPage(browser);
        }

        public DashboardPage GoToDashboard()
        { 
            DashboardButton.Click();

            return new DashboardPage(browser);
        }

        public BudgetPlanPage GoToBudgetPlanPage()
        {
            BudgetPlanButton.Click();

            return new BudgetPlanPage(browser);
        }

        public HomePage LogOut()
        {
            LogOutButton.Click();
            return new HomePage(browser);
        }

        public bool IsUserLoggedIn()
        {
            try
            {
                return FirstPage.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }

        }

        public bool IsDashboardDisplayed()
        {
            try
            {
                return DashboardButton.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

    }
}
