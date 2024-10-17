using AutoMapper;
using DVLD_API.Data;
using DVLD_API.DTOs;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DVLD_API.Controllers
{
	[Route("api/People")]
	[ApiController]
	public class PersonController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public PersonController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		// Add New.
		[HttpPost("AddNewPerson", Name = "AddNewPerson")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Person>> AddNewPersonAsync(Person person)
		{
			if (string.IsNullOrEmpty(person.NationalNo) ||
				string.IsNullOrEmpty(person.FirstName) ||
				string.IsNullOrEmpty(person.SecondName) ||
				string.IsNullOrEmpty(person.LastName) ||
				string.IsNullOrEmpty(person.Email) ||
				string.IsNullOrEmpty(person.Phone) ||
				person.DateOfBirth > DateTime.Now.AddYears(-18) ||
				person.CountryId < 1 ||
				person.Gender < 0 || person.Gender > 1)
			{
				return BadRequest("Your Info Is Not Correct.");
			}

			person.PersonId = 0; // reset id before save it.
			_context.People.Add(person);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetPersonByID", new { id = person.PersonId }, person);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Person.");
		}

		//// Update.
		[HttpPut("UpdatePerson/{id}", Name = "UpdatePerson")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<Person>> UpdatePersonAsync(int id, Person updatedPerson)
		{
			if (id < 1)
				return BadRequest($"Not Accept Person With ID {id}.");

			if (string.IsNullOrEmpty(updatedPerson.NationalNo) ||
				string.IsNullOrEmpty(updatedPerson.FirstName) ||
				string.IsNullOrEmpty(updatedPerson.SecondName) ||
				string.IsNullOrEmpty(updatedPerson.LastName) ||
				string.IsNullOrEmpty(updatedPerson.Email) ||
				string.IsNullOrEmpty(updatedPerson.Phone) ||
				updatedPerson.CountryId < 1 ||
				updatedPerson.Gender < 0 || updatedPerson.Gender > 1 ||
				(DateTime.Now - updatedPerson.DateOfBirth).TotalDays < 18 * 365)
			{
				return BadRequest("Your Info Is Not Correct.");
			}

			var person = await _context.People.FirstOrDefaultAsync(p => p.PersonId == id);
			if (person == null)
				return NotFound($"Not Found Person With ID {id}.");

			// Update the person fields
			_context.Entry(person).CurrentValues.SetValues(updatedPerson);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(person);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Person.");
		}


		//// Get By ID.
		[HttpGet("GetPersonByID/{id}", Name = "GetPersonByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Person>> GetPersonByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept Person With ID {id}.");
			}

			var person = await _context.People.Include(p => p.Country).FirstOrDefaultAsync(p => p.PersonId == id);

			if (person == null)
			{
				return NotFound($"Not Found Person With ID {id}");
			}

			return Ok(person);
		}

		//// Get By NationalNo.
		[HttpGet("GetPersonByNationalNo/{nationalNo}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<Person>> GetPersonByNationalNoAsync(string nationalNo)
		{
			if (string.IsNullOrEmpty(nationalNo) || nationalNo.Length > 20)
			{
				return BadRequest($"Not Accept Person With NationalNo \"{nationalNo}\"");
			}

			var person = await _context.People.AsNoTracking().FirstOrDefaultAsync(p => p.NationalNo == nationalNo);
			if (person == null)
			{
				return NotFound($"Not Found Person With NationalNo \"{nationalNo}\"");
			}

			return Ok(person);
		}

		//// Is Exists By ID.
		[HttpGet("IsPersonExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsPersonExistsByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept Person With ID {id}.");
			}

			if (await _context.People.AnyAsync(p => p.PersonId == id))
			{
				return Ok($"Person With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Person With ID {id}.");
			}
		}

		//// Is Exists By NationalNo.
		[HttpGet("IsPersonExistsByNationalNo/{nationalNo}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsPersonExistsByNationalNoAsync(string nationalNo)
		{
			if (string.IsNullOrEmpty(nationalNo))
			{
				return BadRequest($"Not Accept Person With NationalNo \"{nationalNo}\".");
			}

			if (await _context.People.AsNoTracking().AnyAsync(p => p.NationalNo == nationalNo))
			{
				return Ok($"Person With NationalNo \"{nationalNo}\" Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Person With NationalNo \"{nationalNo}\".");
			}
		}

		//// Delete By ID.
		[HttpDelete("DeletePersonByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeletePersonByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"No Accept ID {id} .");
			}

			var person = _context.People.FirstOrDefault(p => p.PersonId == id);

			if (person != null)
			{
				_context.People.Remove(person);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Person With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Person.");
			}
			else
			{
				return NotFound($"Not Found Person With ID {id} .");
			}
		}

		[HttpGet("GetAllPeople")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<Person>> GetAllPeople()
		{
			var people = _context.People.AsNoTracking().Include(p => p.Country);

			if (!people.Any())
				return NotFound($"Not Found People.");

			return Ok(_mapper.Map<IEnumerable<PersonDTO>>(people));
		}
	}
}
