using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wt_lab2_2oop_liseychikov.Models;

namespace wt_lab2_2oop_liseychikov.Services
{
    public class DeveloperService : IItemService<BaseEntity>
    {
        private readonly List<BaseEntity> _items = new List<BaseEntity>();

        public void Add(BaseEntity item)
        {
            _items.Add(item);
        }

        public List<BaseEntity> GetAll()
        {
            return _items;
        }

        public BaseEntity GetById(int id)
        {
            return _items.Find(item => item.Id == id);
        }

        public BaseEntity GetById(string name)
        {
            return _items.Find(item => item is Developer dev && dev.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Dictionary<int, BaseEntity> ToDictionary()
        {
            Dictionary<int,BaseEntity> dict= new Dictionary<int, BaseEntity>();
            foreach (BaseEntity item in _items)
            {
                dict[item.Id] = item;
            }
            return dict;
        }

        public bool TryGetById(int id, out BaseEntity item)
        {
            Dictionary<int, BaseEntity> dict=ToDictionary();
            return dict.TryGetValue(id, out item);
        }

        public void ShowLinqQueries()
        {
            Console.WriteLine("=== 1. LINQ: Фильтрация, Сортировка и Проекция ===");

            var filteredDevelopers = _items
                .OfType<Developer>()
                .Where(dev => dev.Salary > 2500f)
                .OrderByDescending(dev => dev.Salary)
                .Select(dev => new { dev.Name, dev.Position, dev.Salary });

            foreach (var dev in filteredDevelopers)
            {
                Console.WriteLine($"Имя: {dev.Name} | Должность: {dev.Position} | Зарплата: {dev.Salary} руб.");
            }
        }

        public void ShowLinqAggregatesAndGrouping()
        {
            Console.WriteLine("\n=== 2. LINQ: Агрегатные операции и Группировка ===");

            var developers = _items.OfType<Developer>().ToList();

            if (!developers.Any()) return;

            int count = developers.Count();
            float totalSalary = developers.Sum(dev => dev.Salary);
            double avgSalary = developers.Average(dev => dev.Salary);
            float minSalary = developers.Min(dev => dev.Salary);
            float maxSalary = developers.Max(dev => dev.Salary);

            Console.WriteLine($"Всего разработчиков: {count}");
            Console.WriteLine($"Суммарная зарплата: {totalSalary} руб.");
            Console.WriteLine($"Средняя зарплата: {avgSalary:F2} руб.");
            Console.WriteLine($"Минимальная зарплата: {minSalary} руб.");
            Console.WriteLine($"Максимальная зарплата: {maxSalary} руб.");

            Console.WriteLine("\n--- Группировка по специализации ---");
            var grouped = _items.GroupBy(item => item.GetType().Name);

            foreach (var group in grouped)
            {
                Console.WriteLine($"\nКатегория: {group.Key} (Количество: {group.Count()})");
                foreach (var item in group)
                {
                    item.PrintInfo();
                }
            }
        }
        public async Task ProcessDataAsync()
        {
            Console.WriteLine("Начало фоновой обработки...");
            await Task.Delay(1000);
            Console.WriteLine("Обработка завершена!\n");
        }

        public async Task<List<BaseEntity>> FetchDataFromApiAsync()
        {
            Console.WriteLine("Подключение к БД...");
            try
            {
                await Task.Delay(1500);
                return new List<BaseEntity>(_items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при прлучении данных");
                return new List<BaseEntity>();
            }
            finally
            {
                Console.WriteLine("Запрос к БД завершен");
            }
        }

        public async Task RunParallelTaskAsync()
        {
            Console.WriteLine("Запуск параллельных задач...");

            Task task1 = Task.Run(async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("[Задача 1] Данные о зарплатах загружены.");
            });

            Task task2 = Task.Run(async () =>
            {
                await Task.Delay(1500);
                Console.WriteLine("[Задача 2] Данные о стеке технологий загружены.");
            });

            Task task3 = Task.Run(async () =>
            {
                await Task.Delay(800);
                Console.WriteLine("[Задача 3] Статистика проекта обновлена.");
            });

            await Task.WhenAll(task1, task2, task3);

            Console.WriteLine("Все параллельные задачи успешно выполнены!\n");
        }
        public async Task LongRunningTaskWithCancellationAsync(CancellationToken token)
        {
            Console.WriteLine("Запуск длительного процесса с возможностью отмены...");

            for (int i = 1; i <= 5; i++)
            {
                token.ThrowIfCancellationRequested();

                Console.WriteLine($"Шаг {i}/5 выполняется...");
                await Task.Delay(1000, token);
            }

            Console.WriteLine("Длительный процесс успешно завершен!");
        }
    }
}