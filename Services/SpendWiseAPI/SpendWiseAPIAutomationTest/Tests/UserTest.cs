using Domain;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SpendWiseAPIAutomationTest.Pages;
using System;
using TestSpendWise.Pages;
using WebApiContracts;

namespace TestSpendWise.Tests
{
    [TestClass]
    public class UserTests
    {
        private IWebDriver driver;


        [TestInitialize]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:4200");
        }

        /*        [TestCleanup]
                public void CleanupTest()
                {
                    driver.Quit();
                }*/

        [TestMethod]
        public void Should_LogInIntoAccount_When_LogInFormContainsValidData()
        {
            //go to log in page
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
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };


            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            //wait
            Thread.Sleep(3000);

            //logout
            homePage.LogOut();

        }

        [TestMethod]
        public void Should_GoToDashboard_When_UserIsLoggedIn()
        {
            //go to log in page
            var homePage = new HomePage(driver);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            homePage.GoToDashboard();
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");
        }

        [TestMethod]
        public void Should_AddTransaction_When_ButtonIsPressed()
        {
            //go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var dashboardPage = homePage.GoToDashboard();

            //wait
            Thread.Sleep(3000);

            //assert: check if dashboard page is disaplyed
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");

            var currentPlanPage = dashboardPage.GoToCurrentPlanPage();

            var transactionPage = currentPlanPage.GoToTransactionPage();

            //wait
            Thread.Sleep(3000);

            //asset: check if transaction page is displayed
            Assert.IsTrue(transactionPage.IsTransactionPageDisplayed(), "The transaction name is not found");


            TransactionsContract transaction = new TransactionsContract()
            {
                name = "tomatos",
                category = "Food",
                amount = 2.0
            };

            currentPlanPage = transactionPage.FillInTransactionForm(transaction);

        }

        [TestMethod]
        public void Should_ViewCategoryDetails_When_ViewCategoryButtonIsPressed()
        {
            //go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var currentPlanPage = homePage.GoToDashboard();

            //wait
            Thread.Sleep(3000);

            //assert: check if dashboard page is disaplyed
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");

            var categoryDetailsPage = currentPlanPage.GoToCategoryDetailsPage();

            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");
        }

        [TestMethod]
        public void Should_ReturnToCurrentPlanPage_When_RetrunButtonIsPressed()
        {
            //go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var currentPlanPage = homePage.GoToDashboard();

            //wait
            Thread.Sleep(3000);

            //assert: check if dashboard page is disaplyed
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");

            var categoryDetailsPage = currentPlanPage.GoToCategoryDetailsPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");

            categoryDetailsPage.ReturnToCurrentPlan();

            //assert: check if returned to current plan
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");
        }

        [TestMethod]
        public void Should_FilterCategory_When_FilterIsSelected()
        {
            //go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var currentPlanPage = homePage.GoToDashboard();

            //wait
            Thread.Sleep(3000);

            //assert: check if dashboard page is disaplyed
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");

            var categoryDetailsPage = currentPlanPage.GoToCategoryDetailsPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");

            TransactionsContract filter = new TransactionsContract()
            {
                category = "Food"
            };

            categoryDetailsPage.FilterByCategory(filter);
            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");

        }

        [TestMethod]
        public void Should_SortTransactionsDateAscending_When_SortMethodIsSelected()
        {
            //go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var currentPlanPage = homePage.GoToDashboard();

            //wait
            Thread.Sleep(3000);

            //assert: check if dashboard page is disaplyed
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");

            var categoryDetailsPage = currentPlanPage.GoToCategoryDetailsPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");

            categoryDetailsPage.SortByDateAscending();
            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");

        }

        [TestMethod]
        public void Should_SortTransactionsDateDecending_When_SortMethodIsSelected()
        {
            //go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
                Password = "Password123"
            };

            homePage = logInPage.FillLogInForm(user);

            //wait
            Thread.Sleep(3000);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            var currentPlanPage = homePage.GoToDashboard();

            //wait
            Thread.Sleep(3000);

            //assert: check if dashboard page is disaplyed
            Assert.IsTrue(homePage.IsDashboardDisplayed(), "The dashboard button is not found");

            var categoryDetailsPage = currentPlanPage.GoToCategoryDetailsPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");

            categoryDetailsPage.SortByDateDescending();

            //assert: check if return button is displayed
            Assert.IsTrue(categoryDetailsPage.IsReturnButtonDisplayed(), "The return button is not found");

        }

        [TestMethod]
        public void Should_ChangePassword_When_ChangePaswordButtonIsPressed()
        {
            // go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
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

            var accountSettingsPage = dasboardPage.GoToAccountSettingsPage();

            //assert: check if account settings page is displayed
            Assert.IsTrue(accountSettingsPage.IsAccountSettingsDisplayed(), "The change password button is not found");

            PasswordReset resetPassword = new PasswordReset()
            {
                CurrentPassword = user.Password,
                NewPassword = "Password123",
                ConfirmNewPassword = "Password123"
            };

            accountSettingsPage.ChangePassword(resetPassword);

            //wait
            Thread.Sleep(3000);

            // assert: check if account settings page is displayed
            Assert.IsTrue(accountSettingsPage.IsAccountSettingsDisplayed(), "The change password button is not found");
        }

        [TestMethod]
        public void Should_GoToHistory_When_HistoryButtonISPressed()
        {
            // go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
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

            var historyPage = dasboardPage.GoToHistoryPage();

            //assert: check if hitory pager is displayed
            Assert.IsTrue(historyPage.IsHistoryPageDisplayed(), "The container in not found");
        }

        [TestMethod]
        public void Should_ViewPastPlan_When_PlanButtonIsPresseed()
        {
            // go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
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

            var historyPage = dasboardPage.GoToHistoryPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if hitory pager is displayed
            Assert.IsTrue(historyPage.IsHistoryPageDisplayed(), "The container in not found");

            string pastPlan = "Family Plan";
            historyPage.ViewPastPlan(pastPlan);

        }

        [TestMethod]
        public void Should_ExportPastPlan_When_ExportButtonIsPresseed()
        {
            // go to log in page
            var homePage = new HomePage(driver);

            //wait
            Thread.Sleep(3000);

            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "ioan.cirja@student.tuiasi.ro",
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

            var historyPage = dasboardPage.GoToHistoryPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if hitory pager is displayed
            Assert.IsTrue(historyPage.IsHistoryPageDisplayed(), "The container in not found");

            string pastPlan = "Family Plan";
            historyPage.ViewPastPlan(pastPlan);

            //wait
            Thread.Sleep(3000);

            //assert: check if export button is displayed
            Assert.IsTrue(historyPage.IsPastPlanDisplayed(), "The export button is not found");

            historyPage.ExportPlan();

        }

        [TestMethod]
        public void Should_ViewStatistics_When_StatisticsButtonIsPressed()
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
                Email = "ioan.cirja@student.tuiasi.ro",
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

            var statisticsPage = dasboardPage.GoToStatisticsPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if statistics page is displayed
            Assert.IsTrue(statisticsPage.IsStatisticsPageDisplayed(), "The statistics card is not found");
        }

        [TestMethod]
        public void Should_ChoosePlan_When_UserDoesNotHaveActivePlan()
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
                Email = "ioan.cirja@student.tuiasi.ro",
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

            var currentPlanPage = dasboardPage.GoToCurrentPlanPage();
            //wait
            Thread.Sleep(3000);

            Assert.IsTrue(currentPlanPage.IsCurrentPlanPageDisplayed(), "The container is not found");

            //check if user has active plans
            bool activePlan = currentPlanPage.IsPlanActive();

            //got to budget plan page
            var budgetPlanPage = homePage.GoToBudgetPlanPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if budget plans page in displayed
            Assert.IsTrue(budgetPlanPage.IsBudgetPlanPageDisplayed(), "The budget card is not found");

            //choose a plan
            string planTitle = "Travel Plan";

            //budget plan dialog
            var budgetPlanDialog = budgetPlanPage.ChoseBudgetPlan(planTitle);

            //wait
            Thread.Sleep(3000);

            //assert: check if budget dialog page is displayed
            Assert.IsTrue(budgetPlanDialog.IsBudgetPlanDialogDisplayed(), "The categories inputs are not found");

            currentPlanPage = budgetPlanDialog.FillBudgetPlanDialog();

            //wait
            Thread.Sleep(3000);
        }

        [TestMethod]
        public void Should_CancelPlan_When_UserHasAnActivePlan()
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
                Email = "ioan.cirja@student.tuiasi.ro",
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

            var currentPlanPage = dasboardPage.GoToCurrentPlanPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if current plan page is displayed
            Assert.IsTrue(currentPlanPage.IsCurrentPlanPageDisplayed(), "The container is not found");

            //check if user has active plans
            bool activePlan = currentPlanPage.IsPlanActive();

            //cancel plan
            var cancelPlanDialog = currentPlanPage.CancelCurrentPlan();

            //wait
            Thread.Sleep(3000);

            //assert: check if cancel plan dialog is displayed
            Assert.IsTrue(cancelPlanDialog.IsCancelPlanDialogDisplayed(), "The confirm button is not found");

            cancelPlanDialog.ConfirmCancelCurrentPlan();

            //wait
            Thread.Sleep(3000);

            //assert: check if current plan page is displayed
            Assert.IsTrue(currentPlanPage.IsCurrentPlanPageDisplayed(), "The container is not found");

        }

        [TestMethod]
        public void DemoTest()
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
                Email = "sabina-nadejda.barila@student.tuiasi.ro",
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

            var currentPlanPage = dasboardPage.GoToCurrentPlanPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if current plan page is displayed
            Assert.IsTrue(currentPlanPage.IsCurrentPlanPageDisplayed(), "The container is not found");

            //check if user has active plans
            bool activePlan = currentPlanPage.IsPlanActive();

            //got to budget plan page
            var budgetPlanPage = homePage.GoToBudgetPlanPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if budget plans page in displayed
            Assert.IsTrue(budgetPlanPage.IsBudgetPlanPageDisplayed(), "The budget card is not found");

            //choose a plan
            string planTitle = "Travel Plan";

            //budget plan dialog
            var budgetPlanDialog = budgetPlanPage.ChoseBudgetPlan(planTitle);

            //wait
            Thread.Sleep(3000);

            //assert: check if budget dialog page is displayed
            Assert.IsTrue(budgetPlanDialog.IsBudgetPlanDialogDisplayed(), "The categories inputs are not found");

            currentPlanPage = budgetPlanDialog.FillBudgetPlanDialog();

            //wait
            Thread.Sleep(3000);

            //currentPlanPage = dasboardPage.GoToCurrentPlanPage();

            var transactionPage = currentPlanPage.GoToTransactionPage();

            //wait
            Thread.Sleep(3000);

            //asset: check if transaction page is displayed
            Assert.IsTrue(transactionPage.IsTransactionPageDisplayed(), "The transaction name is not found");


            TransactionsContract transaction = new TransactionsContract()
            {
                name = "tomatos",
                category = "Food",
                amount = 2.0
            };

            currentPlanPage = transactionPage.FillInTransactionForm(transaction);

            transactionPage = currentPlanPage.GoToTransactionPage();

            //wait
            Thread.Sleep(3000);

            //asset: check if transaction page is displayed
            Assert.IsTrue(transactionPage.IsTransactionPageDisplayed(), "The transaction name is not found");


            TransactionsContract transaction1 = new TransactionsContract()
            {
                name = "dress",
                category = "Clothes",
                amount = 20.0
            };

            currentPlanPage = transactionPage.FillInTransactionForm(transaction1);

            var categoryDetailsPage = dasboardPage.GoToCategoryDetailsPage();

            //wait
            Thread.Sleep(3000);

            categoryDetailsPage.FilterByCategory(transaction);

            //wait
            Thread.Sleep(3000);

            currentPlanPage = categoryDetailsPage.ReturnToCurrentPlan();

            //wait
            Thread.Sleep(3000);

            //assert: check if current plan page is displayed
            Assert.IsTrue(currentPlanPage.IsCurrentPlanPageDisplayed(), "The container is not found");

            var statisticsPage = dasboardPage.GoToStatisticsPage();

            //wait
            Thread.Sleep(3000);

            //assert: check if statistics page is displayed
            Assert.IsTrue(statisticsPage.IsStatisticsPageDisplayed(), "The statistics card is not found");

            var historyPage = dasboardPage.GoToHistoryPage();

            //assert: check if hitory pager is displayed
            Assert.IsTrue(historyPage.IsHistoryPageDisplayed(), "The container in not found");

            //wait
            Thread.Sleep(3000);

            currentPlanPage = dasboardPage.GoToCurrentPlanPage();

            //check if user has active plans
            activePlan = currentPlanPage.IsPlanActive();

            //cancel plan
            var cancelPlanDialog = currentPlanPage.CancelCurrentPlan();

            //wait
            Thread.Sleep(3000);

            //assert: check if cancel plan dialog is displayed
            Assert.IsTrue(cancelPlanDialog.IsCancelPlanDialogDisplayed(), "The confirm button is not found");

            cancelPlanDialog.ConfirmCancelCurrentPlan();

            //wait
            Thread.Sleep(3000);

            //assert: check if current plan page is displayed
            Assert.IsTrue(currentPlanPage.IsCurrentPlanPageDisplayed(), "The container is not found");

            homePage.LogOut();

        }

        [TestMethod]
        public void Should_FailLogInIntoAccount_When_LogInFormContainsInvalidData()
        {
            //go to log in page
            var homePage = new HomePage(driver);
            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "simona@gmail.com",
                Password = "12345678"
            };

            homePage = logInPage.FillLogInForm(user);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The first page is not find");

            //logout
            homePage.LogOut();
        }
    }
}
