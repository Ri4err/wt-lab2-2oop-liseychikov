using System;
using System.Collections.Generic;
using wt_lab2_2oop_liseychikov.Models;
using wt_lab2_2oop_liseychikov.Services;

Console.WriteLine("Добро пожаловать в файл С# по ЛР2");
Console.WriteLine("\nМеню:\n");
Console.WriteLine("Вывести все объекты - 1\nПоиск по ID - 2\nПоиск по имени - 3\nЗапросы и фильтрация - 4");
Console.WriteLine("Агрегатные операции - 5\n");

//Developer dev1 = new Developer(1, "Денис", "Junior", 1)
//{};

//Developer dev2 = new Developer(2, "Мария", "Senior", 2)
//{};

//int answer = int.Parse(Console.ReadLine());
//switch(answer)
//{
//    case 1:
//        dev1.PrintInfo();
//        dev2.PrintInfo();
//        Console.WriteLine("Форматированный вывод:\n");
//        Console.WriteLine($"Разработчик #{dev1.Id}: {dev1.Name} ({dev1.Position}) — {dev1.Salary:F2} руб.");
//        Console.WriteLine($"Разработчик #{dev2.Id}: {dev2.Name} ({dev2.Position}) — {dev2.Salary:F2} руб.");
//        break;
//}

IItemService<BaseEntity> service = new DeveloperService();

service.Add(new Developer(1, "Денис", "C# Developer", 2000f));
service.Add(new BackendDeveloper(2, "Мария", "Senior Backend Developer", 4000f, "PostgreSQL"));
service.Add(new FrontendDeveloper(3, "Иван", "JavaScript Developer", 3100f, "React"));
service.Add(new BackendDeveloper(4, "Алексей", "Lead Backend Developer", 5000f, "Docker / PostgreSQL"));
service.Add(new FrontendDeveloper(5, "Елена", "UI/UX Frontend Developer", 2800f, "Vue.js"));
service.Add(new Developer(6, "Олег", "Junior C# Developer", 1200f));
service.Add(new FrontendDeveloper(7, "Анна", "Angular Developer", 3500f, "Angular"));

Dictionary<int, BaseEntity> devDict = service.ToDictionary();
await service.ProcessDataAsync();
List<BaseEntity> remoteData = await service.FetchDataFromApiAsync();

Console.WriteLine($"\nУспешно получено объектов из API: {remoteData.Count}\n");

Console.WriteLine("=== 1. ПАРАЛЛЕЛЬНОЕ ВЫПОЛНЕНИЕ (Task.WhenAll) ===");
await service.RunParallelTaskAsync();

Console.WriteLine("=== 2. ОТМЕНА ОПЕРАЦИИ (CancellationTokenSource) ===");
using (CancellationTokenSource cts = new CancellationTokenSource())
{
    cts.CancelAfter(2500);

    try
    {
        await service.LongRunningTaskWithCancellationAsync(cts.Token);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Операция была успешно отменена по запросу пользователя (таймауту)!");
    }
}

int answer = int.Parse(Console.ReadLine());
switch (answer)
{
    case 1:
        foreach (BaseEntity entity in service.GetAll())
        {
            entity.PrintInfo();
            Console.WriteLine(entity.GetStatus());
            Console.WriteLine(new string('-', 50));
        }
        break;

    case 2:
        Console.Write("Введите ID: ");
        int id = int.Parse(Console.ReadLine());
        BaseEntity item = service.GetById(id);
        item?.PrintInfo();
        break;

    case 3:
        Console.Write("Введите имя: ");
        string name = Console.ReadLine();
        BaseEntity item1 = service.GetById(name);
        item1?.PrintInfo();
        break;
    case 4:
        service.ShowLinqQueries();
        break;
    case 5:
        service.ShowLinqAggregatesAndGrouping();
        break;
}