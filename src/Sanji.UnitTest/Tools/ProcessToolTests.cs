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
        private const string ExePath = @"..\..\..\..\Demo.SampleASP1\bin\Debug\netcoreapp3.1\Demo.SampleASP1.exe";
        private const int ExePort = 5000;
        private Process process = null;

        [TestInitialize]
        public void TestInitialize()
        {
            ProcessTool.KillByPort(ExePort);
        }

        [TestMethod]
        public void Start_Successful()
        {
            this.process = ProcessTool.Start(ExePath);
            Assert.IsFalse(this.process.HasExited);
            this.process.Kill();
        }

        [TestMethod]
        public async Task KillByPort_Successful()
        {
            this.process = ProcessTool.Start(ExePath);
            await Task.Delay(2000);
            ProcessTool.KillByPort(ExePort);
            await Task.Delay(1000);
            Assert.IsTrue(this.process.HasExited);
        }
    }
}
