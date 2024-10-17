using AutoMapper;
using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/LocalDrivingLicenseApplication")]
	[ApiController]
	public class LocalDrivingLicenseApplicationController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public LocalDrivingLicenseApplicationController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewLocalDrivingLicenseApplication", Name = "AddNewLocalDrivingLicenseApplication")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LocalDrivingLicenseApplication>> AddNewLocalDrivingLicenseApplicationAsync
			(LocalDrivingLicenseApplication l_d_lApplication)
		{
			if (l_d_lApplication.ApplicationId < 1 ||
				l_d_lApplication.LicenseClassId < 1)
			{
				return BadRequest($"Your Info Is Not Correct.");
			}

			l_d_lApplication.LocalDrivingLicenseApplicationId = 0; // reset id before save it.
			_context.LocalDrivingLicenseApplications.Add(l_d_lApplication);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetLocalDrivingLicenseApplicationByID",
					new { id = l_d_lApplication.LocalDrivingLicenseApplicationId }, l_d_lApplication);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Local Driving License Application.");
		}

		// Update.
		[HttpPut("UpdateLocalDrivingLicenseApplication/{id}", Name = "UpdateLocalDrivingLicenseApplication")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<LocalDrivingLicenseApplication>> UpdateLocalDrivingLicenseApplicationAsync(int id,
			LocalDrivingLicenseApplication ul_d_lApplication)
		{
			if (id < 1)
				return BadRequest($"Not Accept This ID {id}");

			if (ul_d_lApplication.ApplicationId < 1 ||
				ul_d_lApplication.LicenseClassId < 1)
			{
				return BadRequest($"Your Info Is Not Correct.");
			}

			var l_d_lApplication = await _context.LocalDrivingLicenseApplications.FirstOrDefaultAsync(l => l.LocalDrivingLicenseApplicationId == id);
			if (l_d_lApplication == null)
				return NotFound($"Not Found Local Driving License Application With ID {id}.");

			// Update the Driver fields
			ul_d_lApplication.LocalDrivingLicenseApplicationId = id;
			_context.Entry(l_d_lApplication).CurrentValues.SetValues(ul_d_lApplication);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(l_d_lApplication);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Local Driving License Application.");
		}


		// Get By ID.
		[HttpGet("GetLocalDrivingLicenseApplicationByID/{id}", Name = "GetLocalDrivingLicenseApplicationByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LocalDrivingLicenseApplication>> GetLocalDrivingLicenseApplicationByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept Local Driving License Application With ID {id}");
			}

			var l_d_lApplication = await _context.LocalDrivingLicenseApplications.AsNoTracking()
									.FirstOrDefaultAsync(l => l.LocalDrivingLicenseApplicationId == id);

			if (l_d_lApplication == null)
			{
				return NotFound($"Not Found Local Driving License Application With ID {id}");
			}

			return Ok(l_d_lApplication);
		}

		// Is Exists By ID.
		[HttpGet("IsLocalDrivingLicenseApplicationExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsLocalDrivingLicenseApplicationExistsByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept ID {id} .");
			}

			if (await _context.LocalDrivingLicenseApplications.AnyAsync(l => l.LocalDrivingLicenseApplicationId == id))
			{
				return Ok($"Local Driving License Application With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Local Driving License Application With ID {id} .");
			}
		}

		// Delete By ID.
		[HttpDelete("DeleteLocalDrivingLicenseApplicationByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteLocalDrivingLicenseApplicationByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept ID {id}.");
			}

			var l_d_lApplication = _context.LocalDrivingLicenseApplications.FirstOrDefault(l => l.LocalDrivingLicenseApplicationId == id);

			if (l_d_lApplication != null)
			{
				_context.LocalDrivingLicenseApplications.Remove(l_d_lApplication);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Local Driving License Application With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Local Driving License Application.");
			}
			else
			{
				return NotFound($"Not Found Local Driving License Application With ID {id}.");
			}
		}

		[HttpGet("GetAllLocalDrivingLicenseApplications")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<LocalDrivingLicenseApplication>> GetAllLocalDrivingLicenseApplication()
		{
			var l_d_l_Applications = _context.LocalDrivingLicenseApplications.AsNoTracking();

			if (!l_d_l_Applications.Any())
				return NotFound($"Not Found Local Driving License Applications");

			return Ok(l_d_l_Applications);
		}

	}
}
