namespace Sanji
{
    using System;

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
                var pid = ProcessTool.Start(this.Executable);
                Console.WriteLine($"{this.Name} running on process id {pid}");
                this.Pid = pid;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to launch process for service {this.Name}", ex);
            }
        }

        internal void Stop()
        {
            ProcessTool.Kill(this.Pid);
            Console.WriteLine($"{this.Name} is killed on process id {this.Pid}");
        }
    }
}
