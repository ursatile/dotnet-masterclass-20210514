using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models.Api;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Autobarn.Website.Controllers.Api {

	[Route("api/[controller]")]
	[ApiController]
	public class CarsController : ControllerBase {
		private readonly ICarDatabase database;

		public CarsController(ICarDatabase database) {
			this.database = database;
		}

		[HttpPost]
		public IActionResult Post(PostCarDto postData) {
			var carModel = database.FindCarModel(postData.ModelCode);		
			// 1: What if the model code doesn't exist?
			if (carModel == default) return BadRequest($"Sorry, we don't know anything about the car model {postData.ModelCode}");
			// 2: Same car already listed for sale?
			// 3: Everything's fine.
			var car = postData.ToCarEntity(carModel);
			database.AddCar(car);
			return Created($"/api/cars/{car.Registration}", car);
		}

		
		[HttpGet]
		public IActionResult Get() {
			return Ok(database.Cars.ToList());
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var car = database.Cars.FirstOrDefault(c => c.Registration.Equals(id, System.StringComparison.InvariantCultureIgnoreCase));
			if (car == default) return NotFound($"Sorry, we don't have a car with registration {id}");
			return Ok(car);
		}
	}
}