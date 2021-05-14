using System;
using EasyNetQ;
using Subscriber;

namespace Publisher {
	class Program {
        const string AMQP = "amqps://uzvpuvak:ozcsROQDKpXnTCmOwVV5AWCNFShiHbeD@rattlesnake.rmq.cloudamqp.com/uzvpuvak";

		static void Main(string[] args) {
            var count = 1;
            using var bus = RabbitHutch.CreateBus(AMQP);
            while(true) {
                var message = new Message($"This is message {count++}");
                bus.PubSub.Publish(message);
                Console.WriteLine("Message published");
                Console.ReadKey();
            }
		}
	}
}
