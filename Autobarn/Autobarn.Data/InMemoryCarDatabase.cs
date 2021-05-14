﻿using Autobarn.Data.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Autobarn.Data {
	public class InMemoryCarDatabase : ICarDatabase {

		public IList<Car> Cars => cars;
		public IList<Make> Makes => makes;
		public IList<CarModel> Models => models;
		public Car FindCar(string registration) =>
			Cars.FirstOrDefault(c =>
				String.Equals(c.Registration, registration, StringComparison.InvariantCultureIgnoreCase));

		public void AddCar(Car car) {
			cars.Add(car);
			car.CarModel.Cars.Add(car);
		}

		public CarModel FindCarModel(string modelCode) => this.Models.FirstOrDefault(m => m.Code.Equals(modelCode, StringComparison.InvariantCultureIgnoreCase));

		private readonly List<Make> makes;
		private readonly List<CarModel> models;
		private readonly List<Car> cars;

		public InMemoryCarDatabase(string dataFilePath) {
			makes = new List<Make>();
			foreach (var filePath in Directory.GetFiles(dataFilePath, "*.json")) {
				var json = File.ReadAllText(filePath);
				var make = JsonConvert.DeserializeObject<Make>(json);
				make.Code = Path.GetFileNameWithoutExtension(filePath);
				makes.Add(make);
			}
			makes.ForEach(make => make.Models.ForEach(model => model.Make = make));
			foreach (var carModel in makes.SelectMany(m => m.Models)) {
				carModel.Cars.ForEach(car => car.CarModel = carModel);
			}
			models = makes.SelectMany(make => make.Models).ToList();
			cars = models.SelectMany(model => model.Cars).ToList();
		}
	}
}