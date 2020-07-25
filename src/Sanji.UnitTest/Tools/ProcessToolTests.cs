namespace Sanji.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ProcessToolTests
    {
        private const string ExePath = @"..\..\..\..\demo\Demo.SampleASP1\bin\Release\netcoreapp3.1\Demo.SampleASP1.exe";
        private const int ExePort = 5000;
        private Process process = null;

        [TestInitialize]
        public void TestInitialize()
        {
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (this.process is null)
            {
                return;
            }

            this.process.Kill();
            while (!this.process.HasExited)
            {
            }

            this.process = null;
        }

        [TestMethod]
        public void Start_Successful()
        {
            var pid = ProcessTool.Start(ExePath);
            this.process = Process.GetProcessById(pid);

            Assert.IsFalse(this.process.HasExited);
        }

        [TestMethod]
        public async Task KillByPid_Successful()
        {
            var pid = this.StartMockProcess();
            this.process = Process.GetProcessById(pid);

            ProcessTool.KillByPid(pid);

            await Task.Delay(100);
            Assert.IsTrue(this.process.HasExited);
        }

        [TestMethod]
        public async Task KillByPort_Successful()
        {
            var pid = this.StartMockProcess();
            this.process = Process.GetProcessById(pid);
            await Task.Delay(2000);

            ProcessTool.KillByPort(ExePort);

            await Task.Delay(100);
            Assert.IsTrue(this.process.HasExited);
        }

        private int StartMockProcess()
        {
            var file = new FileInfo(ExePath);
            using var process = new Process()
            {
                StartInfo =
                {
                    FileName = file.FullName,
                    CreateNoWindow = true,
                },
            };

            process.Start();
            return process.Id;
        }
    }
}
