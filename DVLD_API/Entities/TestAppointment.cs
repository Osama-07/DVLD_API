using System.Text.Json.Serialization;

namespace DVLD_API.Entities;

public partial class TestAppointment
{
    public int TestAppointmentId { get; set; }

    public int TestTypeId { get; set; }

    public int LocalDrivingLicenseApplicationId { get; set; }

    public DateTime AppointmentDate { get; set; }

    public decimal PaidFees { get; set; }

    public int CreatedByUserId { get; set; }

    public bool IsLocked { get; set; }

    public int? RetakeTestApplicationId { get; set; }
	[JsonIgnore]
	public virtual User? CreatedByUser { get; set; }
	[JsonIgnore]
	public virtual LocalDrivingLicenseApplication? LocalDrivingLicenseApplication { get; set; }
	[JsonIgnore]
	public virtual TestType? TestType { get; set; }
    [JsonIgnore]
    public virtual Test? Test { get; set; }
}
