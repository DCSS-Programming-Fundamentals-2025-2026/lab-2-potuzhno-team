using Budget_Planner.Domain.Core;
using System;

namespace Budget_Planner.Domain.Records
{
    public class Expense : MoneyRecord, ICategorizable
    {
        public string Category { get; set; }

        public Expense(decimal amount, DateTime date, string category)
            : base(amount, date, RecordType.Expense)
        {
            Category = category;
        }
    }
}