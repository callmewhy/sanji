namespace Sanji
{
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public static class ProcessTool
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

        public static void Kill(int pid)
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
    }
}
