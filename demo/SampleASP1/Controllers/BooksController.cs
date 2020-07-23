namespace SampleASP1.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class BooksController : ControllerBase
    {
        private static readonly List<object> Books = new List<object>();
        private readonly ILogger<BooksController> logger;

        public BooksController(ILogger<BooksController> logger)
        {
            this.logger = logger;
        }

        [HttpGet]
        public IActionResult List()
        {
            this.logger.LogInformation($"List books: {Books.Count} books");
            return this.Ok(Books);
        }

        [HttpPost]
        public IActionResult Add([FromBody] object book)
        {
            this.logger.LogInformation($"Add book");
            Books.Add(book);
            return this.Ok();
        }
    }
}
