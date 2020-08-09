namespace Sanji
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class ProcessTool
    {
        public delegate void DataReceivedEventHandler(string data);

        public static int Start(
            string filename,
            string arguments,
            DataReceivedEventHandler onOutputDataReceived,
            DataReceivedEventHandler onErrorDataReceived)
        {
            var file = new FileInfo(filename);
            using var process = new Process()
            {
                StartInfo =
                {
                    FileName = "netstat.exe",
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                },
                EnableRaisingEvents = true,
            };
            process.OutputDataReceived += (_, e) =>
            {
                onOutputDataReceived(e.Data);
            };
            process.ErrorDataReceived += (_, e) =>
            {
                onErrorDataReceived(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process.Id;
        }

        public static void KillByPid(int pid)
        {
            using var process = Process.GetProcessById(pid);
            process.Kill();
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

            var process = new Process() { StartInfo = pStartInfo };
            process.Start();

            var output = process.StandardOutput.ReadToEnd();

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
                            KillByPid(int.Parse(parts.Last()));
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
