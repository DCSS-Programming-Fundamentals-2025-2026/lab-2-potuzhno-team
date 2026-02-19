using System.Collections;
namespace Budget_Planner;

public class AmountComparer : IComparer
{
    public int Compare(object? x, object? y)
    {
        MoneyRecord m1 = x as MoneyRecord;
        MoneyRecord m2 = y as MoneyRecord;

        if (m1 == null && m2 == null)
        {
            return 0;
        }

        if (m1 == null)
        {
            return -1;
        }

        if (m2 == null)
        {
            return 1;
        }

        return m1.Amount.CompareTo(m2.Amount);
    }
}