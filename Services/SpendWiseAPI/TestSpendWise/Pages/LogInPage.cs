using OpenQA.Selenium;
using OpenQA.Selenium.DevTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiContracts;

namespace TestSpendWise.Pages
{
    public class LogInPage
    {
        private IWebDriver browser;

        public LogInPage(IWebDriver driver)
        {
            browser = driver;
        }

        //locators
        private IWebElement Email => browser.FindElement(By.Id("email"));
        private IWebElement Password => browser.FindElement(By.Id("password"));
        private IWebElement LogInButton => browser.FindElement(By.Id("login"));
        private IWebElement LogInTitle => browser.FindElement(By.Id("signin"));


        //methods
        public string GetPageTitle()
        {
            return LogInTitle.Text;
        }
        public HomePage FillLogInForm( UserCredentialsContract1 user) { 
            Email.SendKeys(user.Email);
            Password.SendKeys(user.Password);
            LogInButton.Click();

            return new HomePage(browser);

        }

    }
}
