using System;

namespace Budget_Planner
{
    public abstract class MoneyRecord : IComparable
    {
        public decimal Amount { get; protected set; }
        public DateTime Date { get; protected set; }

        protected MoneyRecord(decimal amount, DateTime date)
        {
            Amount = amount;
            Date = date;
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            MoneyRecord other = obj as MoneyRecord;
            if(other != null)
            {
                return this.Date.CompareTo(other.Date);
            }
            else
            {
                throw new ArgumentException("Об'єкт не є MoneyRecord");
            }
        }
    }
}
