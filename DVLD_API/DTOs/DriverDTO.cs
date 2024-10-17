namespace DVLD_API.DTOs
{
	public class DriverDTO
	{
		public int DriverId { get; set; }
		public int PersonId { get; set; }
		public string NationalNo { get; set; } = null!;
		public string FullName { get; set; } = null!;
		public string DateOfBirth { get; set; } = null!;
		public string Gender { get; set; } = null!;
		public string Phone { get; set; } = null!;
		public string Email { get; set; } = null!;
		public string? Country { get; set; }
		public string CreatedByUser { get; set; } = null!;
		public string CreatedDate { get; set; } = null!;
	}
}
