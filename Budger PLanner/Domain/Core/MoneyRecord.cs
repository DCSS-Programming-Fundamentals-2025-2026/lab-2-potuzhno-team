using System;

namespace Budget_Planner.Domain.Core
{
    public abstract class MoneyRecord : IComparable
    {
        public decimal Amount { get; protected set; }
        public DateTime Date { get; protected set; }
        public RecordType Type { get; protected set; }

        protected MoneyRecord(decimal amount, DateTime date, RecordType type)
        {
            Amount = amount;
            Date = date;
            Type = type;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            MoneyRecord other = obj as MoneyRecord;

            if (other != null)
                return this.Date.CompareTo(other.Date);

            throw new ArgumentException("Об'єкт не є MoneyRecord");
        }
    }
}