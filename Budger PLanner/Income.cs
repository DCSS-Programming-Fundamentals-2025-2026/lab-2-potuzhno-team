using System;


namespace Budget_Planner
{
    public class Income : MoneyRecord, ICategorizable
    {
        public string Category { get; set; }

        public Income(decimal amount, DateTime date, string category)
            : base(amount, date)
        {
            Category = category;
        }
    }
}
