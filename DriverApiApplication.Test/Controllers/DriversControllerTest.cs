using DriverApiApplication.Controllers;
using DriverApiApplication.Models;
using DriverApiApplication.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using DriverApiApplication.Models.Dto;
using DriverApiApplication.Repositories;
using Moq;

namespace DriverApiApplication.Test.Controllers
{
    public class DriversControllerTest
    {
        [Fact]
        public async Task GetAll_ReturnsAllDrivers()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            var drivers = new List<Driver> { new Driver { Id = 1, FirstName = "Driver 1", LastName = "Driver 1", Email = "Driver1@gmail.com", PhoneNumber = "012701452" }, new Driver { Id = 2, FirstName = "Driver 2", LastName = "Driver 2", Email = "Driver2@gmail.com", PhoneNumber = "812701452" } };
            driverServiceMock.Setup(s => s.GetAll()).ReturnsAsync(drivers);
            var controller = new DriversController(driverServiceMock.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDrivers = Assert.IsType<List<Driver>>(okResult.Value);
            Assert.Equal(drivers.Count, returnedDrivers.Count);
            Assert.Equal(drivers[0].Id, returnedDrivers[0].Id);
            Assert.Equal(drivers[0].FirstName, returnedDrivers[0].FirstName);
            Assert.Equal(drivers[0].LastName, returnedDrivers[0].LastName);
            Assert.Equal(drivers[0].Email, returnedDrivers[0].Email);
            Assert.Equal(drivers[0].PhoneNumber, returnedDrivers[0].PhoneNumber);
            Assert.Equal(drivers[1].Id, returnedDrivers[1].Id);
            Assert.Equal(drivers[1].FirstName, returnedDrivers[1].FirstName);
            Assert.Equal(drivers[1].LastName, returnedDrivers[1].LastName);
            Assert.Equal(drivers[1].Email, returnedDrivers[1].Email);
            Assert.Equal(drivers[1].PhoneNumber, returnedDrivers[1].PhoneNumber);
        }

        [Fact]
        public async Task GetAll_ReturnsNotFoundResult_WhenThereAreNoDrivers()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            driverServiceMock
                .Setup(service => service.GetAll())
                .ReturnsAsync(new List<Driver>());

            var controller = new DriversController(driverServiceMock.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsOkResultWithDriverId()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            driverServiceMock.Setup(s => s.Create(It.IsAny<CreateDriver>())).ReturnsAsync(1);
            var controller = new DriversController(driverServiceMock.Object);
            var model = new CreateDriver { FirstName = "Driver 1", LastName = "ABC123", Email = "test@gmail.com", PhoneNumber = "554545" };

            // Act
            var result = await controller.Create(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successValue = Assert.IsType<bool>(okResult.Value.GetType().GetProperty("isSuccess").GetValue(okResult.Value));
            var idValue = Assert.IsType<int>(okResult.Value.GetType().GetProperty("id").GetValue(okResult.Value));
            var messageValue = Assert.IsType<string>(okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
            Assert.True(successValue);
            Assert.Equal(1, idValue);
            Assert.Equal("Driver created", messageValue);
        }

        [Fact]
        public async Task Create_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            var controller = new DriversController(driverServiceMock.Object);
            controller.ModelState.AddModelError("FirstName", "First Name is required");

            var newDriver = new CreateDriver { LastName = "Driver2", Email = "driver2@gmail.com", PhoneNumber = "ABC123" };

            // Act
            var result = await controller.Create(newDriver);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var errors = Assert.IsType<SerializableError>(badRequestResult.Value);
            Assert.True(errors.ContainsKey("FirstName"));
            Assert.Equal(new string[] { "First Name is required" }, errors["FirstName"]);
        }

        [Fact]
        public async Task GetById_ReturnsDriverById()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            var driver = new Driver { FirstName = "Driver 1", LastName = "ABC123", Email = "test@gmail.com", PhoneNumber = "554545" };
            driverServiceMock.Setup(s => s.GetById(It.IsAny<int>())).ReturnsAsync(driver);
            var controller = new DriversController(driverServiceMock.Object);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedDriver = Assert.IsType<Driver>(okResult.Value);
            Assert.Equal(driver.Id, returnedDriver.Id);
            Assert.Equal(driver.FirstName, returnedDriver.FirstName);
            Assert.Equal(driver.LastName, returnedDriver.LastName);
            Assert.Equal(driver.Email, returnedDriver.Email);
            Assert.Equal(driver.PhoneNumber, returnedDriver.PhoneNumber);
        }

        [Fact]
        public async Task GetById_ReturnsNotFoundResult_WhenDriverIsNotFound()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            driverServiceMock
                .Setup(service => service.GetById(It.IsAny<int>()))
                .ReturnsAsync((Driver)null);

            var controller = new DriversController(driverServiceMock.Object);

            // Act
            var result = await controller.GetById(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResultWithSuccessMessage()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            var controller = new DriversController(driverServiceMock.Object);
            var updateModel = new UpdateDriver { FirstName = "Driver 1", LastName = "ABC123", Email = "test@gmail.com", PhoneNumber = "554545" };

            // Act
            var result = await controller.Update(1, updateModel);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successValue = Assert.IsType<bool>(okResult.Value.GetType().GetProperty("isSuccess").GetValue(okResult.Value));
            var messageValue = Assert.IsType<string>(okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
            Assert.True(successValue);
            Assert.Equal("Driver updated", messageValue);
        }

        [Fact]
        public async Task Update_ReturnsBadRequestResult_WhenIdIsMissing()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            var controller = new DriversController(driverServiceMock.Object);

            var driverToUpdate = new UpdateDriver { FirstName = "John Doe", LastName = "ABC123", Email = "test@gmail.com", PhoneNumber = "554545" };

            // Act
            var result = await controller.Update(0, driverToUpdate);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsOkResultWithSuccessMessage()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();
            var controller = new DriversController(driverServiceMock.Object);

            // Act
            var result = await controller.Delete(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var successValue = Assert.IsType<bool>(okResult.Value.GetType().GetProperty("isSuccess").GetValue(okResult.Value));
            var messageValue = Assert.IsType<string>(okResult.Value.GetType().GetProperty("message").GetValue(okResult.Value));
            Assert.True(successValue);
            Assert.Equal("Driver deleted", messageValue);
        }

        [Fact]
        public async Task Delete_ReturnsNotFoundResult_WhenIdIsMissing()
        {
            // Arrange
            var driverServiceMock = new Mock<IDriverService>();

            var controller = new DriversController(driverServiceMock.Object);

            // Act
            var result = await controller.Delete(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

    }
}

