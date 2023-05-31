using DriverApiApplication.Helper;
using DriverApiApplication.Models;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Reflection.PortableExecutable;

namespace DriverApiApplication.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private DbSettings _dbSettings;

        public DriverRepository(IOptions<DbSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        /// <summary>
        /// get all drivers from system
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Driver>> GetAll()
        {
            try
            {
                string sql = "SELECT * FROM Drivers";
                return await SqlHelper.ExecuteReaderAsync(sql, _dbSettings.DefaultConnection, DriverTranslator.TranslateDriver);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// get driver by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>driver object</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Driver> GetById(int id)
        {
            try
            {
                string sql = $"SELECT * FROM Drivers WHERE Id = '{id}'";
                var result = await SqlHelper.ExecuteReaderAsync(sql, _dbSettings.DefaultConnection, DriverTranslator.TranslateDriver);
                return result?.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// get driver by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>driver object</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Driver> GetByEmail(string email)
        {
            try
            {
                string sql = $"SELECT * FROM Drivers WHERE Email = '{email}'";
                var result = await SqlHelper.ExecuteReaderAsync(sql, _dbSettings.DefaultConnection, DriverTranslator.TranslateDriver);
                return result?.FirstOrDefault();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// create new driver
        /// </summary>
        /// <param name="model"></param>
        /// <returns>driver id created</returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> Create(Driver model)
        {
            try
            {
                string query = $"INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES ('{model.FirstName}', '{model.LastName}', '{model.Email}', '{model.PhoneNumber}');SELECT CAST(scope_identity() AS int)";
                return (int)await SqlHelper.ExecuteScalerAsync(query, _dbSettings.DefaultConnection);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// update driver by id
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Update(Driver driver)
        {
            try
            {
                var query = $"UPDATE Drivers SET FirstName = '{driver.FirstName}', LastName= '{driver.LastName}', Email= '{driver.Email}', PhoneNumber= '{driver.PhoneNumber}' WHERE Id = {driver.Id}";
                SqlHelper.ExecuteNonQueryAsync(query, _dbSettings.DefaultConnection);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// delete driver by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Delete(int id)
        {
            try
            {
                var query = $"DELETE FROM Drivers WHERE Id = {id}";
                SqlHelper.ExecuteNonQueryAsync(query, _dbSettings.DefaultConnection);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// get all driver in system ordered by first name ascending 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Driver>> GetAllDriversOrdered()
        {
            try
            {
                string sql = "SELECT * FROM Drivers ORDER BY UPPER(FirstName) ASC";
                return await SqlHelper.ExecuteReaderAsync(sql, _dbSettings.DefaultConnection, DriverTranslator.TranslateDriver);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Add new driver list in system
        /// </summary>
        /// <param name="driver"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task AddDriverList(List<Driver> drivers)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_dbSettings.DefaultConnection))
                {
                    await connection.OpenAsync();
                    foreach (Driver driver in drivers)
                    {
                        SqlCommand command = new SqlCommand($"IF NOT EXISTS(SELECT 1 FROM Drivers WHERE Email = '{driver.Email}') INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES ('{driver.FirstName}', '{driver.LastName}', '{driver.Email}', '{driver.PhoneNumber}')", connection);
                        var test = await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
