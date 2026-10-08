using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wt_lab2_2oop_liseychikov.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; private set; }

        protected string GetFormattedHeader()
        {
            return $"[ID: {Id} | Создано: {CreatedAt:HH:mm:ss}]";
        }

        public BaseEntity(int id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
        }

        public abstract void PrintInfo();

        public virtual string GetStatus()
        {
            return $"{GetFormattedHeader()} Сущность активна";
        }
    }

    public class Developer : BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; private set; }
        public float Salary { get; private set; }

        public Developer(int id, string name, string position, float salary) : base(id)
        {
            Name = name;
            Position = position;

            if (salary < 0)
            {
                Console.WriteLine("Error!\n");
                Salary = 0;
            }
            else
            {
                Salary = salary;
            }
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Developer] Id: {Id} | Имя: {Name} | Должность: {Position} | Зарплата: {Salary} руб.");
        }
    }

    public class BackendDeveloper : Developer
    {
        public string PrimaryDatabase { get; set; }

        public BackendDeveloper(int id, string name, string position, float salary, string primaryDatabase)
            : base(id, name, position, salary)
        {
            PrimaryDatabase = primaryDatabase;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Backend] Id: {Id} | Имя: {Name} | Должность: {Position} | БД: {PrimaryDatabase} | Зарплата: {Salary} руб.");
        }
    }

    public class FrontendDeveloper : Developer
    {
        public string DevLang { get; set; }

        public FrontendDeveloper(int id, string name, string position, float salary, string devlang)
            : base(id, name, position, salary)
        {
            DevLang = devlang;
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Frontend] Id: {Id} | Имя: {Name} | Должность: {Position} | Язык разработки: {DevLang} | Зарплата: {Salary} руб.");
        }
    }
}