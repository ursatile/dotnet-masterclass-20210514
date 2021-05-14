using System;
using EasyNetQ;

namespace Subscriber {
	class Program {
        const string AMQP = "amqps://uzvpuvak:ozcsROQDKpXnTCmOwVV5AWCNFShiHbeD@rattlesnake.rmq.cloudamqp.com/uzvpuvak";

		static void Main(string[] args) {
			using var bus = RabbitHutch.CreateBus(AMQP);
            bus.PubSub.Subscribe<Message>("subscriber-id", HandleMessage);
            Console.ReadKey();
		}

        static void HandleMessage(Message message) {
            Console.WriteLine(message.Body);
        }
	}

    public class Message {
        public Message(string body) {
            this.Body = body;
        }
		public string Body { get; private set; }
	}
}
