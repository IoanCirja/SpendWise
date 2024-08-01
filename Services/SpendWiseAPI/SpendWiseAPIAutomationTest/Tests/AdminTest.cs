using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSpendWise.Pages;
using WebApiContracts;

namespace SpendWiseAPIAutomationTest.Tests
{
    [TestClass]
    public class AdminTest
    {
        private IWebDriver driver;


        [TestInitialize]
        public void SetupTest()
        {
            ChromeOptions options = new ChromeOptions();

            options.AddArgument("--no-default-browser-check");

            options.AddArgument("--no-first-run");
            options.AddArgument("--disable-search-engine-choice-screen");

            driver = new ChromeDriver(options);
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:4200");
        }


        [TestMethod]
        public void Should_LogInAsAdmin_When_LogInFormContainsValidData()
        {
            // go to log in page
            var homePage = new HomePage(driver);
            var logInPage = homePage.GoToLogInPage();

            //wait
            Thread.Sleep(3000);

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "andrei.cazamir@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var dasboardPage = homePage.GoToDashboard();

            //wait
            Thread.Sleep(3000);

            //assert: check if dashboard page is displayed
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");
        }

        [TestMethod]
        public void Should_CreateNewPlan_When_UserIsLogInAsAdmin()
        {
            // go to log in page
            var homePage = new HomePage(driver);
            var logInPage = homePage.GoToLogInPage();

            //wait
            Thread.Sleep(3000);

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "andrei.cazamir@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var budgetPlanPage = homePage.GoToBudgetPlanPage();

            //wait
            Thread.Sleep(3000);

            //Assert: check if budget plan page is displayed
            Assert.IsTrue(budgetPlanPage.IsBudgetPlanPageDisplayed(), "The budget card is not found");

            var createPlanDialog = budgetPlanPage.AddNewBudgetPlan();

            //wait
            Thread.Sleep(3000);

            //assert: check if create new plan dialog is displayed
            Assert.IsTrue(createPlanDialog.IsCreateBudgetPlanDialogDisplayed(), "The plan name field is not found");

            BudgetPlanContract newBudgetPlan = new BudgetPlanContract()
            {
                Name = "Test",
                Description = "test",
                NoCategory = 5,
                Imagine = "https://www.svgrepo.com/download/533551/car.svg",

            };

            budgetPlanPage = createPlanDialog.AddNewPlan(newBudgetPlan);

            //wait
            Thread.Sleep(3000);

        }

        [TestMethod]
        public void Should_DeletePlan_When_UserIsLogInAsAdmin()
        {
            // go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //wait
            Thread.Sleep(3000);

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "andrei.cazamir@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var budgetPlanPage = homePage.GoToBudgetPlanPage();

            //wait
            Thread.Sleep(3000);

            //Assert: check if budget plan page is displayed
            Assert.IsTrue(budgetPlanPage.IsBudgetPlanPageDisplayed(), "The budget card is not found");

            //show only plans created by that user
            budgetPlanPage.ShowOnlyMyPlans();

            //wait
            Thread.Sleep(3000);

            //choose a plan to delete
            var title = "Test";
            var editPlanPage = budgetPlanPage.ChooseBudgetPlanToDelete(title);

            //assert: check if edit plan dialog displayed
            Assert.IsTrue(editPlanPage.IsEditBudgetPlanPageDisplayed(), "The delete button is not found");

            var deleteConfirmationDialog = editPlanPage.GoToDeleteConfimationDialog();

            //wait
            Thread.Sleep(3000);

            //assert: check if confirm deletion is dispalyed
            Assert.IsTrue(deleteConfirmationDialog.IsDeleteConfimationDialogDisplayed(), "The title is not found");

            budgetPlanPage = deleteConfirmationDialog.ConfirmDeletePlan();

            //wait
            Thread.Sleep(3000);

            //Assert: check if budget plan page is displayed
            Assert.IsTrue(budgetPlanPage.IsBudgetPlanPageDisplayed(), "The budget card is not found");
        }
    }
}
