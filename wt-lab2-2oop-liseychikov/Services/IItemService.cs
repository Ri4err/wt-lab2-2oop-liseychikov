using System.Collections.Generic;
using System.Threading.Tasks;
namespace wt_lab2_2oop_liseychikov.Services
{
    public interface IItemService<T>
    {
        void Add(T item);
        List<T> GetAll();
        T GetById(int id);
        T GetById(string name);

        Dictionary<int, T> ToDictionary();
        bool TryGetById(int id, out T item);

        void ShowLinqQueries();
        void ShowLinqAggregatesAndGrouping();
        Task ProcessDataAsync();
        Task<List<T>> FetchDataFromApiAsync();
        Task RunParallelTaskAsync();
        Task LongRunningTaskWithCancellationAsync(CancellationToken token);

    }
}
