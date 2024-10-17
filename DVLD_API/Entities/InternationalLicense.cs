using System.Text.Json.Serialization;

namespace DVLD_API.Entities;

public partial class InternationalLicense
{
    public int InternationalLicenseId { get; set; }

    public int ApplicationId { get; set; }

    public int DriverId { get; set; }

    public int IssuedUsingLocalLicenseId { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public bool IsActive { get; set; }

    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public virtual Application? Application { get; set; } = null!;
	[JsonIgnore]
	public virtual User? CreatedByUser { get; set; } = null!;
	[JsonIgnore]
	public virtual Driver? Driver { get; set; } = null!;
	[JsonIgnore]
	public virtual License? IssuedUsingLocalLicense { get; set; } = null!;
}
