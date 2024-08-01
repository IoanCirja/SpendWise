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
    public class AccountSettingsPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public AccountSettingsPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }

        //localtor
        public IWebElement CurrentPassword => WaitForElement(By.Id("current-password"));
        public IWebElement NewPassword => WaitForElement(By.Id("new-password"));
        public IWebElement ConfirmNewPassword => WaitForElement(By.Id("confirm-new-password"));
        public IWebElement ChangePasswordButton => WaitForElement(By.Id("change-password-button"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public AccountSettingsPage ChangePassword(PasswordReset resetPassword)
        {
            CurrentPassword.SendKeys(resetPassword.CurrentPassword);
            NewPassword.SendKeys(resetPassword.NewPassword);
            ConfirmNewPassword.SendKeys(resetPassword.ConfirmNewPassword);
            ChangePasswordButton.Click();  

            return new AccountSettingsPage(browser);
        }

        public bool IsAccountSettingsDisplayed()
        {
            try {
                return ChangePasswordButton.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
