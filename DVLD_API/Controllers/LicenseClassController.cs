using AutoMapper;
using DVLD_API.Data;
using DVLD_API.DTOs;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/LicenseClass")]
	[ApiController]
	public class LicenseClassController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public LicenseClassController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewLicenseClass", Name = "AddNewLicenseClass")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LicenseClass>> AddNewLicenseClassAsync(LicenseClass licenseClass)
		{
			if (string.IsNullOrEmpty(licenseClass.ClassName) ||
				string.IsNullOrEmpty(licenseClass.ClassDescription) ||
				licenseClass.MinimumAllowedAge < 15 ||
				licenseClass.DefaultValidityLength < 1 ||
				licenseClass.ClassFees < 0)
			{
				return BadRequest($"Your License Class Info Is Not Correct.");
			}

			licenseClass.LicenseClassId = 0; // reset id before save it.
			_context.LicenseClasses.Add(licenseClass);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetLicenseClassByID", new { id = licenseClass.LicenseClassId }, licenseClass);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding License Class.");
		}

		// Update.
		[HttpPut("UpdateLicenseClass/{id}", Name = "UpdateLicenseClass")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<LicenseClass>> UpdateLicenseClassAsync(int id, LicenseClass uLicenseClass)
		{
			if (id < 1)
				return BadRequest($"Not Accept This ID {id}.");

			if (string.IsNullOrEmpty(uLicenseClass.ClassName) ||
				string.IsNullOrEmpty(uLicenseClass.ClassDescription) ||
				uLicenseClass.MinimumAllowedAge < 15 || 
				uLicenseClass.DefaultValidityLength < 1 || 
				uLicenseClass.ClassFees < 0)
			{
				return BadRequest($"Your License Class Info Is Not Correct.");
			}


			var licenseClass = await _context.LicenseClasses.FirstOrDefaultAsync(l => l.LicenseClassId == id);
			if (licenseClass == null)
				return NotFound($"Not Found License Class With ID {id}.");

			// Update the Driver fields
			uLicenseClass.LicenseClassId = id;
			_context.Entry(licenseClass).CurrentValues.SetValues(uLicenseClass);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(licenseClass);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating License Class.");
		}


		// Get By ID.
		[HttpGet("GetLicenseClassByID/{id}", Name = "GetLicenseClassByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<LicenseClass>> GetLicenseClassByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept License Class With ID {id}.");
			}

			var licenseClass = await _context.LicenseClasses.AsNoTracking()
									.FirstOrDefaultAsync(l => l.LicenseClassId == id);

			if (licenseClass == null)
			{
				return NotFound($"Not Found License Class With ID {id}.");
			}

			return Ok(licenseClass);
		}

		// Is Exists By ID.
		[HttpGet("IsLicenseClassExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsLicenseClassExistsByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept ID {id}.");
			}

			if (await _context.LicenseClasses.AnyAsync(l => l.LicenseClassId == id))
			{
				return Ok($"License Class With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found License Class With ID {id}.");
			}
		}

		// Delete By ID.
		[HttpDelete("DeleteLicenseClassByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteDriverByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept ID {id}.");
			}

			var licenseClass = _context.LicenseClasses.FirstOrDefault(l => l.LicenseClassId == id);

			if (licenseClass != null)
			{
				_context.LicenseClasses.Remove(licenseClass);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted License Class With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete License Class.");
			}
			else
			{
				return NotFound($"Not Found License Class With ID {id}.");
			}
		}

		[HttpGet("GetAllLicenseClasses")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<LicenseClass>> GetAllDrivers()
		{
			var licenseClasses = _context.LicenseClasses.AsNoTracking();

			if (!licenseClasses.Any())
				return NotFound($"Not Found License Classes.");

			return Ok(licenseClasses);
		}

	}
}
