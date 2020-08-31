namespace Sanji
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;

    public class Service
    {
        internal Service(AppSettings.Service serviceSettings)
        {
            this.Settings = serviceSettings;
        }

        internal AppSettings.Service Settings { get; set; }

        private Process Process { get; set; }

        private DateTime StartedAt { get; set; }

        internal void Start()
        {
            var settings = this.Settings;
            try
            {
                ProcessTool.KillByPort(settings.Port);

                this.Settings.EnvironmentVariables["ASPNETCORE_URLS"] = $"http://localhost:{this.Settings.Port}";

                this.Process = ProcessTool.Start(
                    settings.Executable,
                    settings.EnvironmentVariables);
                this.StartedAt = DateTime.Now;
                Console.WriteLine($"{settings.Name} running on process id {this.Process.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch process for service {settings.Name}", ex);
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
            Console.WriteLine($"{this.Settings.Name} is killed on process id {this.Process.Id}\n");
        }

        private void LogText(string type, string text)
        {
            var filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"log_{this.StartedAt:yyyy_MM_dd_HH_mm_ss}_{this.Settings.Name}_{type}.txt");
            File.AppendAllText(filename, text);
        }
    }
}
