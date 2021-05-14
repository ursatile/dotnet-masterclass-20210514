using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using Autobarn.Messages;
using EasyNetQ;
using Newtonsoft.Json;

namespace Autobarn.Notifier {
	class Program {
		const string SIGNALR = "https://workshop.ursatile.com:5001/newcarhub";
		const string AMQP = "amqps://uzvpuvak:ozcsROQDKpXnTCmOwVV5AWCNFShiHbeD@rattlesnake.rmq.cloudamqp.com/uzvpuvak";
		static IBus bus;
		private static HubConnection hub;
		static async Task Main(string[] args) {
			bus = RabbitHutch.CreateBus(AMQP);
			bus.PubSub.Subscribe<NewCarPriceMessage>("autobarn.notifier", HandleNewCarPriceMessage);

			hub = new HubConnectionBuilder().WithUrl(SIGNALR).Build();
			await hub.StartAsync();
			Console.WriteLine("Hub started!");
			Console.ReadKey(false);
			//while (true) {
			//	await hub.SendAsync("SendMessage", "Autobarn.Notifier", "Hey! We're connected! Yay!");
			//	Console.WriteLine("Message sent!");
			//	Console.ReadKey(false);
			//}
		}

		static async void HandleNewCarPriceMessage(NewCarPriceMessage message) {
			var json = JsonConvert.SerializeObject(message);
			await hub.SendAsync("SendMessage", "Autobarn.Notifier", json);
		}
	}
}
