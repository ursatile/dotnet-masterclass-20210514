using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Autobarn.PricingServer {
	public class Program {
		public Program() { }

		public static void Main(string[] args) {
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => {
					webBuilder.ConfigureKestrel(options => {
						var pfxPassword = Environment.GetEnvironmentVariable("UrsatilePfxPassword");
						var https = UseCertificateIfAvailable(@"d:\workshop.ursatile.com\ursatile.com.pfx", pfxPassword);
						options.ListenAnyIP(5002,
							listenOptions => listenOptions.Protocols = HttpProtocols.Http1AndHttp2);
						options.Listen(IPAddress.Any, 5003, https);
					});
					webBuilder.UseStartup<Startup>();
				}
			);

		private static Action<ListenOptions> UseCertificateIfAvailable(string pfxFilePath, string pfxPassword) {
			if (File.Exists(pfxFilePath)) return listen => listen.UseHttps(pfxFilePath, pfxPassword);
			return listen => listen.UseHttps();
		}
	}
}