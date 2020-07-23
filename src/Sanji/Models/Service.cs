namespace Sanji
{
    using System;

    public class Service
    {
        public int Pid { get; set; }

        public AppSettings.Service Settings { get; set; }

        public void Start()
        {
            try
            {
                var pid = ProcessTool.Start(this.Settings.Executable);
                Console.WriteLine($"{this.Settings.Name} running on process id {pid}");
                this.Pid = pid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch process for service {this.Settings.Name}", ex);
            }
        }

        public void Stop()
        {
            ProcessTool.Kill(this.Pid);
        }
    }
}
