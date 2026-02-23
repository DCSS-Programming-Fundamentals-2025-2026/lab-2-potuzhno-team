using Budget_Planner.Domain.Core;
using System.Collections;
namespace Budget_Planner.Domain.Collections;

public class MoneyRecordCollection : IEnumerable
{
    private MoneyRecord[] items = new MoneyRecord[500];
    private int count = 0;
    public int Count
    {
        get { return count; }
    }

    public void Add(MoneyRecord item)
    {
        if (count < items.Length)
        {
            items[count] = item;
            count++;
        }
    }

    public void RemoveAt(int index)
    {
        if (index >= 0 && index < count)
        {
            for (int i = index; i < count - 1; i++)
            {
                items[i] = items[i + 1];
            }

            items[count - 1] = null;
            count--;
        }
    }

    public MoneyRecord GetAt(int index)
    {
        if (index >= 0 && index < count)
        {
            return items[index];
        }
        throw new IndexOutOfRangeException();
    }

    public void SetAt(int index, MoneyRecord item)
    {
        if (index >= 0 && index < count)
        {
            items[index] = item;
        }
        else
        {
            throw new IndexOutOfRangeException();
        }
    }

    public IEnumerator GetEnumerator()
    {
        return new MoneyRecordEnumerator(this);
    }

    public void Sort()
    {
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = 0; j < count - i - 1; j++)
            {
                if (items[j].CompareTo(items[j + 1]) > 0)
                {
                    MoneyRecord temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                }
            }
        }
    }

    public void Sort(IComparer comparer)
    {
        for (int i = 0; i < count - 1; i++)
        {
            for (int j = 0; j < count - i - 1; j++)
            {
                if (comparer.Compare(items[j], items[j + 1]) > 0)
                {
                    MoneyRecord temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                }
            }
        }
    }
}

public class MoneyRecordEnumerator : IEnumerator
{
    private MoneyRecordCollection collection;
    private int position = -1;

    public MoneyRecordEnumerator(MoneyRecordCollection coll)
    {
        collection = coll;
    }
    public object Current
    {
        get
        {
            if (position < 0 || position >= collection.Count)
            {
                throw new InvalidOperationException();
            }
            return collection.GetAt(position);
        }
    }

    public bool MoveNext()
    {
        position++;
        return (position < collection.Count);
    }

    public void Reset()
    {
        position = -1;
    }
}