using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class StatisticsPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public StatisticsPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }
        //locators
        private IWebElement StatisticsCard => WaitForElement(By.ClassName("statistics-card"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public bool IsStatisticsPageDisplayed()
        {
            try
            {
                return StatisticsCard.Displayed;
            }
            catch (NoSuchElementException)
            { 
                return false; 
            }
        }
        

    }
}
