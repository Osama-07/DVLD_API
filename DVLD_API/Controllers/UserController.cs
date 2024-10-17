using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DVLD_API.Controllers
{
	[Route("api/User")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly AppDbContext _context;

		public UserController(AppDbContext context)
		{
			_context = context;
		}

		// Add New.
		[HttpPost("AddNewUser", Name = "AddNewUser")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<User>> AddNewUserAsync(User user)
		{
			if (user.PersonId < 1 ||
				user.Username.IsNullOrEmpty() ||
				user.Password.IsNullOrEmpty())
			{
				return BadRequest("Your Info Is Not Correct.");
			}

			if (await _context.Users.AnyAsync(u => u.PersonId == user.PersonId))
			{
				return BadRequest($"Person Wiht Id {user.PersonId} Is Already User.");
			}

			if (!await _context.People.AnyAsync(p => p.PersonId == user.PersonId))
			{
				return NotFound($"Not Found Person With ID {user.PersonId}.");
			}

			if (await _context.Users.AnyAsync(u => u.Username == user.Username))
			{
				return BadRequest($"This Username \'{user.Username}\' Is Already Uses");
			}

			user.UserId = 0; // reset id before save it.
			_context.Users.Add(user);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetUserByID", new { id = user.UserId }, user);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding User.");
		}

		//// Update.
		[HttpPut("UpdateUser/{id}", Name = "UpdateUser")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<User>> UpdateUserAsync(int id, User uUser)
		{
			if (id < 1)
				return BadRequest($"Not Accept User With ID {id}.");

			if (uUser.PersonId < 1 || 
				uUser.Username.IsNullOrEmpty() ||
				uUser.Password.IsNullOrEmpty())
			{
				return BadRequest("Your Info Is Not Correct.");
			}

			var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);
			if (user == null)
				return NotFound($"Not Found User With ID {id}.");

			if (user.PersonId != uUser.PersonId)
			{
				if (await _context.Users.AnyAsync(u => u.PersonId == uUser.PersonId))
				{
					return BadRequest($"Person Wiht Id {uUser.PersonId} Is Already User.");
				}

				if (!await _context.People.AnyAsync(p => p.PersonId == uUser.PersonId))
				{
					return NotFound($"Not Found Person With ID {uUser.PersonId}.");
				}
			}

			if (user.Username != uUser.Username)
			{
				if (await _context.Users.AnyAsync(u => u.Username == uUser.Username))
				{
					return BadRequest($"This Username \'{uUser.Username}\' Is Already Uses");
				}
			}

			// Update the User fields
			_context.Entry(user).CurrentValues.SetValues(uUser);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(user);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating User.");
		}


		//// Get By ID.
		[HttpGet("GetUserByID/{id}", Name = "GetUserByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<User>> GetUserByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept User With ID {id}.");
			}

			var person = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

			if (person == null)
			{
				return NotFound($"Not Found User With ID {id}.");
			}

			return Ok(person);
		}

		//// Is Exists By ID.
		[HttpGet("IsUserExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsUserExistsByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"Not Accept User With ID {id}.");
			}

			if (await _context.Users.AnyAsync(u => u.UserId == id))
			{
				return Ok($"User With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found User With ID {id}.");
			}
		}

		//// Delete By ID.
		[HttpDelete("DeleteUserByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteUserByIDAsync(int id)
		{
			if (id < 1)
			{
				return BadRequest($"No Accept ID {id}.");
			}

			var user = _context.Users.FirstOrDefault(u => u.UserId == id);

			if (user != null)
			{
				_context.Users.Remove(user);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted User With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete User.");
			}
			else
			{
				return NotFound($"Not Found User With ID {id}.");
			}
		}

		[HttpGet("GetAllUsers")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<User>> GetAllUsers()
		{
			var users = _context.Users.AsNoTracking();

			if (!users.Any())
				return NotFound($"Not Found Users.");

			return Ok(users);
		}

	}
}
