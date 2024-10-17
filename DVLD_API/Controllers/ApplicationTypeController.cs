using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/ApplicationType")]
	[ApiController]
	public class ApplicationTypeController : ControllerBase
	{
		private readonly AppDbContext _context;

		public ApplicationTypeController(AppDbContext context)
		{
			_context = context;
		}

		// Add New.
		[HttpPost("AddNewApplicationType")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApplicationType>> AddNewApplicationTypeAsync(ApplicationType applicationType)
		{
			if (string.IsNullOrEmpty(applicationType.ApplicationTypeTitle) || 
				applicationType.ApplicationFees < 0 || applicationType.ApplicationFees > decimal.MaxValue)
			{
				return BadRequest("Application Type info is not correct.");
			}

			applicationType.ApplicationTypeId = 0; // new Id it will return after save.
			_context.ApplicationTypes.Add(applicationType);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetApplicationTypeByID", new { id = applicationType.ApplicationTypeId }, applicationType);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Application Type.");
		}

		//// Update.
		[HttpPut("UpdateApplicationType/{id}", Name = "UpdateApplicationType")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<ApplicationType>> UpdateApplicationTypeAsync(int id, ApplicationType applicationType)
		{
			if (id < 1 || id > int.MaxValue)
				return BadRequest($"Not Accept this ID {id}");

			if (string.IsNullOrEmpty(applicationType.ApplicationTypeTitle) ||
				applicationType.ApplicationFees < 0 || applicationType.ApplicationFees > decimal.MaxValue)
			{
				return BadRequest("Application Type info is not correct.");
			}

			var app = await _context.ApplicationTypes.FirstOrDefaultAsync(p => p.ApplicationTypeId == id);
			if (app == null)
				return NotFound($"Not Found Application Type With ID {id}.");

			// Update the person fields
			applicationType.ApplicationTypeId = id;
			_context.Entry(app).CurrentValues.SetValues(applicationType);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(applicationType);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Application Type.");
		}

		//// Get By ID.
		[HttpGet("GetApplicationTypeByID/{id}", Name = "GetApplicationTypeByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<ApplicationType>> GetApplicationTypeByIDAsync(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"No Accept Application Type With ID {id}");
			}

			var applicationType = await _context.ApplicationTypes.AsNoTracking()
					.FirstOrDefaultAsync(a => a.ApplicationTypeId == id);

			if (applicationType == null)
			{
				return NotFound($"Not Found Application Type With ID {id}");
			}

			return Ok(applicationType);
		}

		//// Is Exists By ID.
		[HttpGet("IsApplicationTypeExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsApplicationTypeExistsByIDAsync(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"No Accept Application Type With ID {id}.");
			}

			if (await _context.ApplicationTypes.AsNoTracking().AnyAsync(a => a.ApplicationTypeId == id))
			{
				return Ok($"Application Type With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Application Type With ID {id} .");
			}
		}

		//// Delete By ID.
		[HttpDelete("DeleteApplicationTypeByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteApplicationTypeByIDAsync(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"No Accept Application Type With ID {id} .");
			}

			var applicaiton = _context.ApplicationTypes.FirstOrDefault(a => a.ApplicationTypeId == id);

			if (applicaiton != null)
			{
				_context.ApplicationTypes.Remove(applicaiton);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Application Type With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Application Type.");
			}
			else
			{
				return NotFound($"Not Found Application Type With ID {id} .");
			}
		}

		[HttpGet("GetAllApplicationTypes")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<ApplicationType>> GetAllApplicationTypes()
		{
			var applicationTypes = _context.ApplicationTypes;

			if (applicationTypes.Count() == 0)
				return NotFound($"Not Found Application Types.");

			return Ok(applicationTypes);
		}
	}
}
