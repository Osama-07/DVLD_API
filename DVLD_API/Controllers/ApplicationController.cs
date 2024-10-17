using AutoMapper;
using DVLD_API.Data;
using DVLD_API.DTOs;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/Application")]
	[ApiController]
	public class ApplicationController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public ApplicationController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewApplication", Name = "AddNewApplication")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Application>> AddNewApplicationAsync(Application application)
		{
			if (application.ApplicantPersonId < 1 ||
				application.ApplicationTypeId < 1 ||
				application.LastStatusDate > DateTime.Today ||
				application.PaidFees < 0 ||
				application.ApplicationStatus < 0 || 
				application.CreatedByUserId < 1)
			{
				return BadRequest("Application info is not correct.");
			}

			application.ApplicationId = 0; // new Id it will return after save.
			_context.Applications.Add(application);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetApplicationByID", new { id = application.ApplicationId }, application);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Application.");
		}

		//// Update.
		[HttpPut("UpdateApplication/{id}", Name = "UpdateApplication")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Application>> UpdateApplicationAsync(int id, Application application)
		{
			if (id < 1 || id > int.MaxValue)
				return BadRequest($"Not Accept this ID {id}");

			if (application.ApplicantPersonId < 1 || application.ApplicantPersonId > int.MaxValue ||
				application.ApplicationTypeId < 1 || application.ApplicationTypeId > int.MaxValue ||
				application.LastStatusDate != DateTime.Today ||
				application.PaidFees < 0 || application.PaidFees > int.MaxValue ||
				application.ApplicationStatus < 0 || application.ApplicationStatus > byte.MaxValue ||
				application.CreatedByUserId < 1 || application.CreatedByUserId > int.MaxValue)
			{
				return BadRequest("Application info is not correct.");
			}

			var app = await _context.Applications.FirstOrDefaultAsync(p => p.ApplicationId == id);
			if (app == null)
				return NotFound($"Application with ID {id} not found.");

			// Update the person fields
			application.ApplicationId = id;
			_context.Entry(app).CurrentValues.SetValues(application);

			if (await _context.SaveChangesAsync() > 0)
			{
				app = await _context.Applications.AsNoTracking()
					.Include(a => a.ApplicantPerson)
					.Include(a => a.ApplicationType)
					.Include(a => a.CreatedByUser)
						.ThenInclude(u => u!.Person)
					.FirstOrDefaultAsync(p => p.ApplicationId == id);

				return Ok(_mapper.Map<ApplicationDTO>(app));
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error updating Application.");
		}

		//// Get By ID.
		[HttpGet("GetApplicationByID/{id}", Name = "GetApplicationByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Application>> GetApplicationByIDAsync(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"No Accept With ID {id}");
			}

			var Application = await _context.Applications.AsNoTracking()
					.Include(a => a.ApplicantPerson)
					.Include(a => a.ApplicationType)
					.Include(a => a.CreatedByUser)
						.ThenInclude(u => u!.Person)
					.FirstOrDefaultAsync(a => a.ApplicationId == id);

			if (Application == null)
			{
				return NotFound($"No Application With ID {id}");
			}

			return Ok(_mapper.Map<ApplicationDTO>(Application));
		}

		//// Is Exists By ID.
		[HttpGet("IsApplicationExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsApplicationExistsByIDAsync(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"No Accept ID {id} .");
			}

			if (await _context.Applications.AsNoTracking().AnyAsync(a => a.ApplicationId == id))
			{
				return Ok($"Application With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Application With ID {id} .");
			}
		}

		//// Delete By ID.
		[HttpDelete("DeleteApplicationByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteApplicationByIDAsync(int id)
		{
			if (id < 1 || id > int.MaxValue)
			{
				return BadRequest($"No Accept ID {id} .");
			}

			var applicaiton = _context.Applications.FirstOrDefault(a => a.ApplicationId == id);

			if (applicaiton != null)
			{
				_context.Applications.Remove(applicaiton);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Application With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Application.");
			}
			else
			{
				return NotFound($"Not Found Application With ID {id} .");
			}
		}

		[HttpGet("GetAllApplications")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Application>> GetAllApplications()
		{
			var applications = _context.Applications.AsNoTracking()
				.Include(a => a.ApplicantPerson)
				.Include(a => a.ApplicationType)
				.Include(a => a.CreatedByUser)
					.ThenInclude(u => u!.Person);

			if (applications.Count() == 0)
				return NotFound($"Not Found Applications");

			return Ok(_mapper.Map<IEnumerable<ApplicationDTO>>(applications));
		}

	}
}
