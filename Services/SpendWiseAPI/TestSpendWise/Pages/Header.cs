using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestSpendWise.Pages
{
    public class Header
    {
        private IWebDriver browser;

        public Header(IWebDriver driver)
        {
            browser = driver;
        }

        private IWebElement UserInfo => browser.FindElement(By.ClassName("user-info"));

        public bool IsUserLoggedIn() {
            return UserInfo.Displayed;
        }

    }
}
