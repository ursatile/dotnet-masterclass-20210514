using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text.Json.Serialization;
using System.Linq;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Controllers.Api {
	public static class ObjectExtensions {
		public static dynamic ToDynamic(this object value) {
			IDictionary<string, object> expando = new ExpandoObject();
			var properties = TypeDescriptor.GetProperties(value.GetType());
			foreach (PropertyDescriptor property in properties) {
				if (property.Attributes.OfType<JsonIgnoreAttribute>().Any()) continue;
				expando.Add(property.Name, property.GetValue(value));
			}
			return (ExpandoObject)expando;
		}


		public static dynamic ToHypermediaResult(this Car car) {
			var result = car.ToDynamic();
			result._links = new {
				self = new {
					href = $"/api/cars/{car.Registration}"
				},
				model = new {
					href = $"/api/carmodels/{car.CarModel.Code}"
				}
			};
            return result;
		}
	}
}