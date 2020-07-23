namespace SampleASP1.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> logger;

        public BookController(ILogger<BookController> logger)
        {
            this.logger = logger;
        }
    }
}
