using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/TestType")]
	[ApiController]
	public class TestTypeController : ControllerBase
	{
		private readonly AppDbContext _context;

		public TestTypeController(AppDbContext context)
		{
			_context = context;
		}

		//// Get By ID.
		[HttpGet("GetTestTypeByID/{id}", Name = "GetTestTypeByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Country>> GetTestTypeByIDAsync(int? id)
		{
			if (id < 1 || id == null)
			{
				return BadRequest($"No Accept Test Type With ID {id}");
			}

			var testType = await _context.TestTypes.FirstOrDefaultAsync(t => t.TestTypeId == id);

			if (testType == null)
			{
				return NotFound($"Not Found Test Type With ID {id}");
			}

			return Ok(testType);
		}

		[HttpGet("GetAllTestTypes")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<TestType>> GetAllTestTypes()
		{
			var testTypes = _context.TestTypes;

			if (!testTypes.Any())
				return NotFound($"Not Found TestTypes");

			return Ok(testTypes);
		}

	}
}
