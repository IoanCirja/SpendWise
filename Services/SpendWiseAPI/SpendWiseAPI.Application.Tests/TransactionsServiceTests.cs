using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NSubstitute;
using Application.Services;
using Domain;

namespace SpendWiseAPI.Application.Tests
{
    public class TransactionsServiceTests
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IMonthlyPlanRepository _monthlyPlanRepository;
        private readonly IBudgetPlanRepository _budgetPlanRepository;
        private readonly IParserData _parserData;
        private readonly TransactionsService _transactionsService;
        public TransactionsServiceTests()
        {
            this._transactionsRepository=Substitute.For<ITransactionsRepository>();
            this._monthlyPlanRepository=Substitute.For<IMonthlyPlanRepository>();
            this._budgetPlanRepository=Substitute.For<IBudgetPlanRepository>();
            this._parserData = Substitute.For<IParserData>();
            this._transactionsService=new TransactionsService(this._transactionsRepository,this._monthlyPlanRepository,this._budgetPlanRepository,this._parserData);
        }
        [Fact]
        public async Task AddTransaction_Should_ReturnTrue_When_InfoIsCorrect()
        {
            //Arrange
            var transaction = new Transactions
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", 100).Returns("350,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, transaction.amount, "350,300,450,300,200,250").Returns(true);
            this._transactionsRepository.AddTransaction(transaction).Returns(true);

            //Act
            var result = await this._transactionsService.AddTransaction(transaction);

