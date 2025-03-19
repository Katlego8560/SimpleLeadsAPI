using System.Text.Json;
using EasyNetQ;
using Leads.External.Models;
using Messages;
using Microsoft.AspNetCore.Mvc;
using SimpleLeadsAPI.Models;


namespace Leads.External.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController(IBus bus) : ControllerBase
    {
        private readonly string _InternalAPIBaseUrl = "https://localhost:7060/api/leads/";
        private readonly IBus _Bus = bus;

        [HttpGet]
        public async Task<IActionResult> GetLead([FromQuery] Guid id)
        {
             using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_InternalAPIBaseUrl);
           
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
        [HttpGet ("data")]
        public async Task<IActionResult> GetLead([FromQuery] string ContactNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_InternalAPIBaseUrl);

                try
                {
                    using var response = await client.GetAsync($"?contactNumber={ContactNumber}");

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
                client.BaseAddress = new Uri(_InternalAPIBaseUrl);

                try
                {
                    using var response = await client.PostAsJsonAsync(_InternalAPIBaseUrl, model);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();

                        var lead = JsonSerializer.Deserialize<CreatedLeadResponseDto>(responseData,
                           new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                        return CreatedAtAction(nameof(GetLead), lead);
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
                if (ModelState.IsValid)
                {
                    if (ValidateContactNumber(model.ContactNumber) == false
                        || ValidateFullName(model.FullName) == false
                        )
                    {
                        return ValidationProblem(ModelState);
                    }

                    await _Bus.PubSub.PublishAsync(new LeadMessage
                    {
                        FullName = model.FullName,
                        ContactNumber = model.ContactNumber,
                        CurrentlyInsured = model.CurrentlyInsured,
                        OtherInsurer = model.OtherInsurer,
                        Insurer = model.Insurer,
                    });

                    return Accepted();
                }else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }  
        }

        private bool ValidateFullName(string? fullName)
        {
            const string fullNameKey = nameof(Lead.FullName);

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ModelState.AddModelError(fullNameKey, "Please provide a full name");

                return false;
            }
            return true;
            }

         private bool ValidateContactNumber(string? contactNumber)
        {
            const string contactNumberKey = nameof(Lead.ContactNumber);

            if (string.IsNullOrWhiteSpace(contactNumber))
            {
                ModelState.AddModelError(contactNumberKey, "Please provide a contact number.");

                return false;
            }

            if (contactNumber.StartsWith('0') == false
                && contactNumber.StartsWith("+27") == false
                && contactNumber.StartsWith("27") == false)
            {
                ModelState.AddModelError(contactNumberKey, "Please provide a valid contact number.");

                return false;
            }

            if (contactNumber.StartsWith('0') && contactNumber.Length != 10)
            {
                ModelState.AddModelError(contactNumberKey, "Pleaase provide a valid South African number. e.g. 0123456789");

                return false;
            }

            if (contactNumber.StartsWith("+27") &&
               contactNumber.Length != 12)
            {
                ModelState.AddModelError(contactNumberKey, "Pleaase provide a valid South African number. e.g. +27123456789");

                return false;
            }

            if (contactNumber.StartsWith("27") && contactNumber.Length != 11)
            {
                ModelState.AddModelError(contactNumberKey, "Pleaase provide a valid South African number. e.g. 27123456789");

                return false;
            }

            return true;
        }
    }
}
    


