using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AttendanceJournal
{
    public class Student
    {
        public string Name { get; }
        public Dictionary<DateTime, bool> Visits { get; } = new Dictionary<DateTime, bool>();
        public Student(string name) { Name = name; }
    }

    public class Journal
    {
        private readonly List<Student> _students = new List<Student>();

        public void AddStudent(string name)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(name), "Имя студента не должно быть пустым");
            _students.Add(new Student(name));
            Debug.WriteLine("Добавлен студент: " + name);
        }

        public void Mark(string name, DateTime date, bool present)
        {
            Student s = _students.First(x => x.Name == name);
            s.Visits[date.Date] = present;
            Debug.WriteLine($"Отметка: {name}, {date:dd.MM.yyyy}, присутствовал = {present}");
        }

        public double GetPercent(string name)
        {
            Debug.Indent();
            Student s = _students.First(x => x.Name == name);
            int total = s.Visits.Count;
            int present = s.Visits.Count(v => v.Value);
            double percent = total == 0 ? 0 : 100.0 * present / total;
            Debug.Assert(percent >= 0 && percent <= 100, "Процент вне диапазона 0..100");
            Debug.WriteLine($"Посещаемость {name}: {present} из {total} = {percent:F1}%");
            Debug.Unindent();
            return percent;
        }
    }

    class Program
    {
        static void Main()
        {
            var journal = new Journal();
            journal.AddStudent("Шевченко");
            journal.AddStudent("Иванов");
            for (int day = 1; day <= 5; day++)
            {
                DateTime date = new DateTime(2026, 11, day);
                journal.Mark("Шевченко", date, day != 3);
                journal.Mark("Иванов", date, day % 2 == 1);
            }
            double p1 = journal.GetPercent("Шевченко");
            double p2 = journal.GetPercent("Иванов");
            Console.WriteLine($"Шевченко: {p1:F1}%");
            Console.WriteLine($"Иванов: {p2:F1}%");
            Console.ReadKey();
        }
    }
}
