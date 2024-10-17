using AutoMapper;
using DVLD_API.Data;
using DVLD_API.DTOs;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/Licenses")]
	[ApiController]
	public class LicenseController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public LicenseController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewLicense", Name = "AddNewLicense")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<License>> AddNewLicenseAsync(License license)
		{
			if (await _context.Licenses.AnyAsync(l => 
												  l.DriverId == license.DriverId && 
												  l.LicenseClass == license.LicenseClass &&
												  l.ExpirationDate < DateTime.Now && 
												  l.IsActive))
			{
				return BadRequest("This Driver Is Already Has License.");
			}

			if (!await _context.Users.AnyAsync(u => u.UserId == license.CreatedByUserId))
			{
				return BadRequest($"Not Found User With ID {license.CreatedByUserId}.");
			}

			if (license.ApplicationId < 1 ||
				license.IssueDate > license.ExpirationDate || 
				license.IssueDate > DateTime.Today)
			{
				return BadRequest($"Your Info Is Not Correct.");
			}

			license.LicenseId = 0; // reset id before save it.
			_context.Licenses.Add(license);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetLicenseByID", new { id = license.LicenseId }, license);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding License.");
		}

		// Update.
		[HttpPut("UpdateLicense/{id}", Name = "UpdateLicense")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<License>> UpdateLicenseAsync(int id, License uLicense)
		{
			if (id < 1)
				return BadRequest($"Not Accept this ID {id}");

			var license = await _context.Licenses.FirstOrDefaultAsync(l => l.LicenseId == id);
			if (license == null)
				return NotFound($"Not Found License With ID {id}.");

			if (license.DriverId != uLicense.DriverId)
			{
				if (await _context.Licenses.AnyAsync(l => 
													 l.DriverId == uLicense.DriverId &&
													 l.LicenseClass == uLicense.LicenseClass &&
													 l.IsActive))
				{

					return BadRequest($"Driver With ID {uLicense.DriverId} Is Already Has License.");
				}	
			}

			if (!await _context.Users.AnyAsync(u => u.UserId == license.CreatedByUserId))
			{
				return BadRequest($"Not Found User With ID {license.CreatedByUserId}.");
			}

			if (license.ApplicationId < 1 ||
				license.IssueDate > license.ExpirationDate ||
				license.IssueDate > DateTime.Today)
			{
				return BadRequest($"Your Info Is Not Correct.");
			}

			// Update the license fields
			_context.Entry(license).CurrentValues.SetValues(uLicense);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(license);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating License.");
		}


		// Get By ID.
		[HttpGet("GetLicenseByID/{id}", Name = "GetLicenseByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<License>> GetLicenseByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept License With ID {id}");
			}

			var license = await _context.Licenses.FirstOrDefaultAsync(l => l.LicenseId == id);
			if (license == null)
			{
				return NotFound($"Not Found License With ID {id}.");
			}

			return Ok(license);
		}

		// Is Exists By ID.
		[HttpGet("IsLicenseExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsLicenseExistsByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept License ID {id}.");
			}

			if (await _context.Licenses.AnyAsync(l => l.LicenseId == id))
			{
				return Ok($"License With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found License With ID {id}.");
			}
		}

		//// Delete By ID.
		[HttpDelete("DeleteLicenseByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteLicenseByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept License With ID {id}.");
			}

			var license = _context.Licenses.FirstOrDefault(l => l.LicenseId == id);

			if (license != null)
			{
				_context.Licenses.Remove(license);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted License With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete License.");
			}
			else
			{
				return NotFound($"Not Found License With ID {id}.");
			}
		}

		[HttpGet("GetAllLicenses")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<License>> GetAllLicenses()
		{
			var licenses = _context.Licenses.AsNoTracking();

			if (!licenses.Any())
				return NotFound($"Not Found Licenses");

			return Ok(licenses);
		}

	}
}
