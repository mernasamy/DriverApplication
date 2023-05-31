using DriverApiApplication.Models;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;
using System.Text.Json;

namespace DriverApiApplication.Helper
{
    public class DataContext
    {
        private DbSettings _dbSettings;

        public DataContext(IOptions<DbSettings> dbSettings)
        {
            _dbSettings = dbSettings.Value;
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_dbSettings.DefaultConnection);
        }

        public async Task Init()
        {
            await InitDatabase();
            await InitTables();
            //await Seed();
        }

        /// <summary>
        /// initial database if not exist
        /// </summary>
        /// <returns></returns>
        private async Task InitDatabase()
        {
            try
            {
                string queryString = "IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'DriverDb') CREATE DATABASE DriverDb";
                await SqlHelper.ExecuteNonQueryAsync(queryString, _dbSettings.MasterConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
        }

        /// <summary>
        /// initial table if not exist
        /// </summary>
        /// <returns></returns>
        private async Task InitTables()
        {
            try
            {
                string createTableSql = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Drivers')
        BEGIN
            CREATE TABLE Drivers (
                Id INT IDENTITY(1,1) PRIMARY KEY,
                FirstName VARCHAR(50) NOT NULL,
                    LastName VARCHAR(50) NOT NULL,
                    Email VARCHAR(50) NOT NULL UNIQUE,
                    PhoneNumber VARCHAR(50) NOT NULL
            )
        END";
                await SqlHelper.ExecuteNonQueryAsync(createTableSql, _dbSettings.DefaultConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
        }

        /// <summary>
        /// Seed driver data from json file 
        /// </summary>
        /// <returns></returns>
        private async Task Seed()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "drivers.json");
                var jsonData = File.ReadAllText(filePath);
                var drivers = JsonSerializer.Deserialize<List<Driver>>(jsonData);

                using (SqlConnection connection = (SqlConnection)CreateConnection())
                {
                    await connection.OpenAsync();
                    foreach (Driver item in drivers)
                    {
                        SqlCommand command = new SqlCommand($"IF NOT EXISTS(SELECT 1 FROM Drivers WHERE Email = '{item.Email}') INSERT INTO Drivers (FirstName, LastName, Email, PhoneNumber) VALUES ('{item.FirstName}', '{item.LastName}', '{item.Email}', '{item.PhoneNumber}')", connection);
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
            }
        }
    }
}
