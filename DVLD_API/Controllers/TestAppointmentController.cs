using AutoMapper;
using DVLD_API.Data;
using DVLD_API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace DVLD_API.Controllers
{
	[Route("api/TestAppointment")]
	[ApiController]
	public class TestAppointmentController : ControllerBase
	{
		private readonly AppDbContext _context;
		private readonly IMapper _mapper;

		public TestAppointmentController(AppDbContext context, IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		private async Task<bool> _CheckPersonPassedTest(TestAppointment testAppointment)
		{
			// verify if person is already passed for this test type.
			var found = await _context.Tests
									.Join(_context.TestAppointments,
										test => test.TestAppointmentId,
										appointment => appointment.TestAppointmentId,
										(test, appointment) => new { test, appointment })
									.Where(joined =>
										joined.appointment.IsLocked &&
										joined.appointment.LocalDrivingLicenseApplicationId == testAppointment.LocalDrivingLicenseApplicationId &&
										joined.appointment.TestTypeId == testAppointment.TestTypeId &&
										joined.test.TestResult)
									.Select(_ => 1)
									.AnyAsync();

			return found;
		}
		private async Task<bool> _CheckActiveTest(TestAppointment testAppointment)
		{
			// check if there the same test is active.
			var found = await _context.TestAppointments.AnyAsync(t => !t.IsLocked &&
											t.LocalDrivingLicenseApplicationId == testAppointment.LocalDrivingLicenseApplicationId &&
											t.TestTypeId == testAppointment.TestTypeId);

			return found;
		}

		// Add New.
		[HttpPost("AddNewTestAppointment", Name = "AddNewTestAppointment")]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<TestAppointment>> AddNewTestAppointmentAsync(TestAppointment testAppointment)
		{
			if (testAppointment.TestTypeId < 1 ||
				testAppointment.LocalDrivingLicenseApplicationId < 1 ||
				testAppointment.AppointmentDate > DateTime.Today ||
				testAppointment.PaidFees < 0 ||
				testAppointment.CreatedByUserId < 1)
			{
				return BadRequest("Test Appointment Info Is Not Correct.");
			}

			if (await _CheckActiveTest(testAppointment))
			{
				return BadRequest("He Is Already has Active Test For This Test Type.");
			}

			if (await _CheckPersonPassedTest(testAppointment))
			{
				return BadRequest("He Is Already Passed For This Test Type.");
			}

			testAppointment.TestAppointmentId = 0; // reset id before save it.
			_context.TestAppointments.Add(testAppointment);

			if (await _context.SaveChangesAsync() > 0)
			{
				return CreatedAtRoute("GetTestAppointmentByID", new { id = testAppointment.TestAppointmentId }, testAppointment);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Adding Test Appointment.");
		}

		// Update.
		[HttpPut("UpdateTestAppointment/{id}", Name = "UpdateTestAppointment")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<TestAppointment>> UpdateTestAppointmentAsync(int id, TestAppointment uTestAppointment)
		{
			if (id < 1)
				return BadRequest($"Not Accept This ID {id}.");

			if (uTestAppointment.TestTypeId < 1 ||
				uTestAppointment.LocalDrivingLicenseApplicationId < 1 ||
				uTestAppointment.AppointmentDate > DateTime.Today ||
				uTestAppointment.PaidFees < 0 ||
				uTestAppointment.CreatedByUserId < 1)
			{
				return BadRequest("Test Appointment Info Is Not Correct.");
			}


			var testAppointment = await _context.TestAppointments.FirstOrDefaultAsync(t => t.TestAppointmentId == id);
			if (testAppointment == null)
				return NotFound($"Not Found Test Appointment With ID {id}.");

			// Update the Driver fields
			uTestAppointment.TestAppointmentId = id;
			_context.Entry(testAppointment).CurrentValues.SetValues(uTestAppointment);

			if (await _context.SaveChangesAsync() > 0)
			{
				return Ok(testAppointment);
			}
			else
				return StatusCode(StatusCodes.Status500InternalServerError, "Error Updating Test Appointment.");
		}

		// Get By ID.
		[HttpGet("GetTestAppointmentByID/{id}", Name = "GetTestAppointmentByID")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<TestAppointment>> GetTestAppointmentByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept Test Appointment With ID {id}.");


			var test = await _context.TestAppointments.AsNoTracking()
									.FirstOrDefaultAsync(t => t.TestAppointmentId == id);

			if (test == null)
			{
				return NotFound($"Not Found Test Appointment With ID {id}");
			}

			return Ok(test);
		}

		// Is Exists By ID.
		[HttpGet("IsTestAppointmentExistsByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> IsTestAppointmentExistsByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept ID {id}.");


			if (await _context.TestAppointments.AnyAsync(t => t.TestAppointmentId == id))
			{
				return Ok($"Test Appointment With ID {id} Is Exists.");
			}
			else
			{
				return NotFound($"Not Found Test Appointment With ID {id}.");
			}
		}

		// Delete By ID.
		[HttpDelete("DeleteTestAppointmentByID/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<ActionResult<bool>> DeleteTestAppointmentByIDAsync(int id)
		{
			if (id < 1)
				return BadRequest($"Not Accept ID {id}.");

			var test = _context.TestAppointments.FirstOrDefault(t => t.TestAppointmentId == id);

			if (test != null)
			{
				_context.TestAppointments.Remove(test);

				if (await _context.SaveChangesAsync() > 0)
				{
					return Ok($"Deleted Test Appointment With ID {id} Successfully.");
				}
				else
					return StatusCode(StatusCodes.Status500InternalServerError, "Error Delete Test Appointment.");
			}
			else
			{
				return NotFound($"Not Found Test Appointment With ID {id}.");
			}
		}

		[HttpGet("GetAllTestAppointments")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public ActionResult<IEnumerable<TestAppointment>> GetAllTestAppointments()
		{
			var tests = _context.TestAppointments.AsNoTracking();

			if (!tests.Any())
				return NotFound($"Not Found Test Appointments");

			return Ok(tests);
		}
	}
}
