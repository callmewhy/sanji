namespace DemoTest
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using Sanji;

    [TestClass]
    public class SampleASP1Test
    {
        private Service ServiceASP1 => Sanji.GetService("SampleASP1");

        [TestMethod]
        public async Task Test()
        {
            var httpClient = this.ServiceASP1.CreateHttpClient();
            var response = await httpClient.GetAsync("/books");
            var content = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<object>>(content);
            Assert.AreEqual(0, books.Count);

            var jsonString = JsonConvert.SerializeObject(new { Name = "Book" });
            await httpClient.PostAsync("http://localhost:5000/books", new StringContent(jsonString, Encoding.UTF8, "application/json"));

            response = await httpClient.GetAsync("http://localhost:5000/books");
            content = await response.Content.ReadAsStringAsync();
            books = JsonConvert.DeserializeObject<List<object>>(content);
            Assert.AreEqual(1, books.Count);
        }
    }
}
