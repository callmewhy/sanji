namespace DemoTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Sanji;

    [TestClass]
    public class AssemblyInit
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            Sanji.Start();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            Sanji.Stop();
        }
    }
}
