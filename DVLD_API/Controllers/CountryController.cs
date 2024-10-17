using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/Countries")]
	[ApiController]
	public class CountryController : ControllerBase
	{
		private readonly AppDbContext _context;

		public CountryController(AppDbContext context)
		{
			_context = context;
		}

		// Get By ID.
		[HttpGet("GetCountryByID/{id}", Name = "GetCountryByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Country>> GetCountryByIDAsync(int? id)
		{
			if (id < 1 || id == null)
			{
				return BadRequest($"No Accept Country With ID {id}");
			}

			var country = await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == id);

			if (country == null)
			{
				return NotFound($"Not Found Country With ID {id}");
			}

			return Ok(country);
		}

		[HttpGet("GetAllCountries")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Country>> GetAllCountries()
		{
			var countries = _context.Countries;

			if (countries.Count() == 0)
				return NotFound($"Not Found Countries");

			return Ok(countries);
		}
	}
}
