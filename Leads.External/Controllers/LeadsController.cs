using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Leads.External.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly string baseUrl = "https://localhost:7060/api/leads/";

        [HttpGet]
        public IActionResult GetLead([FromQuery] Guid id)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

           // var response = client.GetAsync("?" + id);

            return Ok();
        }
    }
}
