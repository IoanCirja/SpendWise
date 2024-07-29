using OpenQA.Selenium;
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

        public HomePage(IWebDriver driver)
        {
            browser = driver;
        }

        //locators
        private IWebElement LogInButton => browser.FindElement(By.Id("login"));
        private IWebElement LogOutButton => browser.FindElement(By.Id("logout"));
/*        private IWebElement LogOutB => browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);*/


        //methods
        public LogInPage GoToLogInPage()
        {
            LogInButton.Click();

            return new LogInPage(browser);
        }
        public HomePage LogOut()
        {
            
            LogOutButton.Click();

            return new HomePage(browser);
        }

        public bool IsUserLoggedIn()
        {

            return LogOutButton.Displayed;
        }

    }
}
