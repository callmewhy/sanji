namespace Sanji
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class ProcessTool
    {
        public delegate void DataReceivedEventHandler(string data);

        public static Process Start(
            string filename,
            IDictionary<string, string> environmentVariables = null,
            string arguments = "")
        {
            var file = new FileInfo(filename);
            var process = new Process()
            {
                StartInfo =
                {
                    FileName = file.FullName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                },
                EnableRaisingEvents = true,
            };

            if (environmentVariables != null)
            {
                foreach (var i in environmentVariables)
                {
                    process.StartInfo.Environment.Add(i);
                }
            }

            process.Start();

            return process;
        }

        public static void KillByPort(int port)
        {
            var pStartInfo = new ProcessStartInfo()
            {
                FileName = "netstat.exe",
                Arguments = "-a -n -o",
                WindowStyle = ProcessWindowStyle.Maximized,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };

            var netstatProcess = new Process() { StartInfo = pStartInfo };
            netstatProcess.Start();

            var output = netstatProcess.StandardOutput.ReadToEnd();

            foreach (var line in output.Split("\r\n"))
            {
                if (line.Trim().StartsWith("Proto"))
                {
                    continue;
                }

                var parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length > 2)
                {
                    var currentPort = int.Parse(parts[1].Split(':').Last());
                    if (currentPort == port)
                    {
                        try
                        {
                            var pid = int.Parse(parts.Last());
                            var process = Process.GetProcessById(pid);
                            process.Kill();
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }
    }
}
