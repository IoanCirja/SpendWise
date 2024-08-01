using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpendWiseAPIAutomationTest.Pages
{
    public class BudgetPlanPage
    {
        private IWebDriver browser;
        private WebDriverWait wait;

        public BudgetPlanPage(IWebDriver driver)
        {
            browser = driver;
            wait = new WebDriverWait(new SystemClock(), browser, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(10));
        }

        //locators
        private IWebElement BudgetPlanCard => WaitForElement(By.ClassName("budget-plan-card"));
        private IWebElement AddNewPlanButton => WaitForElement(By.ClassName("details-button"));

        //methods
        private IWebElement WaitForElement(By by)
        {
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(by));
        }

        public BudgetPlanDialog ChoseBudgetPlan(string budgetPlanName)
        {
            var budgetPlanCards = browser.FindElements(By.ClassName("budget-plan-card"));
            var buttons = browser.FindElements(By.ClassName("details-button"));

            foreach (var budgetPlanCard in budgetPlanCards)
            {
                //bool value = budgetPlanCard.Text.Contains(budgetPlanName);
                var title = budgetPlanCard.FindElement(By.Id("title"));

                if (title.Text.Contains(budgetPlanName))
                {
                    var button = budgetPlanCard.FindElement(By.ClassName("details-button"));
                    button.Click();
                    break;
                }
                  
            }

            return new BudgetPlanDialog(browser);
        }

        public bool IsBudgetPlanPageDisplayed()
        {
            try
            {
                return BudgetPlanCard.Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public CreatePlanDialog AddNewBudgetPlan()
        {
            AddNewPlanButton.Click();
            return new CreatePlanDialog(browser);
        }

        public BudgetPlanPage ShowOnlyMyPlans()
        {
            var checkbox = WaitForElement(By.Id("checkbox"));
            checkbox.Click();
            return new BudgetPlanPage(browser);
        }

        public EditBudgetPlanDialog ChooseBudgetPlanToDelete(string budgetPlanName)
        {
            var budgetPlanCards = browser.FindElements(By.ClassName("budget-plan-card"));
            var buttons = browser.FindElements(By.ClassName("edit-button"));
            foreach (var budgetPlanCard in budgetPlanCards)
            {
                var title = budgetPlanCard.FindElement(By.Id("title"));

                if (title.Text.Equals(budgetPlanName))
                {
                    var button = budgetPlanCard.FindElement(By.ClassName("edit-button"));
                    button.Click();
                    break;
                }

            }

            return new EditBudgetPlanDialog(browser);
        }

    }
}
