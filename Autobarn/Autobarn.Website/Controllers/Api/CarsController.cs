using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Models.Api;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Dynamic;
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
		public IActionResult Get(int index, int count = 2) {
			var items = database.Cars.Skip(index).Take(count).Select(car => car.ToHypermediaResult());
			var total = database.Cars.Count();
			var links = Paginate("/api/cars", index, count, total);
			var result = new {				
				_links = links,
				index = index,
				count = count,
				total = total,
				items = items
			};
			return Ok(result);
		}

		public dynamic Paginate(string url, int index, int count, int total) {
			dynamic links = new ExpandoObject();
			if (index > 0) links.previous = $"{url}?index={index-count}&count={count}";
			if (index < total) links.next = $"{url}?index={index+count}&count={count}";
			return links;	
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var car = database.Cars.FirstOrDefault(c => c.Registration.Equals(id, System.StringComparison.InvariantCultureIgnoreCase));
			if (car == default) return NotFound($"Sorry, we don't have a car with registration {id}");
			var result = car.ToHypermediaResult();
			return Ok(result);
		}
	}
}