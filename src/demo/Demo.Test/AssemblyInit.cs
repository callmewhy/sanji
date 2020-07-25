namespace DemoTest
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sanji;

    [TestClass]
    public class AssemblyInit
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Console.WriteLine($"Test {context.TestName} start");
            Sanji.Start();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Sanji.Stop();
        }
    }
}
