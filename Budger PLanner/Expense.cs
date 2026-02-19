using System;


namespace Budget_Planner
{
    public class Expense : MoneyRecord, ICategorizable
    {
        public string Category { get; set; }

        public Expense(decimal amount, DateTime date, string category)
            : base(amount, date)
        {
            Category = category;
        }
    }
}
