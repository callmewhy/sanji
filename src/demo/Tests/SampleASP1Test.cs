namespace DemoTest
{
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

        private HttpClient HttpClient => this.ServiceASP1.CreateHttpClient();

        [TestMethod]
        public async Task Test()
        {
            var books = await this.ListBooksAsync();
            Assert.AreEqual(0, books.Count);

            await this.AddBookAsync();

            books = await this.ListBooksAsync();
            Assert.AreEqual(1, books.Count);
        }

        private async Task<List<object>> ListBooksAsync()
        {
            var response = await this.HttpClient.GetAsync("/books");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<object>>(content);
        }

        private async Task AddBookAsync()
        {
            var jsonString = JsonConvert.SerializeObject(new { Name = "Book" });
            var body = new StringContent(jsonString, Encoding.UTF8, "application/json");
            await this.HttpClient.PostAsync("http://localhost:5000/books", body);
        }
    }
}
