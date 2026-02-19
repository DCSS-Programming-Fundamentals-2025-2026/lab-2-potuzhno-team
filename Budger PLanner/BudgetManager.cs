using System;


namespace Budget_Planner
{
    public class BudgetManager
    {
        public MoneyRecordCollection Records { get; private set; } = new MoneyRecordCollection();

        private string[] categories = new string[30];
        private int categoryCount = 0;

        private bool EnsureCategory(string category)
        {
            for (int i = 0; i < categoryCount; i++)
            {
                if (categories[i] == category)
                {
                    return true;
                }
            }

            if (categoryCount >= categories.Length)
            {
                return false;
            }

            categories[categoryCount] = category;
            categoryCount++;
            return true;
        }

        public bool AddIncome(decimal amount, DateTime date, string category)
        {
            if (Records.Count >= 500)
            {
                return false;
            }

            if (!EnsureCategory(category))
            {
                return false;
            }

            Records.Add(new Income(amount, date, category));
            return true;
        }

        public bool AddExpense(decimal amount, DateTime date, string category)
        {
            if (Records.Count >= 500)
            {
                return false;
            }

            if (!EnsureCategory(category))
            {
                return false;
            }

            Records.Add(new Expense(amount, date, category));
            return true;
        }

        public MoneyRecord[] GetAllRecords()
        {
            MoneyRecord[] result = new MoneyRecord[Records.Count];

            for (int i = 0; i < Records.Count; i++)
            {
                result[i] = Records.GetAt(i);
            }

            return result;
        }

        public decimal GetTotalIncome()
        {
            decimal sum = 0;

            for (int i = 0; i < Records.Count; i++)
            {
                Income income = Records.GetAt(i) as Income;
                if (income != null)
                {
                    sum += income.Amount;
                }
            }

            return sum;
        }

        public decimal GetTotalExpense()
        {
            decimal sum = 0;

            for (int i = 0; i < Records.Count; i++)
            {
                Expense expense = Records.GetAt(i) as Expense;
                if (expense != null)
                {
                    sum += expense.Amount;
                }
            }

            return sum;
        }

        public decimal GetBalance()
        {
            return GetTotalIncome() - GetTotalExpense();
        }

        public Expense GetMinExpense()
        {
            Expense min = null;

            for (int i = 0; i < Records.Count; i++)
            {
                Expense expense = Records.GetAt(i) as Expense;
                if (expense != null)
                {
                    if (min == null || expense.Amount < min.Amount)
                    {
                        min = expense;
                    }
                }
            }

            return min;
        }

        public Expense GetMaxExpense()
        {
            Expense max = null;

            for (int i = 0; i < Records.Count; i++)
            {
                Expense expense = Records.GetAt(i) as Expense;
                if (expense != null)
                {
                    if (max == null || expense.Amount > max.Amount)
                    {
                        max = expense;
                    }
                }
            }

            return max;
        }

        public void GetCategorySummary(out string[] resultCategories, out decimal[] resultSums, out int resultCount)
        {
            resultCategories = new string[categoryCount];
            resultSums = new decimal[categoryCount];
            resultCount = 0;

            for (int i = 0; i < Records.Count; i++)
            {
                Expense expense = Records.GetAt(i) as Expense;
                if (expense != null)
                {
                    bool found = false;

                    for (int j = 0; j < resultCount; j++)
                    {
                        if (resultCategories[j] == expense.Category)
                        {
                            resultSums[j] += expense.Amount;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        resultCategories[resultCount] = expense.Category;
                        resultSums[resultCount] = expense.Amount;
                        resultCount++;
                    }
                }
            }
        }

    }
}
