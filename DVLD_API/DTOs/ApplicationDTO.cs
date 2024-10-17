namespace DVLD_API.DTOs
{
    public class ApplicationDTO
    {
		public int ApplicationId { get; set; }

		public int ApplicantPersonId { get; set; }

		public DateTime ApplicationDate { get; set; }

		public int ApplicationTypeId { get; set; }

		/// <summary>
		/// 1-New 2-Cancelled 3-Completed
		/// </summary>
		public byte ApplicationStatus { get; set; }

		public DateTime LastStatusDate { get; set; }

		public decimal PaidFees { get; set; }

		public string PersonFullName { get; set; } = null!;

		public string ApplicationType { get; set; } = null!;

		public string UserFullName { get; set; } = null!;
	}
}
