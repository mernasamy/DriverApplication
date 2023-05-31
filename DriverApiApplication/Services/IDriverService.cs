using DriverApiApplication.Models;
using DriverApiApplication.Models.Dto;

namespace DriverApiApplication.Services
{
    public interface IDriverService
    {
        Task<IEnumerable<Driver>> GetAll();
        Task<Driver> GetById(int id);
        Task<int> Create(CreateDriver model);
        Task Update(int id, UpdateDriver model);
        Task Delete(int id);
        Task<IEnumerable<Driver>> GetAllOrderedDrivers();
        Task<Driver> GetUserAlphabetized(int id);
        Task CreateDriverList(List<CreateDriver> models);
        Task CreateRandomDriverList(int listLength);
    }
}
