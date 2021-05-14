using System;
using Grpc.Net.Client;
using EasyNetQ;
using Autobarn.PricingServer;
using Autobarn.Messages;

namespace Autobarn.PricingClient {
	class Program {
		const string GRPC = "https://workshop.ursatile.com:5003";
		const string AMQP = "amqps://uzvpuvak:ozcsROQDKpXnTCmOwVV5AWCNFShiHbeD@rattlesnake.rmq.cloudamqp.com/uzvpuvak";
		static Pricer.PricerClient pricer;
		
		static void Main(string[] args) {
			using var channel = GrpcChannel.ForAddress(GRPC);
			pricer = new Pricer.PricerClient(channel);
			using var bus = RabbitHutch.CreateBus(AMQP);

            bus.PubSub.Subscribe<NewCarMessage>("autobarn.pricingclient", HandleNewCarMessage);
            Console.ReadKey();
		}

        static void HandleNewCarMessage(NewCarMessage message) {
			var priceRequest = new PriceRequest {
				Make = message.Make,
				Model = message.Model,
				Color = message.Color,
				Year = message.Year
			};
			var price = pricer.GetPrice(priceRequest);
			Console.WriteLine("Priced car: ");
			Console.WriteLine($"{message.Make} {message.Model} ({message.Color}, {message.Year}");
			Console.WriteLine($"Price: {price.Price} {price.CurrencyCode}");
        }
	}
}