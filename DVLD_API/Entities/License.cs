using System.Text.Json.Serialization;

namespace DVLD_API.Entities;

public partial class License
{
    public int LicenseId { get; set; }

    public int ApplicationId { get; set; }

    public int DriverId { get; set; }

    public int LicenseClass { get; set; }

    public DateTime IssueDate { get; set; }

    public DateTime ExpirationDate { get; set; }

    public string? Notes { get; set; }

    public decimal PaidFees { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// 1-FirstTime, 2-Renew, 3-Replacement for Damaged, 4- Replacement for Lost.
    /// </summary>
    public byte IssueReason { get; set; }

    public int CreatedByUserId { get; set; }
    [JsonIgnore]
    public virtual Application Application { get; set; } = null!;
	[JsonIgnore]
	public virtual User CreatedByUser { get; set; } = null!;
	[JsonIgnore]
	public virtual ICollection<DetainedLicense> DetainedLicenses { get; set; } = new List<DetainedLicense>();
	[JsonIgnore]
	public virtual Driver Driver { get; set; } = null!;
	[JsonIgnore]
	public virtual ICollection<InternationalLicense> InternationalLicenses { get; set; } = new List<InternationalLicense>();
	[JsonIgnore]
	public virtual LicenseClass LicenseClassNavigation { get; set; } = null!;
}
