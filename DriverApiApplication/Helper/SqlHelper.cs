using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DriverApiApplication.Helper
{
    public static class SqlHelper
    {
        /// <summary>
        /// Executes a SQL query asynchronously and maps the results to a list of objects using a translator delegate.
        /// </summary>
        /// <typeparam name="T">The type of the objects to map the results to.</typeparam>
        /// <param name="queryString">The SQL query to execute.</param>
        /// <param name="connectionString">The connection string to use to connect to the database.</param>
        /// <param name="translator">The delegate that maps the results to objects of type T.</param>
        /// <returns>Returns a list of objects of type T that represent the results of the query.</returns>
        public static async Task<List<T>> ExecuteReaderAsync<T>(string queryString, string connectionString, Func<IDataReader, T> translator)
        {
            // Create an empty list to store the results
            var result = new List<T>();

            // Create a new SQL connection using the specified connection string
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create a new SQL command using the specified query string and connection
                using (var command = new SqlCommand(queryString, connection))
                {
                    // Open the connection asynchronously
                    await connection.OpenAsync();

                    // Execute the command asynchronously and create a data reader to read the results
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        // Read the results and map them to objects of type T using the specified translator delegate
                        while (await reader.ReadAsync())
                        {
                            result.Add(translator(reader));
                        }
                    }
                }
            }
            // Return the list of results
            return result;
        }

        /// <summary>
        /// Executes a SQL query that doesn't return any records asynchronously.
        /// </summary>
        /// <param name="queryString">The SQL query to execute.</param>
        /// <param name="connectionString">The connection string to use to connect to the database.</param>
        public static async Task ExecuteNonQueryAsync(string queryString, string connectionString)
        {
            // Create a new SQL connection using the specified connection string
            using (SqlConnection connection = new SqlConnection(
                      connectionString))
            {
                // Create a new SQL command using the specified query string and connection
                SqlCommand command = new SqlCommand(queryString, connection);

                // Open the connection asynchronously
                await connection.OpenAsync();

                // Execute the command asynchronously without returning any records
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Executes a SQL query that returns a single scalar value asynchronously.
        /// </summary>
        /// <param name="sql">The SQL query to execute.</param>
        /// <param name="connString">The connection string to use to connect to the database.</param>
        /// <returns>Returns the scalar value returned by the query, or null if the query didn't return any values.</returns>
        public static async Task<object> ExecuteScalerAsync(string sql, string connString)
        {
            // Create a variable to store the result of the query
            object? returnValue = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                // Create a new SQL command using the specified query string and connection
                SqlCommand cmd = new SqlCommand(sql, conn);
                // Open the connection asynchronously
                await conn.OpenAsync();

                // Execute the command asynchronously and retrieve the scalar value returned by the query
                returnValue = await cmd.ExecuteScalarAsync();
            }
            return returnValue;
        }
    }
}

