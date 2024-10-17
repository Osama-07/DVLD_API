using System.Text.Json.Serialization;

namespace DVLD_API.Entities;

public partial class Application
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

    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public virtual Person? ApplicantPerson { get; set; } = null!;
    [JsonIgnore]
    public virtual ApplicationType? ApplicationType { get; set; } = null!;
    [JsonIgnore]
    public virtual User? CreatedByUser { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<DetainedLicense> DetainedLicenses { get; set; } = new List<DetainedLicense>();
    [JsonIgnore]
    public virtual ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();
    [JsonIgnore]
    public virtual ICollection<License> Licenses { get; set; } = new List<License>();
    [JsonIgnore]
    public virtual ICollection<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; set; } = new List<LocalDrivingLicenseApplication>();
}
