using System.Text.Json;
using EasyNetQ;
using Leads.External.Models;
using Microsoft.AspNetCore.Mvc;

namespace Leads.External.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController(IBus bus) : ControllerBase
    {
        private readonly string InternalAPIBaseUrl = "https://localhost:7060/api/leads/";
        private readonly IBus _Bus = bus;

        [HttpGet]
        public async Task<IActionResult> GetLead([FromQuery] Guid id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InternalAPIBaseUrl);
           

                try
                {
                   using var response = await client.GetAsync($"?id={id}");

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
                catch (Exception)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveLead([FromBody] CreateLeadDto model)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(InternalAPIBaseUrl);

                try
                {
                     using var response = await client.PostAsJsonAsync(InternalAPIBaseUrl, model);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        var lead = JsonSerializer.Deserialize<CreatedLeadResponseDto>(responseData,
                           new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                        return CreatedAtAction(nameof(SaveLead), lead);

                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
        }

        [HttpPost("enqueue")]
        public async Task<IActionResult> EnqueueLead([FromBody] CreateLeadDto model)
        {
            try
            {
                await _Bus.PubSub.PublishAsync(model);

                return Accepted();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }  
        }
    }
}
