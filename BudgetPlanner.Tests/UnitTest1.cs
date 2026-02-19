using NUnit.Framework;
using Budget_Planner.Domain.Services;
using Budget_Planner.Domain.Records;
using Budget_Planner.Domain.Core;
using System;

namespace BudgetPlanner.Tests
{
    public class BudgetManagerTests
    {

        [Test]
        public void AddIncome_WhenCategoryLimitExceeded_ShouldNotChangeTotalIncome()
        {
            // Arrange
            var manager = new BudgetManager();

            for (int i = 0; i < 30; i++)
            {
                manager.AddIncome(10, DateTime.Now, "Category" + i);
            }

            // Act
            var result = manager.AddIncome(100, DateTime.Now, "Overflow");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(300, manager.GetTotalIncome());
        }

        [Test]
        public void AddIncome_WithNegativeValue_ShouldDecreaseTotalIncome()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddIncome(100, DateTime.Now, "Salary");

            // Act
            manager.AddIncome(-50, DateTime.Now, "Adjustment");
            var totalIncome = manager.GetTotalIncome();

            // Assert
            Assert.AreEqual(50, totalIncome);
        }


        [Test]
        public void GetTotalIncome_SumOfIncome_ShouldReturnCorrectSum()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddIncome(100, DateTime.Now, "Salary");
            manager.AddIncome(50, DateTime.Now, "Freelance");

            // Act
            var totalIncome = manager.GetTotalIncome();

            // Assert
            Assert.AreEqual(150, totalIncome);
        }

        [Test]
        public void GetTotalIncome_WhenNoRecords_ShouldReturnZero()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var result = manager.GetTotalIncome();

            // Assert
            Assert.AreEqual(0, result);
        }


        [Test]
        public void GetTotalExpense_SumOfExpenses_ShouldReturnCorrectSum()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddExpense(30, DateTime.Now, "Groceries");
            manager.AddExpense(20, DateTime.Now, "Transport");

            // Act
            var totalExpense = manager.GetTotalExpense();

            // Assert
            Assert.AreEqual(50, totalExpense);

        }

        [Test]
        public void GetBalance_IncomeAndExpense_ShouldReturnCorrectBalance()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddIncome(200, DateTime.Now, "Salary");
            manager.AddExpense(50, DateTime.Now, "Groceries");

            // Act
            var balance = manager.GetBalance();

            // Assert
            Assert.AreEqual(150, balance);
        }

        [Test]
        public void GetMinExpense_WhenManyExpenses_ShouldReturnMinExpense()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddExpense(100, DateTime.Now, "Food");
            manager.AddExpense(50, DateTime.Now, "Taxi");
            manager.AddExpense(30, DateTime.Now, "Entertainment");

            // Act
            var minExpense = manager.GetMinExpense();

            // Assert
            Assert.AreEqual(30, minExpense.Amount);

        }

        [Test]
        public void GetMinExpense_WhenNoExpenses_ShouldReturnNull()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var result = manager.GetMinExpense();

            // Assert
            Assert.IsNull(result);
        }


        [Test]
        public void GetMaxExpense_WhenManyExpenses_ShouldReturnMaxExpense()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddExpense(100, DateTime.Now, "Food");
            manager.AddExpense(50, DateTime.Now, "Taxi");
            manager.AddExpense(30, DateTime.Now, "Entertainment");

            // Act
            var maxExpense = manager.GetMaxExpense();

            // Assert
            Assert.AreEqual(100, maxExpense.Amount);
        }

        [Test]
        public void GetMaxExpense_WhenNoExpenses_ShouldReturnNull()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var result = manager.GetMaxExpense();

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public void GetCategorySummary_WhenMultipleCategories_ShouldReturnCorrectSummary()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddExpense(100, DateTime.Now, "Food");
            manager.AddExpense(50, DateTime.Now, "Taxi");
            manager.AddExpense(30, DateTime.Now, "Entertainment");
            manager.AddExpense(20, DateTime.Now, "Food");

            // Act
            manager.GetCategorySummary(out string[] categories, out decimal[] sums, out int count);

            // Assert
            Assert.AreEqual(3, count);
            Assert.AreEqual("Food", categories[0]);
            Assert.AreEqual("Taxi", categories[1]);
            Assert.AreEqual("Entertainment", categories[2]);
            Assert.AreEqual(120, sums[0]);
            Assert.AreEqual(50, sums[1]);
            Assert.AreEqual(30, sums[2]);
        }

        [Test]
        public void GetCategorySummary_WhenNoRecords_ShouldReturnEmptySummary()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            manager.GetCategorySummary(out string[] categories, out decimal[] sums, out int count);

            // Assert
            Assert.AreEqual(0, count);
            Assert.IsEmpty(categories);
            Assert.IsEmpty(sums);
        }

        [Test]
        public void GetAllRecords_WhenMultipleRecords_ShouldReturnAllRecords()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddIncome(100, DateTime.Now, "Salary");
            manager.AddExpense(50, DateTime.Now, "Groceries");

            // Act
            var records = manager.GetAllRecords();

            // Assert
            Assert.AreEqual(2, records.Length);
            Assert.IsTrue(records[0] is Income);
            Assert.IsTrue(records[1] is Expense);
        }

        [Test]
        public void GetAllRecords_WhenNoRecords_ShouldReturnEmptyArray()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var records = manager.GetAllRecords();

            // Assert
            Assert.IsEmpty(records);
        }

        [Test]
        public void FullScenario_ShouldReturnCorrectFinancialResults()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddIncome(1000, DateTime.Now, "Salary");
            manager.AddIncome(500, DateTime.Now, "Freelance");
            manager.AddExpense(200, DateTime.Now, "Food");
            manager.AddExpense(100, DateTime.Now, "Taxi");
            manager.AddExpense(50, DateTime.Now, "Food");

            // Act
            var totalIncome = manager.GetTotalIncome();
            var totalExpense = manager.GetTotalExpense();
            var balance = manager.GetBalance();

            // Assert
            Assert.AreEqual(1500, totalIncome);
            Assert.AreEqual(350, totalExpense);
            Assert.AreEqual(1150, balance);
        }

        [Test]
        public void FullScenario_ShouldReturnCorrectStatistics()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddIncome(1000, DateTime.Now, "Salary");
            manager.AddIncome(500, DateTime.Now, "Freelance");
            manager.AddExpense(200, DateTime.Now, "Food");
            manager.AddExpense(100, DateTime.Now, "Taxi");
            manager.AddExpense(50, DateTime.Now, "Food");

            // Act
            var min = manager.GetMinExpense();
            var max = manager.GetMaxExpense();
            manager.GetCategorySummary(out string[] categories, out decimal[] sums, out int count);

            // Assert
            Assert.AreEqual(50, min.Amount);
            Assert.AreEqual(200, max.Amount);
            Assert.AreEqual(2, count);
        }

        [Test]
        public void FullScenario_WithOverflow_ShouldKeepStateConsistent()
        {
            // Arrange
            var manager = new BudgetManager();

            for (int i = 0; i < 500; i++)
            {
                manager.AddIncome(10, DateTime.Now, "Test");
            }

            // Act
            var result = manager.AddIncome(100, DateTime.Now, "Overflow");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(5000, manager.GetTotalIncome());
        }
    }
}


