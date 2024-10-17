using AutoMapper;
using DVLD_API.Data;
using DVLD_API.DTOs;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/Drivers")]
	[ApiController]
	public class DriverController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public DriverController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewDriver", Name = "AddNewDriver")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Driver>> AddNewDriverAsync(Driver driver)
		{
			if (await _context.Drivers.AnyAsync(d => d.PersonId == driver.PersonId))
			{
				return BadRequest($"Person With ID {driver.PersonId} Is Already Driver.");
			}

			if (!await _context.People.AnyAsync(p => p.PersonId == driver.PersonId) ||
				!await _context.Users.AnyAsync(u => u.UserId == driver.CreatedByUserId) ||
				driver.CreatedDate != DateTime.Today)
			{
				return BadRequest("Driver Info Is Not Correct.");
			}

			driver.DriverId = 0; // reset id before save it.
			_context.Drivers.Add(driver);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetDriverByID", new { id = driver.DriverId }, driver);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Driver.");
		}

		// Update.
		[HttpPut("UpdateDriver/{id}", Name = "UpdateDriver")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<DriverDTO>> UpdateDriverAsync(int id, Driver uDriver)
		{
			if (id < 1)
				return BadRequest($"Not Accept This ID {id}");

			if (uDriver.PersonId < 1 ||
				uDriver.CreatedByUserId < 1 ||
				uDriver.CreatedDate > DateTime.Today)
			{
				return BadRequest("Driver Info Is Not Correct.");
			}

			var driver = await _context.Drivers.FirstOrDefaultAsync(d => d.DriverId == id);
			if (driver == null)
				return NotFound($"Not Found Driver With ID {id}.");

			// if change person, it will verify person if has driver or not.
			if (driver.PersonId != uDriver.PersonId)
			{
				if (await _context.Drivers.AnyAsync(d => d.PersonId == uDriver.PersonId))
					return BadRequest($"Person With ID {uDriver.PersonId} Is Already Driver.");
			}

			// Update the Driver fields
			uDriver.DriverId = id;
			_context.Entry(driver).CurrentValues.SetValues(uDriver);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(driver);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Driver.");
		}


		// Get By ID.
		[HttpGet("GetDriverByID/{id}", Name = "GetDriverByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<DriverDTO>> GetDriverByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept Driver With ID {id}");
			}

			var driver = await _context.Drivers.AsNoTracking()
									.Include(d => d.Person)
										.ThenInclude(p => p!.Country)
									.Include(d => d.CreatedByUser)
									.FirstOrDefaultAsync(d => d.DriverId == id);

			if (driver == null)
			{
				return NotFound($"Not Found Driver With ID {id}");
			}

			return Ok(_mapper.Map<DriverDTO>(driver));
		}

		// Is Exists By ID.
		[HttpGet("IsDriverExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsDriverExistsByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept ID {id} .");
			}

			if (await _context.Drivers.AnyAsync(d => d.DriverId == id))
			{
				return Ok($"Driver With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Driver With ID {id} .");
			}
		}

		// Delete By ID.
		[HttpDelete("DeleteDriverByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteDriverByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept ID {id} .");
			}

			var driver = _context.Drivers.FirstOrDefault(d => d.DriverId == id);

			if (driver != null)
			{
				_context.Drivers.Remove(driver);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Driver With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Driver.");
			}
			else
			{
				return NotFound($"Not Found Driver With ID {id} .");
			}
		}

		[HttpGet("GetAllDrivers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Driver>> GetAllDrivers()
		{
			var drivers = _context.Drivers.AsNoTracking()
									.Include(d => d.Person)
										.ThenInclude(p => p!.Country)
									.Include(d => d.CreatedByUser);

			if (!drivers.Any())
				return NotFound($"Not Found Drivers");

			return Ok(_mapper.Map<IEnumerable<DriverDTO>>(drivers));
		}
	}
}
