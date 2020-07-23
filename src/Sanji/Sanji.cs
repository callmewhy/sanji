namespace Sanji
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public static class Sanji
    {
        private static AppSettings AppSettings { get; set; }

        private static List<Service> Services { get; set; }

        public static void Start()
        {
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();

            AppSettings = configuration.Get<AppSettings>();
            Services = AppSettings.Services.ConvertAll(i => new Service() { Settings = i, });

            foreach (var service in Services)
            {
                service.Start();
            }
        }

        public static void Stop()
        {
            foreach (var service in Services)
            {
                service.Stop();
            }
        }
    }
}
