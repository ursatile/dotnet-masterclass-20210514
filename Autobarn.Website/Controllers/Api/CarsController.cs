﻿using Autobarn.Data;
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