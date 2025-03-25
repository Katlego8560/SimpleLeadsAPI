using Microsoft.AspNetCore.Mvc;
using SimpleLeadsAPI.Models;
using SimpleLeadsAPI.Services;


namespace SimpleLeadsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LeadsController : ControllerBase
    {
        private readonly ApplicationDbContext _Context;
        public LeadsController(ApplicationDbContext context)
        {
            _Context = context;
        }

        [HttpGet]
        public ActionResult<LeadDTO> GetLead([FromQuery] GetLeadQueryDto queryDto)
        {
            var leadQuery = _Context
                .Leads
                .AsQueryable();

            if (queryDto.Id.HasValue)
            {
                leadQuery = leadQuery.Where(item => item.Id == queryDto.Id);
            }

            if (!string.IsNullOrWhiteSpace(queryDto.ContactNumber))
            {
                leadQuery = leadQuery.Where(item => item.ContactNumber == queryDto.ContactNumber.Trim());

            }

            var lead = leadQuery.FirstOrDefault();

            if (lead == null)
            {
                return Problem(
                    "Lead not found",
                    string.Empty,
                    404
                    );

            }
            else
            {
                return Ok(lead);
            }
        }

        [HttpPost]
        public ActionResult<LeadDTO> SaveLead([FromBody] CreateLeadDto model)
        {
            if (ModelState.IsValid)
            {
                if (ValidateContactNumber(model.ContactNumber) == false 
                    || ValidateFullName(model.FullName) == false
                    )
                {
                    return ValidationProblem(ModelState);
                }

                var newLead = new Lead
                {
                    Id = Guid.NewGuid(),
                    FullName = model.FullName,
                    ContactNumber = model.ContactNumber,
                    CurrentlyInsured = model.CurrentlyInsured,
                    OtherInsurer = model.OtherInsurer,
                    Insurer = model.Insurer,
                };

                _Context.Leads.Add(newLead);
                _Context.SaveChanges();

                return CreatedAtAction(nameof(GetLead), new { id = newLead.Id });
            }
            else
            {
                return BadRequest(ModelState);
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

