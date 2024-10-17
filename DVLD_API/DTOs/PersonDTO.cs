namespace DVLD_API.DTOs
{
	public class PersonDTO
	{
		public int PersonId { get; set; }

		public string NationalNo { get; set; } = null!;

		public string FirstName { get; set; } = null!;

		public string SecondName { get; set; } = null!;

		public string? ThirdName { get; set; }

		public string LastName { get; set; } = null!;

		public string FullName => $"{this.FirstName} {this.SecondName} {this.ThirdName} {this.LastName}";

		public DateTime DateOfBirth { get; set; }

		public string Gender { get; set; } = null!;

		public string? Address { get; set; }

		public string Phone { get; set; } = null!;

		public string Email { get; set; } = null!;

		public string? PersonalPicture { get; set; }

		public string? Country { get; set; }
	}
}
