namespace Sanji
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class Service
    {
        public string Name { get; set; }

        internal string Executable { get; set; }

        internal int Port { get; set; }

        internal Process Process { get; set; }

        internal DateTime StartedAt { get; set; }

        internal void Start()
        {
            try
            {
                ProcessTool.KillByPort(this.Port);
                this.Process = ProcessTool.Start(this.Executable);
                this.StartedAt = DateTime.Now;
                Console.WriteLine($"{this.Name} running on process id {this.Process.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch process for service {this.Name}", ex);
                throw;
            }
        }

        internal void Stop()
        {
            this.Process.Kill();
            var output = this.Process.StandardOutput.ReadToEnd();
            var error = this.Process.StandardError.ReadToEnd();
            this.LogText("output", output);
            this.LogText("error", error);
            Console.WriteLine($"{this.Name} is killed on process id {this.Process.Id}\n");
        }

        private void LogText(string type, string text)
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"log_{this.Name}_{type}_{this.StartedAt:yyyy_MM_dd_HH_mm_ss}.txt");
            File.AppendAllText(filename, text);
        }
    }
}
