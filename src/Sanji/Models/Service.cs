namespace Sanji
{
    using System;
    using System.IO;

    public class Service
    {
        public string Name { get; set; }

        internal string Executable { get; set; }

        internal int Port { get; set; }

        internal int Pid { get; set; }

        internal void Start()
        {
            try
            {
                ProcessTool.KillByPort(this.Port);
                var pid = ProcessTool.Start(
                    this.Executable,
                    string.Empty,
                    (data) =>
                    {
                        Console.WriteLine($"{this.Name} log: {data}");
                        this.LogText(data);
                    },
                    (data) =>
                    {
                        Console.WriteLine($"{this.Name} log: {data}");
                        this.LogText(data);
                    });
                Console.WriteLine($"{this.Name} running on process id {pid}");
                this.Pid = pid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch process for service {this.Name}", ex);
                throw;
            }
        }

        internal void Stop()
        {
            ProcessTool.KillByPid(this.Pid);
            Console.WriteLine($"{this.Name} is killed on process id {this.Pid}");
        }

        private void LogText(string text)
        {
            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "log.txt", text);
        }
    }
}
