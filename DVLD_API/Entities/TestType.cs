using System.Text.Json.Serialization;

namespace DVLD_API.Entities;

public partial class TestType
{
    public int TestTypeId { get; set; }

    public string TestTypeTitle { get; set; } = null!;

    public string TestTypeDescription { get; set; } = null!;

    public decimal TestTypeFees { get; set; }
    [JsonIgnore]
    public virtual ICollection<TestAppointment> TestAppointments { get; set; } = new List<TestAppointment>();
}
