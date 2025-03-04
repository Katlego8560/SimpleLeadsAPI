using System.Net.Http.Headers;
using System.Text.Json;
using Leads.External.Models;
using Microsoft.AspNetCore.Mvc;

namespace Leads.External.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : ControllerBase
    {
        private readonly string InternalAPIBaseUrl = "https://localhost:7060/api/leads/";

        [HttpGet]
        public async Task<ActionResult<LeadDTO>> GetLead([FromQuery] Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InternalAPIBaseUrl);
           

                try
                {
                    var response = await client.GetAsync($"?id={id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var lead = JsonSerializer.Deserialize<LeadDTO>(responseData,
                            new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                        return Ok(lead);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Internal server error: {ex.Message}");
                }
            }
        }
    }
}
