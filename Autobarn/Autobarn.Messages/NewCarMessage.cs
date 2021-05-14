using System;

namespace Autobarn.Messages {
	public class NewCarMessage {
		public string Make { get; set; }
		public string Model { get; set; }
		public string Registration { get; set; }
		public int Year { get; set; }
		public string Color { get; set; }
		public DateTimeOffset ListedAt { get; set; }
	}

	public class NewCarPriceMessage : NewCarMessage {
		public int Price {get;set;}
		public string CurrencyCode { get;set;}
	}
}
