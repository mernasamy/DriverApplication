using AutoMapper;
using DriverApiApplication.Helper;
using DriverApiApplication.Models;
using DriverApiApplication.Models.Dto;
using DriverApiApplication.Repositories;
using System;
using System.Reflection;

namespace DriverApiApplication.Services
{
    public class DriverService : IDriverService
    {
        private IDriverRepository _driverRepository;
        private readonly IMapper _mapper;

        public DriverService(
            IDriverRepository driverRepository,
            IMapper mapper)
        {
            _driverRepository = driverRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// get all drivers form system
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Driver>> GetAll()
        {
            return await _driverRepository.GetAll();
        }

        /// <summary>
        /// get driver by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public async Task<Driver> GetById(int id)
        {
            var driver = await _driverRepository.GetById(id);

            if (driver == null)
                throw new KeyNotFoundException("Driver not found");

            return driver;
        }

        /// <summary>
        /// create new driver in system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        public async Task<int> Create(CreateDriver model)
        {
            // validate
            if (await _driverRepository.GetByEmail(model.Email!) != null)
                throw new AppException("Driver with the email '" + model.Email + "' already exists");

            // map model to new driver object
            var driver = _mapper.Map<Driver>(model);

            // save driver
            return await _driverRepository.Create(driver);
        }

        /// <summary>
        /// update existing driver by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        /// <exception cref="AppException"></exception>
        public async Task Update(int id, UpdateDriver model)
        {
            var driver = await _driverRepository.GetById(id);

            if (driver == null)
                throw new KeyNotFoundException("Driver not found");

            // validate
            var emailChanged = !string.IsNullOrEmpty(model.Email) && driver.Email != model.Email;
            if (emailChanged && await _driverRepository.GetByEmail(model.Email!) != null)
                throw new AppException("Driver with the email '" + model.Email + "' already exists");

            // copy model props to driver
            _mapper.Map(model, driver);

            // save driver
            await _driverRepository.Update(driver);
        }

        /// <summary>
        /// delete driver by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task Delete(int id)
        {
            var driver = await _driverRepository.GetById(id);

            if (driver == null)
                throw new KeyNotFoundException("Driver not found");

            await _driverRepository.Delete(id);
        }

        /// <summary>
        /// get all drivers ordered by first name ascending 
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Driver>> GetAllOrderedDrivers()
        {
            var drivers = await _driverRepository.GetAll();
            if (drivers == null) return null;
            return drivers.OrderBy(x => x.FirstName?.ToLower());
            //return await _driverRepository.GetAllOrderedDrivers();
        }

        /// <summary>
        /// get 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Driver> GetUserAlphabetized(int id)
        {
            try
            {
                Driver driver = await _driverRepository.GetById(id);
                if (driver == null)
                    throw new KeyNotFoundException("Driver not found");

                string sortedFirstName = driver?.FirstName?.SortCaseInsensitive();
                string sortedLastName = driver?.LastName?.SortCaseInsensitive();

                driver.FirstName = sortedFirstName;
                driver.LastName = sortedLastName;
                return driver;
            }
            catch (Exception ex)
            {
                // Handle any repository or database exceptions here
                throw new Exception($"Error fetching driver with id {id}", ex);
            }
        }

        /// <summary>
        /// create driver list
        /// </summary>
        /// <param name="models">list of drivers</param>
        /// <returns></returns>
        public async Task CreateDriverList(List<CreateDriver> models)
        {
            List<Driver> drivers = new List<Driver>();
            Driver driver = new Driver();
            foreach (var model in models)
            {
                driver = _mapper.Map<Driver>(model);
                drivers.Add(driver);
            }
            await _driverRepository.AddDriverList(drivers);
        }

        /// <summary>
        /// create random list of driver 
        /// </summary>
        /// <param name="listLength">length of list</param>
        /// <returns></returns>
        public async Task CreateRandomDriverList(int listLength)
        {
            List<Driver> drivers = new List<Driver>();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            for (int i = 0; i < listLength; i++)
            {
                Driver driver = new Driver();
                driver.FirstName = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[new Random().Next(s.Length)]).ToArray());
                driver.LastName = new string(Enumerable.Repeat(chars, 6)
                    .Select(s => s[new Random().Next(s.Length)]).ToArray());
                driver.Email = driver.LastName + "@gmail.com";
                driver.PhoneNumber = new Random().NextInt64(1000000000, 9999999999).ToString();
                drivers.Add(driver);
            }
            await _driverRepository.AddDriverList(drivers);
        }
    }
}
