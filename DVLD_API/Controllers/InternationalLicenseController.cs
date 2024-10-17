using AutoMapper;
using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/InternationalLicenses")]
	[ApiController]
	public class InternationalLicenseController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public InternationalLicenseController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewILicense", Name = "AddNewILicense")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<InternationalLicense>> AddNewILicenseAsync(InternationalLicense iLicense)
		{
			if (await _context.InternationalLicenses.AnyAsync(i => i.DriverId == iLicense.DriverId && i.IsActive))
			{
				return BadRequest($"Driver With ID {iLicense.DriverId} Is Already Has International License.");
			}

			// if local license UnActive Cancel Add International License for it.
			if (!await _context.Licenses.AnyAsync(l => l.LicenseId == iLicense.IssuedUsingLocalLicenseId && l.IsActive))
			{
				return BadRequest($"License With ID {iLicense.IssuedUsingLocalLicenseId} Is Not Active.");
			}

			if (!await _context.Users.AnyAsync(u => u.UserId == iLicense.CreatedByUserId))
			{
				return NotFound($"Not Found User With ID {iLicense.CreatedByUserId}");
			}

			if (iLicense.ApplicationId < 1 ||
				iLicense.IssueDate > iLicense.ExpirationDate || 
				iLicense.IssueDate > DateTime.Today)
			{
				return BadRequest("International License Info Is Not Correct.");
			}

			iLicense.InternationalLicenseId = 0; // reset id before save it.
			_context.InternationalLicenses.Add(iLicense);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetILicenseByID", new { id = iLicense.InternationalLicenseId }, iLicense);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding International License.");
		}

		// Update.
		[HttpPut("UpdateILicense/{id}", Name = "UpdateILicense")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<InternationalLicense>> UpdateILicenseAsync(int id, InternationalLicense updateILicense)
		{
			if (id < 1)
				return BadRequest($"Not Accept This ID {id}");

			var oldLicense = await _context.InternationalLicenses.FirstOrDefaultAsync(i => i.InternationalLicenseId == id);
			if (oldLicense == null)
				return NotFound($"Not Found International License With ID {id}.");

			if (oldLicense!.DriverId != updateILicense.DriverId)
			{
				if (!await _context.InternationalLicenses.AnyAsync(i => i.DriverId == updateILicense.DriverId && i.IsActive))
				{
					return BadRequest($"Driver With ID {updateILicense.DriverId} Is Already Has International License.");
				}
			}

			// if local license UnActive Cancel Add International License for it.
			if (!await _context.Licenses.AnyAsync(l => l.LicenseId == updateILicense.IssuedUsingLocalLicenseId && l.IsActive))
			{
				return BadRequest($"License With ID {updateILicense.IssuedUsingLocalLicenseId} Is Not Active.");
			}

			if (!await _context.Users.AnyAsync(u => u.UserId == updateILicense.CreatedByUserId))
			{
				return NotFound($"Not Found User With ID {updateILicense.CreatedByUserId}");
			}

			if (updateILicense.ApplicationId < 1 ||
				updateILicense.IssueDate > updateILicense.ExpirationDate ||
				updateILicense.IssueDate > DateTime.Today)
			{
				return BadRequest("International License Info Is Not Correct.");
			}

			// Update the International License fields
			oldLicense.InternationalLicenseId = id;
			_context.Entry(oldLicense).CurrentValues.SetValues(updateILicense);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(oldLicense);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating International License.");
		}


		// Get By ID.
		[HttpGet("GetILicenseByID/{id}", Name = "GetILicenseByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<InternationalLicense>> GetILicenseByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept International License With ID {id}");
			}

			var iLicense = await _context.InternationalLicenses.AsNoTracking()
									.FirstOrDefaultAsync(i => i.InternationalLicenseId == id);

			if (iLicense == null)
			{
				return NotFound($"Not Found International License With ID {id}");
			}

			return Ok(iLicense);
		}

		// Is Exists By ID.
		[HttpGet("IsILicenseExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsILicenseExistsByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept ID {id} .");

			if (await _context.InternationalLicenses.AnyAsync(i => i.InternationalLicenseId == id))
			{
				return Ok($"International License With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found International License With ID {id}.");
			}
		}

		// Delete By ID.
		[HttpDelete("DeleteILicenseByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteILicenseByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept ID {id} .");

			var iLicense = _context.InternationalLicenses.FirstOrDefault(i => i.InternationalLicenseId == id);

			if (iLicense != null)
			{
				_context.InternationalLicenses.Remove(iLicense);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted International License With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete International License.");
			}
			else
			{
				return NotFound($"Not Found International License With ID {id} .");
			}
		}

		[HttpGet("GetAllInternationalLicenses")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<InternationalLicense>> GetAllInternationalLicenses()
		{
			var iLicense = _context.InternationalLicenses;

			if (!iLicense.Any())
				return NotFound($"Not Found InternationalLicenses");

			return Ok(iLicense);
		}
	}
}
