using AutoMapper;
using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/DetainedLicenses")]
	[ApiController]
	public class DetainedLicenseController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public DetainedLicenseController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewDetaineLicense")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<DetainedLicense>> AddNewDetaineLicenseAsync(DetainedLicense detaineLicense)
		{
			// check Is License Exists.
			if (!await _context.Licenses.AnyAsync(l => l.LicenseId == detaineLicense.LicenseId))
			{
				return BadRequest($"Not Found License With ID {detaineLicense.LicenseId}");
			}
			// check Is License already Detained.
			if (await _context.DetainedLicenses.AnyAsync(d => d.LicenseId == detaineLicense.LicenseId && !d.IsReleased))
			{
				return BadRequest($"This License With ID {detaineLicense.LicenseId} Is Already Detained.");
			}
			// check if info is correct.
			if (detaineLicense.DetainDate > DateTime.Today ||
				detaineLicense.FineFees < 0 || detaineLicense.FineFees > decimal.MaxValue ||
				!await _context.Users.AnyAsync(u => u.UserId == detaineLicense.CreatedByUserId))
			{
				return BadRequest($"Your Info Is Not Correct.");
			}
			// check if Release Info is Correct.
			if (detaineLicense.ReleaseDate != null &&
				detaineLicense.ReleasedByUserId != null &&
				detaineLicense.ReleaseApplicationId != null)
			{
				if (!await _context.Applications.AnyAsync(a => a.ApplicationId == detaineLicense.ReleaseApplicationId) ||
					!await _context.Users.AnyAsync(u => u.UserId == detaineLicense.ReleasedByUserId) || 
					detaineLicense.ReleaseDate > DateTime.Today)
				{
					return BadRequest("Your Release Info Is Not Correct.");
				}
			}

			detaineLicense.DetainId = 0; // reset id before save it
			_context.DetainedLicenses.Add(detaineLicense);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetDetainedLicenseByID", new { id = detaineLicense.DetainId }, detaineLicense);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Detaine License.");
		}

		//// Update.
		[HttpPut("UpdateDetaineLicense/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<DetainedLicense>> UpdateDetaineLicenseAsync(int id, DetainedLicense uDetaineLicense)
		{
			if (id < 1 || id > int.MaxValue)
				return BadRequest($"Not Accept this ID {id}");

			// check Is License Exists.
			if (!await _context.Licenses.AnyAsync(l => l.LicenseId == uDetaineLicense.LicenseId))
			{
				return BadRequest($"Not Found License With ID {uDetaineLicense.LicenseId}");
			}
			// check if info is correct.
			if (uDetaineLicense.DetainDate > DateTime.Today ||
				uDetaineLicense.FineFees < 0 || uDetaineLicense.FineFees > decimal.MaxValue ||
				!await _context.Users.AnyAsync(u => u.UserId == uDetaineLicense.CreatedByUserId))
			{
				return BadRequest($"Your Info Is Not Correct.");
			}
			// check if Release Info is Correct.
			if (uDetaineLicense.ReleaseDate != null &&
				uDetaineLicense.ReleasedByUserId != null &&
				uDetaineLicense.ReleaseApplicationId != null)
			{
				if (!await _context.Applications.AnyAsync(a => a.ApplicationId == uDetaineLicense.ReleaseApplicationId) ||
					!await _context.Users.AnyAsync(u => u.UserId == uDetaineLicense.ReleasedByUserId) ||
					uDetaineLicense.ReleaseDate > DateTime.Today)
				{
					return BadRequest("Your Release Info Is Not Correct.");
				}
			}

			var detained = await _context.DetainedLicenses.FirstOrDefaultAsync(d => d.DetainId == id);
			
			if (detained == null)
				return NotFound($"Not Found Detained License With ID {id}.");

			_context.Entry(detained).CurrentValues.SetValues(uDetaineLicense);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(detained);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Detained License.");
		}

		//// Get By ID.
		[HttpGet("GetDetainedLicenseByID/{id}", Name = "GetDetainedLicenseByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<DetainedLicense>> GetDetainedLicenseByID(int id)
		{
			if (id < 1 || id > int.MaxValue)
				return BadRequest($"No Accept Detained License With ID {id}");

			var detained = await _context.DetainedLicenses.AsNoTracking().FirstOrDefaultAsync(d => d.DetainId == id);

			if (detained == null)
			{
				return NotFound($"Not Found Detained License With ID {id}");
			}

			return Ok(detained);
		}

		//// Is Exists By ID.
		[HttpGet("IsDetainedLicenseExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsDetainedLicenseExistsByIDAsync(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"Not Accept Detained License ID {id} .");
			}

			if (await _context.DetainedLicenses.AnyAsync(d => d.DetainId == id))
			{
				return Ok($"Detained License With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Detained License With ID {id}.");
			}
		}

		//// Delete By ID.
		[HttpDelete("DeleteDetainedLicenseByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteDetainedLicenseByID(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"Not Accept Detained License ID {id} .");
			}

			var detained = _context.DetainedLicenses.FirstOrDefault(d => d.DetainId == id);

			if (detained != null)
			{
				_context.DetainedLicenses.Remove(detained);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Detained License With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Detained License.");
			}
			else
			{
				return NotFound($"Not Found Detained License With ID {id} .");
			}
		}

		[HttpGet("GetAllDetainedLicenses")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<DetainedLicense>> GetAllDetainedLicenses()
		{
			var detainedLicenses = _context.DetainedLicenses.AsNoTracking();

			if (detainedLicenses.Count() == 0)
				return NotFound($"Not Found Detained Licenses.");

			return Ok(detainedLicenses);
		}

	}
}
