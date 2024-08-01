using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeleniumExtras.WaitHelpers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class CancelPlanDialog
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public CancelPlanDialog(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(new SystemClock(), browser, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10));
        }

        //locators
        private IWebElement CancelButton => WaitForElement(By.Id("cancel"));
        private IWebElement ConfirmButton => WaitForElement(By.Id("confirm"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public CurrentPlanPage ConfirmCancelCurrentPlan()
        {
            ConfirmButton.Click();
            return new CurrentPlanPage(browser);
        }

        public CurrentPlanPage DontCancelCurretnPlan()
        {
            CancelButton.Click();
            return new CurrentPlanPage(browser);
        }

        public bool isClicked(IWebElement element)
        {
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(element));
                element.Click();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsCancelPlanDialogDisplayed()
        {
            try {
                return ConfirmButton.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
