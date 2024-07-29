using Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using TestSpendWise.Pages;
using WebApiContracts;

namespace TestSpendWise.Tests
{
    [TestClass]
    public class SpendWiseTests
    {
        private IWebDriver driver;
        

        [TestInitialize]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://localhost:4200");
        }

        [TestCleanup]
        public void CleanupTest()
        {
            driver.Quit();
        }

        [TestMethod]
        public void Should_LogInIntoAccount_When_LogInFormContainsValidData()
        {
            //go to log in page
            var homePage = new HomePage(driver);
            var header = new Header(driver);
            var logInPage = homePage.GoToLogInPage();

            //assert: user has reached correct page
            var expectedTitle = "SIGN IN";
            var actualTitle = logInPage.GetPageTitle();

            Assert.AreEqual(expectedTitle, actualTitle);

            //fill in log in form
            UserCredentialsContract1 user = new UserCredentialsContract1()
            {
                Email = "test@gmail.com",
                Password = "test123"
            };

             homePage = logInPage.FillLogInForm(user);

            //assert: check if user is log in
            Assert.IsTrue(homePage.IsUserLoggedIn(), "The log out button is not find");

            //logout
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

            var accountPage = logInPage.FillLogInForm(user);

            //assert: check if user is log in

            //Assert.IsTrue(homePage.IsUserLoggedIn(), "The log out button is not find");

            //logout
            //homePage.LogOut();
        }

        
    }
}
