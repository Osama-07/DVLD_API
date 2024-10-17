using System.Text.Json.Serialization;

namespace DVLD_API.Entities;
public partial class DetainedLicense
{
    public int DetainId { get; set; }

    public int LicenseId { get; set; }

    public DateTime DetainDate { get; set; }

    public decimal FineFees { get; set; }

    public int CreatedByUserId { get; set; }

    public bool IsReleased { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int? ReleasedByUserId { get; set; }

    public int? ReleaseApplicationId { get; set; }
    [JsonIgnore]
    public virtual User? CreatedByUser { get; set; } = null!;
    [JsonIgnore]
    public virtual License? License { get; set; } = null!;
    [JsonIgnore]
    public virtual Application? ReleaseApplication { get; set; }
    [JsonIgnore]
    public virtual User? ReleasedByUser { get; set; }
}
