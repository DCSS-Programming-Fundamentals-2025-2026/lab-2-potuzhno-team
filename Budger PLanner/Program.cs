using Budget_Planner.Domain.Core;
using Budget_Planner.Domain.Records;
using Budget_Planner.Domain.Services;
using Budget_Planner.Domain.Comparers;
using Budget_Planner.Domain.Collections;
using System;
using System.Text;

namespace Budget_Planner
{
    class Program
    {
        static BudgetManager manager = new BudgetManager();

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            bool running = true;

            while (running)
            {
                Console.Clear();
                Console.WriteLine("=== ОБЛІК БЮДЖЕТУ ===");
                Console.WriteLine("1. Додати дохід");
                Console.WriteLine("2. Додати витрату");
                Console.WriteLine("3. Список операцій");
                Console.WriteLine("4. Підсумок по категоріях");
                Console.WriteLine("5. Баланс та статистика");
                Console.WriteLine("6. Сортувати записи");
                Console.WriteLine("0. Вихід");
                Console.Write("\nВаш вибір: ");

                string input = Console.ReadLine();

                if (input == "1")
                {
                    AddRecord(true);
                }
                else if (input == "2")
                {
                    AddRecord(false);
                }
                else if (input == "3")
                {
                    ShowAll();
                }
                else if (input == "4")
                {
                    ShowCategories();
                }
                else if (input == "5")
                {
                    ShowStats();
                }
                else if (input == "6")
                {
                    SortRecords();
                }
                else if (input == "0")
                {
                    running = false;
                }
                else
                {
                    Console.WriteLine("Помилка! Спробуйте ще раз.");
                    Pause();
                }
            }
        }

        static void AddRecord(bool isIncome)
        {
            Console.Clear();
            Console.Write("Сума: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            Console.Write("Дата (дд.мм.рррр) або Enter: ");
            string dateInput = Console.ReadLine();
            DateTime date;

            if (dateInput == "")
            {
                date = DateTime.Now;
            }
            else
            {
                date = DateTime.Parse(dateInput);
            }

            Console.Write("Категорія: ");
            string category = Console.ReadLine();

            bool result;
            if (isIncome)
            {
                result = manager.AddIncome(amount, date, category);
            }
            else
            {
                result = manager.AddExpense(amount, date, category);
            }

            if (result)
            {
                Console.WriteLine("Успішно додано!");
            }
            else
            {
                Console.WriteLine("Помилка: ліміт вичерпано.");
            }

            Pause();
        }

        static void ShowAll()
        {
            Console.Clear();
            if (manager.Records.Count == 0)
            {
                Console.WriteLine("Записів немає.");
            }
            else
            {
                Console.WriteLine("Дата | Тип | Сума | Категорія");

                var it = manager.Records.GetEnumerator();
                while (it.MoveNext())
                {
                    MoneyRecord item = (MoneyRecord)it.Current;
                    string type = item.Type == RecordType.Income ? "Дохід" : "Витрата";
                    ICategorizable categorizable = item as ICategorizable;
                    string cat = (categorizable != null) ? categorizable.Category : "---";

                    Console.WriteLine(item.Date.ToString("dd.MM.yyyy") + " | " + type + " | " + item.Amount + " | " + cat);
                }
            }
            Pause();
        }

        static void ShowCategories()
        {
            Console.Clear();
            string[] cats;
            decimal[] sums;
            int count;

            manager.GetCategorySummary(out cats, out sums, out int resultCount);
            count = resultCount;

            if (count == 0)
            {
                Console.WriteLine("Дані відсутні.");
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine(cats[i] + ": " + sums[i]);
                }
            }
            Pause();
        }

        static void ShowStats()
        {
            Console.Clear();
            Console.WriteLine("Доходи: " + manager.GetTotalIncome());
            Console.WriteLine("Витрати: " + manager.GetTotalExpense());
            Console.WriteLine("Баланс: " + manager.GetBalance());
            Console.WriteLine("-------------------------");

            Expense min = manager.GetMinExpense();
            Expense max = manager.GetMaxExpense();

            if (min != null)
            {
                Console.WriteLine("Min витрата: " + min.Amount + " (" + min.Category + ")");
            }

            if (max != null)
            {
                Console.WriteLine("Max витрата: " + max.Amount + " (" + max.Category + ")");
            }

            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть Enter...");
            Console.ReadLine();
        }

        static void SortRecords()
        {
            Console.Clear();
            if (manager.Records.Count == 0)
            {
                Console.WriteLine("Немає записів для сортування.");
                Pause();
                return;
            }

            Console.WriteLine("Виберіть тип сортування:");
            Console.WriteLine("1. За датою");
            Console.WriteLine("2. За сумою");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                manager.Records.Sort();
                Console.WriteLine("Записи відсортовано за датою.");
            }
            else if (choice == "2")
            {
                manager.Records.Sort(new AmountComparer());
                Console.WriteLine("Записи відсортовано за сумою.");
            }
            else
            {
                Console.WriteLine("Невірний вибір.");
            }

            Pause();
        }
    }
}