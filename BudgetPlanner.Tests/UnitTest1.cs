using NUnit.Framework;
using Budget_Planner.Domain.Services;
using Budget_Planner.Domain.Records;
using Budget_Planner.Domain.Core;
using Budget_Planner.Domain.Collections;
using Budget_Planner.Domain.Comparers;
using System;

namespace BudgetPlanner.Tests
{
    public class BudgetManagerTests
    {
        [Test]
        public void GetTotalIncome_WhenNoIncomes_ShouldReturnZero()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var totalIncome = manager.GetTotalIncome();

            // Assert
            Assert.AreEqual(0m, totalIncome);
        }

        [Test]
        public void GetTotalIncome_WhenMultipleIncomes_ShouldReturnCorrectSum()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddIncome(1000m, DateTime.Now, "Salary");
            manager.AddIncome(500m, DateTime.Now, "Bonus");
            manager.AddExpense(200m, DateTime.Now, "Food");

            // Act
            var totalIncome = manager.GetTotalIncome();

            // Assert
            Assert.AreEqual(1500m, totalIncome);
        }

        [Test]
        public void GetTotalExpense_WhenNoExpenses_ShouldReturnZero()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var totalExpense = manager.GetTotalExpense();

            // Assert
            Assert.AreEqual(0m, totalExpense);
        }

        [Test]
        public void GetTotalExpense_WhenMultipleExpenses_ShouldReturnCorrectSum()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(300m, DateTime.Now, "Rent");
            manager.AddExpense(150m, DateTime.Now, "Utilities");
            manager.AddIncome(2000m, DateTime.Now, "Salary");

            // Act
            var totalExpense = manager.GetTotalExpense();

            // Assert
            Assert.AreEqual(450m, totalExpense);
        }

        [Test]
        public void GetBalance_WhenIncomesAndExpenses_ShouldReturnCorrectBalance()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddIncome(2000m, DateTime.Now, "Salary");
            manager.AddExpense(500m, DateTime.Now, "Rent");
            manager.AddExpense(200m, DateTime.Now, "Food");

            // Act
            var balance = manager.GetBalance();

            // Assert
            Assert.AreEqual(1300m, balance);
        }

        [Test]
        public void GetBalance_WhenNoRecords_ShouldReturnZero()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var balance = manager.GetBalance();

            // Assert
            Assert.AreEqual(0m, balance);
        }

        [Test]
        public void GetBalance_WhenOnlyExpenses_ShouldReturnNegativeBalance()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(300m, DateTime.Now, "Rent");
            manager.AddExpense(150m, DateTime.Now, "Utilities");

            // Act
            var balance = manager.GetBalance();

            // Assert
            Assert.AreEqual(-450m, balance);

        }

        [Test]
        public void GetMinExpense_WhenNoExpenses_ShouldReturnNull()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var minExpense = manager.GetMinExpense();

            // Assert
            Assert.IsNull(minExpense);
        }

        [Test]
        public void GetMinExpense_WhenMultipleExpenses_ShouldReturnMinimumExpense()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(300m, DateTime.Now, "Rent");
            manager.AddExpense(150m, DateTime.Now, "Utilities");
            manager.AddExpense(50m, DateTime.Now, "Food");

            // Act
            var minExpense = manager.GetMinExpense();

            // Assert
            Assert.AreEqual(50m, minExpense.Amount);
        }

        [Test]
        public void GetMinExpence_WhenOnlyOneExpense_ShouldReturnThatExpense()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(200m, DateTime.Now, "Rent");

            // Act
            var minExpense = manager.GetMinExpense();

            // Assert
            Assert.AreEqual(200m, minExpense.Amount);
        }

        [Test]
        public void GetMaxExpense_WhenNoExpenses_ShouldReturnNull()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var maxExpense = manager.GetMaxExpense();

            // Assert
            Assert.IsNull(maxExpense);
        }

        [Test]
        public void GetMaxExpense_WhenMultipleExpenses_ShouldReturnMaximumExpense()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(300m, DateTime.Now, "Rent");
            manager.AddExpense(150m, DateTime.Now, "Utilities");
            manager.AddExpense(50m, DateTime.Now, "Food");

            // Act
            var maxExpense = manager.GetMaxExpense();

            // Assert
            Assert.AreEqual(300m, maxExpense.Amount);
        }

        [Test]
        public void GetMaxExpense_WhenOnlyOneExpense_ShouldReturnThatExpense()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(200m, DateTime.Now, "Rent");

            // Act
            var maxExpense = manager.GetMaxExpense();

            // Assert
            Assert.AreEqual(200m, maxExpense.Amount);
        }

        [Test]
        public void GetCategorySummary_WhenNoRecords_ShouldReturnEmptyArrays()
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
        public void GetCategorySummary_WhenMultipleRecords_ShouldReturnCorrectSummary()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(300m, DateTime.Now, "Rent");
            manager.AddExpense(150m, DateTime.Now, "Utilities");
            manager.AddExpense(50m, DateTime.Now, "Food");

            // Act
            manager.GetCategorySummary(out string[] categories, out decimal[] sums, out int count);

            // Assert
            Assert.AreEqual(3, count);
            Assert.Contains("Rent", categories);
            Assert.Contains("Utilities", categories);
            Assert.Contains("Food", categories);
            Assert.AreEqual(300m, sums[Array.IndexOf(categories, "Rent")]);
            Assert.AreEqual(150m, sums[Array.IndexOf(categories, "Utilities")]);
            Assert.AreEqual(50m, sums[Array.IndexOf(categories, "Food")]);
        }

        [Test]
        public void GetCategorySummary_WhenMultipleRecordsWithSameCategory_ShouldSumAmounts()
        {
            // Arrange
            var manager = new BudgetManager();
            manager.AddExpense(100m, DateTime.Now, "Food");
            manager.AddExpense(50m, DateTime.Now, "Food");
            manager.AddExpense(25m, DateTime.Now, "Food");

            // Act
            manager.GetCategorySummary(out string[] categories, out decimal[] sums, out int count);

            // Assert
            Assert.AreEqual(1, count);
            Assert.Contains("Food", categories);
            Assert.AreEqual(175m, sums[Array.IndexOf(categories, "Food")]);
        }

        [Test]
        public void AddIncome_WhenAmountIsZero_ShouldReturnFalse()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var result = manager.AddIncome(0m, DateTime.Now, "Salary");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddIncome_WhenAmountIsNegative_ShouldNotIncreaseCount()
        {
            // Arrange
            var manager = new BudgetManager();
            var initialCount = manager.Records.Count;

            // Act
            var result = manager.AddIncome(-100m, DateTime.Now, "Salary");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, manager.Records.Count);
        }

        [Test]
        public void AddIncome_WhenLimitExceeded_ShouldReturnFalse()
        {
            // Arrange
            var manager = new BudgetManager();

            for (int i = 0; i < 500; i++)
            {
                manager.AddIncome(1m, DateTime.Now, "Salary");
            }

            // Act
            var result = manager.AddIncome(1m, DateTime.Now, "Salary");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddExpense_WhenAmountIsZero_ShouldReturnFalse()
        {
            // Arrange
            var manager = new BudgetManager();

            // Act
            var result = manager.AddExpense(0m, DateTime.Now, "Food");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void AddExpense_WhenAmountIsNegative_ShouldNotIncreaseCount()
        {
            // Arrange
            var manager = new BudgetManager();
            var initialCount = manager.Records.Count;

            // Act
            var result = manager.AddExpense(-100m, DateTime.Now, "Food");

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(initialCount, manager.Records.Count);
        }

        [Test]
        public void AddExpense_WhenLimitExceeded_ShouldReturnFalse()
        {
            // Arrange
            var manager = new BudgetManager();

            for (int i = 0; i < 500; i++)
            {
                manager.AddExpense(1m, DateTime.Now, "Food");
            }

            // Act
            var result = manager.AddExpense(1m, DateTime.Now, "Food");

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Add_WhenCalled_ShouldIncreaseCount()
        {
            // Arrange
            var collection = new MoneyRecordCollection();
            var record = new Income(100m, DateTime.Now, "Salary");

            // Act
            collection.Add(record);

            // Assert
            Assert.AreEqual(1, collection.Count);
        }

        [Test]
        public void Add_WhenCalled_ShouldStoreCorrectElement()
        {
            // Arrange
            var collection = new MoneyRecordCollection();
            var record = new Income(100m, DateTime.Now, "Salary");

            // Act
            collection.Add(record);

            // Assert
            var stored = collection.GetAt(0);
            Assert.AreEqual(record, stored);
        }

        [Test]
        public void RemoveAt_WhenCalled_ShouldDecreaseCount()
        {
            // Arrange
            var collection = new MoneyRecordCollection();
            var record = new Income(100m, DateTime.Now, "Salary");
            collection.Add(record);

            // Act
            collection.RemoveAt(0);

            // Assert
            Assert.AreEqual(0, collection.Count);
        }

        [Test]
        public void RemoveAt_WhenCalled_ShouldShiftElementsCorrectly()
        {
            // Arrange
            var collection = new MoneyRecordCollection();
            var record1 = new Income(100m, DateTime.Now, "Salary");
            var record2 = new Expense(50m, DateTime.Now, "Food");
            collection.Add(record1);
            collection.Add(record2);

            // Act
            collection.RemoveAt(0);
            // Assert

            var stored = collection.GetAt(0);
            Assert.AreEqual(record2, stored);
        }

        [Test]
        public void RemoveAt_WhenDeleteLastElement_ShouldSetNull()
        {
            // Arrange
            var collection = new MoneyRecordCollection();
            var record1 = new Income(100m, DateTime.Now, "Salary");
            var record2 = new Expense(50m, DateTime.Now, "Food");
            collection.Add(record1);
            collection.Add(record2);

            // Act
            collection.RemoveAt(1);

            // Assert
            Assert.Throws<IndexOutOfRangeException>(() => collection.GetAt(1));
        }

        [Test]
        public void Sort_WhenCalled_ShouldSortByDateAscending()
        {
            // Arrange
            var collection = new MoneyRecordCollection();

            var record1 = new Income(100m, new DateTime(2024, 1, 1), "Salary");
            var record2 = new Expense(50m, new DateTime(2023, 12, 12), "Food");

            collection.Add(record1);
            collection.Add(record2);

            // Act
            collection.Sort();

            // Assert
            var first = collection.GetAt(0);
            var second = collection.GetAt(1);

            Assert.AreEqual(record2, first);
            Assert.AreEqual(record1, second);
        }

        [Test]
        public void Sort_WhenCalledWithAmountComparer_ShouldSortByAmountAscending()
        {
            // Arrange
            var collection = new MoneyRecordCollection();

            var record1 = new Income(100m, new DateTime(2024, 1, 1), "Salary");
            var record2 = new Expense(50m, new DateTime(2023, 12, 12), "Food");

            collection.Add(record1);
            collection.Add(record2);

            // Act
            collection.Sort(new AmountComparer());

            // Assert
            var first = collection.GetAt(0);
            var second = collection.GetAt(1);

            Assert.AreEqual(record2, first);
            Assert.AreEqual(record1, second);
        }

        [Test]
        public void Integration_FullFinancialScenario_ShouldReturnCorrectStatistics()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddIncome(2000m, DateTime.Now, "Salary");
            manager.AddIncome(500m, DateTime.Now, "Bonus");

            manager.AddExpense(300m, DateTime.Now, "Rent");
            manager.AddExpense(200m, DateTime.Now, "Food");

            // Act
            var totalIncome = manager.GetTotalIncome();
            var totalExpense = manager.GetTotalExpense();
            var balance = manager.GetBalance();

            // Assert
            Assert.AreEqual(2500m, totalIncome);
            Assert.AreEqual(500m, totalExpense);
            Assert.AreEqual(2000m, balance);
        }

        [Test]
        public void Integration_CategorySummaryAndMinExpense_ShouldReturnCorrectData()
        {
            // Arrange
            var manager = new BudgetManager();

            manager.AddExpense(300m, DateTime.Now, "Rent");
            manager.AddExpense(150m, DateTime.Now, "Food");
            manager.AddExpense(50m, DateTime.Now, "Food");
            manager.AddExpense(400m, DateTime.Now, "Utilities");

            // Act
            manager.GetCategorySummary(out string[] categories, out decimal[] sums, out int count);
            var minExpense = manager.GetMinExpense();

            // Assert
            Assert.AreEqual(3, count);

            Assert.AreEqual(300m, sums[Array.IndexOf(categories, "Rent")]);
            Assert.AreEqual(200m, sums[Array.IndexOf(categories, "Food")]);
            Assert.AreEqual(400m, sums[Array.IndexOf(categories, "Utilities")]);

            Assert.AreEqual(50m, minExpense.Amount);
        }
    }
}


