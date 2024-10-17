using System.Text.Json.Serialization;

namespace DVLD_API.Entities;
public partial class LocalDrivingLicenseApplication
{
    public int LocalDrivingLicenseApplicationId { get; set; }

    public int ApplicationId { get; set; }

    public int LicenseClassId { get; set; }
	[JsonIgnore]
	public virtual Application? Application { get; set; } = null!;
	[JsonIgnore]
	public virtual LicenseClass? LicenseClass { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<TestAppointment> TestAppointments { get; set; } = new List<TestAppointment>();
}
