using Autobarn.Data.Entities;

namespace Autobarn.Website.Models.Api {
	public class PostCarDto {
		public string Registration { get; set; }
		public int Year { get; set; }
		public string Color { get; set; }
		public string ModelCode { get; set; }

		public Car ToCarEntity(CarModel carModel) {
			var car = new Car {
				Registration = this.Registration,
				Color = this.Color,
				Year = this.Year,
				CarModel = carModel
			};
			return car;
		}
	}
}