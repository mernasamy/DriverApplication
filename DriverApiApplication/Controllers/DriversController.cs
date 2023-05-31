using DriverApiApplication.Models;
using DriverApiApplication.Models.Dto;
using DriverApiApplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace DriverApiApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private IDriverService _driverService;

        public DriversController(IDriverService driverService)
        {
            _driverService = driverService;
        }

        /// <summary>
        /// Returns all drivers in the system 
        /// </summary>
        /// <returns>Returns an HTTP 200 OK response with the list of drivers in the response body.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAll()
        {
            var drivers = await _driverService.GetAll();
            if (drivers == null || !drivers.Any())
                return NoContent();
            return Ok(drivers);
        }

        /// <summary>
        /// Get driver by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns an HTTP 200 OK response with the the driver in the response body.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id == 0)
                return BadRequest("Invalid driver Id");
            var driver = await _driverService.GetById(id);
            if (driver == null)
                return NoContent();
            return Ok(driver);
        }

        /// <summary>
        /// Create new driver 
        /// </summary>
        /// <param name="model">model of driver</param>
        /// <returns>Returns Created response with the ID of the newly created driver in the response body.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateDriver model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            int driverId = await _driverService.Create(model);
            return Ok(new { isSuccess = true, id = driverId, message = "Driver created" });
        }

        /// <summary>
        /// update driver  
        /// </summary>
        /// <param name="id">deriver id</param>
        /// <param name="model">model of driver</param>
        /// <returns>Returns response with message Updated driver in the response body.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateDriver model)
        {
            if (id == 0)
                return BadRequest("Invalid driver Id");
            await _driverService.Update(id, model);
            return Ok(new { isSuccess = true, message = "Driver updated" });
        }

        /// <summary>
        /// Delete driver
        /// </summary>
        /// <param name="id">driver id</param>
        /// <returns>Returns response with message Deleted driver in the response body.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
                return BadRequest("Invalid driver Id");
            await _driverService.Delete(id);
            return Ok(new { isSuccess = true, message = "Driver deleted" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetOrderedDrivers")]
        public async Task<IActionResult> GetAllDriversSorted()
        {
            var drivers = await _driverService.GetAllOrderedDrivers();
            return Ok(drivers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUserAlphabetized")]
        public async Task<IActionResult> GetUserAlphabetized(int id)
        {
            var drivers = await _driverService.GetUserAlphabetized(id);
            return Ok(drivers);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateDriverList")]
        public async Task<IActionResult> CreateRandomDriverList(List<CreateDriver> models)
        {
            await _driverService.CreateDriverList(models);
            return Ok(new { isSuccess = true, message = "Driver List Created" });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listLength"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateRandomList")]
        public async Task<IActionResult> CreateRandomDriverList(int listLength)
        {
            await _driverService.CreateRandomDriverList(listLength);
            return Ok(new { isSuccess = true, message = "Driver List Created" });
        }
    }
}
