using Budget_Planner.Domain.Core;
using System;

namespace Budget_Planner.Domain.Records
{
    public class Income : MoneyRecord, ICategorizable
    {
        public string Category { get; set; }

        public Income(decimal amount, DateTime date, string category)
            : base(amount, date, RecordType.Income)
        {
            Category = category;
        }
    }
}