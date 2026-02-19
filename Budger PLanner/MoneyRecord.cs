using System;

namespace Budget_Planner
{
    public abstract class MoneyRecord
    {
        public decimal Amount { get; protected set; }
        public DateTime Date { get; protected set; }

        protected MoneyRecord(decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }
    }
}
