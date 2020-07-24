namespace Sanji
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public static class Sanji
    {
        private static AppSettings AppSettings { get; set; }

        private static List<Service> Services { get; set; }

        public static Service GetService(string serviceName)
        {
            return Services.Find(i => i.Name == serviceName);
        }

        public static void Start()
        {
            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                    .Build();

            AppSettings = configuration.Get<AppSettings>();
            Services = AppSettings.Services.ConvertAll(i =>
            {
                return new Service()
                {
                    Name = i.Name,
                    Executable = i.Executable,
                    Port = i.Port,
                };
            });

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
