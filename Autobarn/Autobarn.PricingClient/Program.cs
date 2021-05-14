using System;
using Grpc.Net.Client;
using Autobarn.PricingServer;

namespace Autobarn.PricingClient {
	class Program {
		const string GRPC = "https://workshop.ursatile.com:5003";
		static void Main(string[] args) {
			using var channel = GrpcChannel.ForAddress(GRPC);
			var client = new Pricer.PricerClient(channel);
			while (true) {
				var request = new PriceRequest {
					Color = "blue",
					Year = 1985,
					Make = "Ford",
					Model = "Mustang"
				};
				var reply = client.GetPrice(request);
				Console.WriteLine($"{reply.Price} {reply.CurrencyCode}");
				Console.ReadKey();
			}
		}
	}
}
