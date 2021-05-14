using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer {
	public class PricerService : Pricer.PricerBase {
        static int prices = 0;
		static Random random = new Random();

		private readonly ILogger<PricerService> _logger;
		public PricerService(ILogger<PricerService> logger) {
			_logger = logger;
		}

		public override Task<PriceReply> GetPrice(PriceRequest request, ServerCallContext context) {
            _logger.Log(LogLevel.Information, $"{prices++} prices calculated!");
			int price = 0;
			if (request.Color.Contains("blue", StringComparison.OrdinalIgnoreCase)) {
				price = 200;
			} else if (request.Make == "Tesla") {
				price = 10000;
			} else {
				price = 5000 + random.Next(5000);
			}
			return Task.FromResult(new PriceReply {
				Price = price,
				CurrencyCode = "USD"
			});
		}
	}
}
