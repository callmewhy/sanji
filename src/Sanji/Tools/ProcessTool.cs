namespace Sanji
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal static class ProcessTool
    {
        public static int Start(string filename, string arguments = "")
        {
            var file = new FileInfo(filename);
            using var process = new Process()
            {
                StartInfo =
                {
                    FileName = file.FullName,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                },
                EnableRaisingEvents = true,
            };
            var outputBuilder = new StringBuilder();
            process.OutputDataReceived += (_, e) =>
            {
                outputBuilder.AppendLine(e.Data);
            };

            var errorBuilder = new StringBuilder();
            process.ErrorDataReceived += (_, e) =>
            {
                errorBuilder.AppendLine(e.Data);
            };

            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return process.Id;
        }

        public static void KillByPid(int pid)
        {
            try
            {
                using var process = Process.GetProcessById(pid);
                process?.Kill();
            }
            catch
            {
            }
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
            try
            {
                process.Start();
            }
            catch
            {
                return;
            }

            var output = process.StandardOutput.ReadToEnd();
            if (process.ExitCode != 0)
            {
                throw new Exception($"netstat failed with ExitCode: {process.ExitCode}");
            }

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
                        Process.GetProcessById(int.Parse(parts.Last())).Kill();
                    }
                }
            }
        }
    }
}
