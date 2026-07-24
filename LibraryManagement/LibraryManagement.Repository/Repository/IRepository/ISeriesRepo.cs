using LibraryManagement.Models;

namespace LibraryManagement.Repository.IRepository
{
    public interface ISeriesRepo : IRepository<Series>
    {
        void Update(Series series);
    }
}
