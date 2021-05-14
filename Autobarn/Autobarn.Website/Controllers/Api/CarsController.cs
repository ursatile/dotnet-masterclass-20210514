using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models.Api;
using Microsoft.AspNetCore.Mvc;
using System;
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
			if (String.IsNullOrEmpty(postData.Registration)) {
				return BadRequest("Cars with blank registrations are not allowed for sale");
			}

			var existingCar = database.FindCar(postData.Registration);
			if (existingCar != default) return Conflict($"Sorry - the car with registration {postData.Registration} is already in our system!");

			var carModel = database.FindCarModel(postData.ModelCode);		
			if (carModel == default) return BadRequest($"Sorry, we don't know anything about the car model {postData.ModelCode}");

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