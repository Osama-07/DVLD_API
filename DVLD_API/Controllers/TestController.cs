using AutoMapper;
using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/Test")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public TestController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewTest", Name = "AddNewTest")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Test>> AddNewTestAsync(Test test)
		{
			if (test.TestAppointmentId < 1 ||
				test.CreatedByUserId < 1)
			{
				return BadRequest("Test Info Is Not Correct.");
			}

			test.TestId = 0; // reset id before save it.
			_context.Tests.Add(test);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetTestByID", new { id = test.TestId }, test);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Test.");
		}

		// Update.
		[HttpPut("UpdateTest/{id}", Name = "UpdateTest")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Test>> UpdateTestAsync(int id, Test uTest)
		{
			if (id < 1)
				return BadRequest($"Not Accept This ID {id}.");

			if (uTest.TestAppointmentId < 1 ||
				uTest.CreatedByUserId < 1)
			{
				return BadRequest("Test Info Is Not Correct.");
			}

			var test = await _context.Tests.FirstOrDefaultAsync(t => t.TestId == id);
			if (test == null)
				return NotFound($"Not Found Test With ID {id}.");

			// Update the Driver fields
			uTest.TestId = id;
			_context.Entry(test).CurrentValues.SetValues(uTest);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(test);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Test.");
		}


		// Get By ID.
		[HttpGet("GetTestByID/{id}", Name = "GetTestByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Test>> GetTestByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept Test With ID {id}.");


			var test = await _context.Tests.AsNoTracking()
									.FirstOrDefaultAsync(t => t.TestId == id);

			if (test == null)
			{
				return NotFound($"Not Found Test With ID {id}");
			}

			return Ok(test);
		}

		// Is Exists By ID.
		[HttpGet("IsTestExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsTestExistsByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept ID {id}.");


			if (await _context.Tests.AnyAsync(t => t.TestId == id))
			{
				return Ok($"Test With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Test With ID {id}.");
			}
		}

		// Delete By ID.
		[HttpDelete("DeleteTestByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteTestByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept ID {id}.");
			
			var test = _context.Tests.FirstOrDefault(t => t.TestId == id);

			if (test != null)
			{
				_context.Tests.Remove(test);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Test With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Test.");
			}
			else
			{
				return NotFound($"Not Found Test With ID {id}.");
			}
		}

		[HttpGet("GetAllTests")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Test>> GetAllTests()
		{
			var tests = _context.Tests.AsNoTracking();

			if (!tests.Any())
				return NotFound($"Not Found Tests");

			return Ok(tests);
		}
	}
}
