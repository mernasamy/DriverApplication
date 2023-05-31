using DriverApiApplication.Models;

namespace DriverApiApplication.Repositories
{
    public interface IDriverRepository : IRepository<Driver>
    {
        Task<Driver> GetByEmail(string email);
        Task<IEnumerable<Driver>> GetAllDriversOrdered();
        Task AddDriverList(List<Driver> driver);
    }

    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<int> Create(T entity);
        Task Update(T entity);
        Task Delete(int id);
    }
}
