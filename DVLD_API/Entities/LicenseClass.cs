using System.Text.Json.Serialization;

namespace DVLD_API.Entities;
public partial class LicenseClass
{
    public int LicenseClassId { get; set; }

    public string ClassName { get; set; } = null!;

    public string ClassDescription { get; set; } = null!;

    /// <summary>
    /// Minmum age allowed to apply for this license
    /// </summary>
    public byte MinimumAllowedAge { get; set; }

    /// <summary>
    /// How many years the licesnse will be valid.
    /// </summary>
    public byte DefaultValidityLength { get; set; }

    public decimal ClassFees { get; set; }
    [JsonIgnore]
    public virtual ICollection<License> Licenses { get; set; } = new List<License>();
    [JsonIgnore]
    public virtual ICollection<LocalDrivingLicenseApplication> LocalDrivingLicenseApplications { get; set; } = new List<LocalDrivingLicenseApplication>();
}
