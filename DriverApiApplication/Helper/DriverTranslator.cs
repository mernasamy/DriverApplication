using System;
using System.Data;
using DriverApiApplication.Models;

namespace DriverApiApplication.Helper
{
    public static class DriverTranslator
    {
        /// <summary>
        /// translate from IDataReader to driver
        /// </summary>
        /// <param name="reader"></param>
        /// <returns>return driver</returns>
        public static Driver TranslateDriver(IDataReader reader)
        {
            var driver = new Driver
            {
                Id = (int)reader["Id"],
                FirstName = (string)reader["FirstName"],
                LastName = reader["LastName"].ToString(),
                Email = reader["Email"].ToString(),
                PhoneNumber = reader["PhoneNumber"].ToString()
            };

            return driver;
        }
    }
}
