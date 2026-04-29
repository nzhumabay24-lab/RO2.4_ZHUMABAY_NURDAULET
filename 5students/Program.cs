using System;
using System.Collections.Generic;
using System.Linq;

public class Student
{
    private static int _idCounter = 100;
    public int StudentId { get; private set; }
    public string Name { get; set; }
    public string Faculty { get; set; }

    private double _gpa;
    public double GPA
    {
        get => _gpa;
        set
        {
            if (value >= 0.0 && value <= 4.0)
                _gpa = value;
            else
                throw new ArgumentException("GPA must be between 0.0 and 4.0");
        }
    }

    public Student(string name, string faculty, double gpa)
    {
        StudentId = ++_idCounter;
        Name = name;
        Faculty = faculty;
        GPA = gpa;
    }

    public override string ToString()
    {
        return $"[ID: {StudentId}] Name: {Name} | Faculty: {Faculty} | GPA: {GPA:F2}";
    }
}

public class Registry
{
    private Student[] _students = new Student[100];
    private int _count = 0;

    public void Add(Student student)
    {
        if (_count < 100)
        {
            _students[_count++] = student;
        }
    }

    public Student FindById(int id)
    {
        for (int i = 0; i < _count; i++)
        {
            if (_students[i].StudentId == id) return _students[i];
        }
        return null;
    }

    public List<Student> FindByName(string name)
    {
        List<Student> results = new List<Student>();
        for (int i = 0; i < _count; i++)
        {
            if (_students[i].Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                results.Add(_students[i]);
        }
        return results;
    }

    public List<Student> GetTopStudents(int n)
    {
        return _students.Take(_count)
                        .OrderByDescending(s => s.GPA)
                        .Take(n)
                        .ToList();
    }

    public void PrintAll()
    {
        for (int i = 0; i < _count; i++)
        {
            Console.WriteLine(_students[i]);
        }
    }
}

class Program
{
    static Registry registry = new Registry();

    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\n1. Add Student\n2. Find by ID\n3. Find by Name\n4. Top N\n5. Print All\n6. Exit");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": AddStudent(); break;
                case "2": FindById(); break;
                case "3": FindByName(); break;
                case "4": ShowTop(); break;
                case "5": registry.PrintAll(); break;
                case "6": exit = true; break;
            }
        }
    }

    static void AddStudent()
    {
        try
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();
            Console.Write("Faculty: ");
            string faculty = Console.ReadLine();
            Console.Write("GPA: ");
            double gpa = double.Parse(Console.ReadLine());
            registry.Add(new Student(name, faculty, gpa));
        }
        catch (Exception ex) { Console.WriteLine(ex.Message); }
    }

    static void FindById()
    {
        Console.Write("ID: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var s = registry.FindById(id);
            Console.WriteLine(s?.ToString() ?? "Not found");
        }
    }

    static void FindByName()
    {
        Console.Write("Name: ");
        var res = registry.FindByName(Console.ReadLine());
        if (res.Count > 0) res.ForEach(Console.WriteLine);
        else Console.WriteLine("Not found");
    }

    static void ShowTop()
    {
        Console.Write("N: ");
        if (int.TryParse(Console.ReadLine(), out int n))
        {
            var top = registry.GetTopStudents(n);
            top.ForEach(Console.WriteLine);
        }
    }
}