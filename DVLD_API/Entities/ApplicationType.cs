using System.Text.Json.Serialization;

namespace DVLD_API.Entities;
public partial class ApplicationType
{
    public int ApplicationTypeId { get; set; }

    public string ApplicationTypeTitle { get; set; } = null!;

    public decimal ApplicationFees { get; set; }
    [JsonIgnore]
    public ICollection<Application> Applications { get; set; } = new List<Application>();
}
