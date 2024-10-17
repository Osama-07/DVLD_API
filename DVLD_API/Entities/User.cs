using System.Text.Json.Serialization;

namespace DVLD_API.Entities;
public partial class User
{
    public int UserId { get; set; }

    public int PersonId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }
	[JsonIgnore]
	public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
	[JsonIgnore]
	public virtual ICollection<DetainedLicense> DetainedLicenseCreatedByUsers { get; set; } = new List<DetainedLicense>();
	[JsonIgnore]
	public virtual ICollection<DetainedLicense> DetainedLicenseReleasedByUsers { get; set; } = new List<DetainedLicense>();
	[JsonIgnore]
	public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
	[JsonIgnore]
	public virtual ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();
	[JsonIgnore]
	public virtual ICollection<License> Licenses { get; set; } = new List<License>();
	[JsonIgnore]
	public virtual Person? Person { get; set; } = null!;
	[JsonIgnore]
	public virtual ICollection<TestAppointment> TestAppointments { get; set; } = new List<TestAppointment>();
    [JsonIgnore]
    public virtual ICollection<Test> Tests { get; set; } = new List<Test>();
}
