namespace Sanji
{
    using System.Collections.Generic;

    public class AppSettings
    {
        public List<Service> Services { get; set; }

        public class Service
        {
            public string Name { get; set; }

            public string Executable { get; set; }

            public int Port { get; set; }
        }
    }
}