            //Assert
            result.Should().BeTrue();
        }
        [Fact]
        public async Task AddTransaction_Should_ReturnTrue_When_InfoIsCorrectButAddTransactionRepoReturnFalse()
        {
            //Arrange
            var transaction = new Transactions
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", 100).Returns("350,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, transaction.amount, "350,300,450,300,200,250").Returns(true);
            this._transactionsRepository.AddTransaction(transaction).Returns(false);

            //Act
            var result = await this._transactionsService.AddTransaction(transaction);

            //Assert
            result.Should().BeFalse();
        }
        [Fact]
        public async Task AddTransaction_Should_ReturnException_When_UpdateMoneyinMonthlyPlanReturnFalse()
        {
            //Arrange
            var transaction = new Transactions
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", 100).Returns("350,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, transaction.amount, "350,300,450,300,200,250").Returns(false);
            this._transactionsRepository.AddTransaction(transaction).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._transactionsService.AddTransaction(transaction);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Errorrs, while updating the money spend for a category", ex.Message);
        }
        [Fact]
        public async Task AddTransaction_Should_ReturnException_When_ParsingStringSpentOfCategory()
        {
            //Arrange
            var transaction = new Transactions
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", 100).Returns("");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, transaction.amount, "350,300,450,300,200,250").Returns(false);
            this._transactionsRepository.AddTransaction(transaction).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._transactionsService.AddTransaction(transaction);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("parsing number error", ex.Message);
        }
        [Fact]
        public async Task AddTransaction_Should_ReturnException_When_MonthlyPlanWithIdDoesntExist()
        {
            //Arrange
            var transaction = new Transactions
            {
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>());
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", 100).Returns("350,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, transaction.amount, "350,300,450,300,200,250").Returns(false);
            this._transactionsRepository.AddTransaction(transaction).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._transactionsService.AddTransaction(transaction);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("monthy plan id not exist", ex.Message);
        }
        [Fact]
        public async Task DeleteTransaction_Should_ReturnTrue_When_InfoIsCorrect()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransaction(transaction.transaction_id).Returns(new List<Transactions>()
            {
                transaction
            });
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", -100).Returns("150,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, -transaction.amount, "150,300,450,300,200,250").Returns(true);
            this._transactionsRepository.DeleteTransactions(transaction.transaction_id).Returns(true);

            //Act
            var result = await this._transactionsService.DeleteTransactions(transaction.transaction_id);

            //Assert
            result.Should().BeTrue();
        }
        public async Task DeleteTransaction_Should_ReturnTrue_When_InfoIsCorrectButDeleteransactionRepoReturnFalse()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransaction(transaction.transaction_id).Returns(new List<Transactions>()
            {
                transaction
            });
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", -100).Returns("150,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, -transaction.amount, "150,300,450,300,200,250").Returns(true);
            this._transactionsRepository.DeleteTransactions(transaction.transaction_id).Returns(false);

            //Act
            var result = await this._transactionsService.DeleteTransactions(transaction.transaction_id);

            //Assert
            result.Should().BeFalse();
        }
        [Fact]
        public async Task DeleteTransaction_Should_ReturnException_When_UpdateMoneyinMonthlyPlanReturnFalse()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransaction(transaction.transaction_id).Returns(new List<Transactions>()
            {
                transaction
            });
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", -100).Returns("150,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, -transaction.amount, "150,300,450,300,200,250").Returns(false);
            this._transactionsRepository.DeleteTransactions(transaction.transaction_id);

            //Act
            Func<Task<bool>> act = async () => await this._transactionsService.DeleteTransactions(transaction.transaction_id);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("Errorrs, while updating the money spend for a category", ex.Message);
        }
        [Fact]
        public async Task DeleteTransaction_Should_ReturnException_When_ParsingStringSpentOfCategory()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransaction(transaction.transaction_id).Returns(new List<Transactions>()
            {
                transaction
            });
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>()
            {
                new MonthlyPlanGet
                {
                    monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    user_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                    plan_name = "Family Plan",
                    description = "description",
                    noCategory = 6,
                    category = "Food,Travel,Health,Pet,School,Rent",
                    date = DateTime.Parse("2024-07-29 18:32:00"),
                    totalAmount = 3500,
                    amountSpent = 1750,
                    status = "In Progress",
                    priceByCategory = "500,750,750,550,450,500",
                    spentOfCategory = "250,300,450,300,200,250"
                }
            });
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", -100).Returns("");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, -transaction.amount, "150,300,450,300,200,250").Returns(false);
            this._transactionsRepository.DeleteTransactions(transaction.transaction_id).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._transactionsService.DeleteTransactions(transaction.transaction_id);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("parsing number error", ex.Message);
        }
        [Fact]
        public async Task DeleteTransaction_Should_ReturnException_When_MonthlyPlanWithIdDoesntExist()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransaction(transaction.transaction_id).Returns(new List<Transactions>()
            {
                transaction
            });
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>());
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", -100).Returns("150,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, -transaction.amount, "150,300,450,300,200,250").Returns(false);
            this._transactionsRepository.DeleteTransactions(transaction.transaction_id).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._transactionsService.DeleteTransactions(transaction.transaction_id);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("monthy plan id not exist", ex.Message);
        }
        [Fact]
        public async Task DeleteTransaction_Should_ReturnException_When_TransactionDoesntExist()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransaction(transaction.transaction_id).Returns(new List<Transactions>());
            this._monthlyPlanRepository.GetMonthlyPlanFromHistory(transaction.monthlyPlan_id).Returns(new List<MonthlyPlanGet>());
            this._parserData.GetUpdatedStringSpentOfCategoryWhenAddTransaction("Food,Travel,Health,Pet,School,Rent", "250,300,450,300,200,250", "Food", -100).Returns("150,300,450,300,200,250");
            this._monthlyPlanRepository.UpdateMonthlyPlanWithTransaction(transaction.monthlyPlan_id, -transaction.amount, "350,300,450,300,200,150").Returns(false);
            this._transactionsRepository.DeleteTransactions(transaction.transaction_id).Returns(true);

            //Act
            Func<Task<bool>> act = async () => await this._transactionsService.DeleteTransactions(transaction.transaction_id);

            //Assert
            Exception ex = await Assert.ThrowsAsync<Exception>(() => act());
            Assert.Equal("errors: transaction id not found", ex.Message);
        }
        [Fact]
        public async Task GetAllTransactions_Should_ReturnList_When_InfoIsCorrect()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("B13EA2C5-5A9E-489B-91D9-EB3703EDA4C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            var transaction1 = new Transactions
            {
                transaction_id = Guid.Parse("B13DA2E5-3A0E-949C-96F9-EC3743ADA0C8"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetAllTransactions(transaction.monthlyPlan_id).Returns(new List<Transactions>()
            {
                transaction,
                transaction1
            });

            //Act
            var result =  this._transactionsService.GetAllTransactions(transaction.monthlyPlan_id);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(2);
            result.FirstOrDefault().Should().Be(transaction);
        }
        [Fact]
        public async Task GetAllTransactions_Should_ReturnEmptyList_When_DoesntExistTransaction()
        {
            //Arrange
            var monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7");

            this._transactionsRepository.GetAllTransactions(monthlyPlan_id).Returns(new List<Transactions>());

            //Act
            var result = this._transactionsService.GetAllTransactions(monthlyPlan_id);

            //Assert
            result.Should().BeEmpty();
            result.Count().Should().Be(0);
        }
        [Fact]
        public async Task GetTransaction_Should_ReturnList_When_InfoIsCorrect()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("B13EA2F5-5A9E-489B-91D9-EB3703EDA4C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransaction(transaction.transaction_id).Returns(new List<Transactions>()
            {
                transaction
            });

            //Act
            var result = this._transactionsService.GetTransaction(transaction.transaction_id);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(1);
            result.FirstOrDefault().Should().Be(transaction);
        }
        [Fact]
        public async Task GetTransaction_Should_ReturnEmptyList_When_DoesntExistTransaction()
        {
            //Arrange
            var transaction_id = Guid.Parse("B13EA2F5-5A9E-489B-91D9-EB3703EDA4C7");

            this._transactionsRepository.GetTransaction(transaction_id).Returns(new List<Transactions>());

            //Act
            var result = this._transactionsService.GetTransaction(transaction_id);

            //Assert
            result.Should().BeEmpty();
            result.Count().Should().Be(0);
        }
        [Fact]
        public async Task GetTransactionForCategory_Should_ReturnList_When_InfoIsCorrect()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("B13EA2F5-5A9E-489B-91D9-EB3703EDA4C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            this._transactionsRepository.GetTransactionsForCategory(transaction.category).Returns(new List<Transactions>()
            {
                transaction
            });

            //Act
            var result = this._transactionsService.GetTransactionForCategory(transaction.category);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(1);
            result.FirstOrDefault().Should().Be(transaction);
        }
        [Fact]
        public async Task GetTransactionForCategory_Should_ReturnEmptyList_When_DoesntExistTransaction()
        {
            //Arrange
            var category = "Food";

            this._transactionsRepository.GetTransactionsForCategory(category).Returns(new List<Transactions>());

            //Act
            var result = this._transactionsService.GetTransactionForCategory(category);

            //Assert
            result.Should().BeEmpty();
            result.Count().Should().Be(0);
        }
        [Fact]
        public async Task GetAllTransactionsForUser_Should_ReturnList_When_InfoIsCorrect()
        {
            //Arrange
            var transaction = new Transactions
            {
                transaction_id = Guid.Parse("B13EA2F5-5A9E-489B-91D9-EB3703EDA4C7"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            var transaction1 = new Transactions
            {
                transaction_id = Guid.Parse("B13DA2E5-3A0E-949C-96A9-EE3743EDA0C8"),
                monthlyPlan_id = Guid.Parse("C13DA2D5-4A1E-439B-96D9-EB3743EDA0C7"),
                name = "Pizza",
                date = DateTime.Parse("2024-07-29 18:32:00"),
                category = "Food",
                amount = 100
            };
            var user_id = Guid.Parse("A13DA2E5-3A0E-949C-96A9-EE3743CDA0C8");
            this._transactionsRepository.GetAllTransactionsForUser(user_id).Returns(new List<Transactions>()
            {
                transaction,
                transaction1
            });

            //Act
            var result = this._transactionsService.GetAllTransactionForUser(user_id);

            //Assert
            result.Should().NotBeNullOrEmpty();
            result.Count.Should().Be(2);
            result.FirstOrDefault().Should().Be(transaction);
        }
        [Fact]
        public async Task GetAllTransactionsForUser_Should_ReturnEmptyList_When_DoesntExistTransaction()
        {
            //Arrange
            var user_id = Guid.Parse("A13DA2E5-3A0E-949C-9BD9-ED3743FDA0C8");

            this._transactionsRepository.GetAllTransactionsForUser(user_id).Returns(new List<Transactions>());

            //Act
            var result = this._transactionsService.GetAllTransactionForUser(user_id);

            //Assert
            result.Should().BeEmpty();
            result.Count().Should().Be(0);
        }
    }
}
