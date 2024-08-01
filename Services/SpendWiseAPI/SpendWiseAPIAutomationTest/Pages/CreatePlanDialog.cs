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
    public class CreatePlanDialog
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public CreatePlanDialog(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(browser, TimeSpan.FromSeconds(20));
        }

        //locators
        private IWebElement PlanName => WaitForElement(By.Id("plan-name"));
        private IWebElement PlanDescription => WaitForElement(By.Id("plan-description"));
        private IWebElement PlanIcon => WaitForElement(By.Id("plan-icon"));
        private IWebElement AddCategoryButton => WaitForElement(By.Id("add-category"));
        private IWebElement DeleteCategoryButton => WaitForElement(By.Id("delete-category"));
        private IWebElement SaveButton => WaitForElement(By.ClassName("save-btn"));


        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public BudgetPlanPage AddNewPlan(BudgetPlanContract budgetPlan)
        {
            PlanName.Click();
            PlanName.SendKeys(budgetPlan.Name);

            PlanDescription.Click();
            PlanDescription.SendKeys(budgetPlan.Description);

            PlanIcon.Click();
            PlanIcon.SendKeys(budgetPlan.Imagine);

            int i;
            for (i = 0; i < budgetPlan.NoCategory-1; i++)
            {
                var categoryName = browser.FindElements(By.Id("category-name"));
                string value = Guid.NewGuid().ToString("n").Substring(0, 8);
                categoryName[i].Click();
                categoryName[i].SendKeys(value);
                AddCategoryButton.Click();
            }

            var CategoryName = browser.FindElements(By.Id("category-name"));
            string value1 = Guid.NewGuid().ToString("n").Substring(0, 8);
            CategoryName[i].Click();
            CategoryName[i].SendKeys(value1);

            SaveButton.Click();

            return new BudgetPlanPage(browser);
        }

        public BudgetPlanPage DeletePlan()
        {

            return new BudgetPlanPage(browser);
        }

        public bool IsCreateBudgetPlanDialogDisplayed()
        {
            try {
                return PlanName.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
