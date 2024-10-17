using System.Text.Json.Serialization;

namespace DVLD_API.Entities;

public partial class Driver
{
    public int DriverId { get; set; }

    public int PersonId { get; set; }

    public int CreatedByUserId { get; set; }

    public DateTime CreatedDate { get; set; }
    [JsonIgnore]
    public virtual User? CreatedByUser { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();
	[JsonIgnore]
	public virtual ICollection<License> Licenses { get; set; } = new List<License>();
	[JsonIgnore]
	public virtual Person? Person { get; set; } = null!;
}
