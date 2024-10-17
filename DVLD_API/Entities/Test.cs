using System.Text.Json.Serialization;

namespace DVLD_API.Entities;
public partial class Test
{
    public int TestId { get; set; }

    public int TestAppointmentId { get; set; }

    /// <summary>
    /// 0 - Fail 1-Pass
    /// </summary>
    public bool TestResult { get; set; }

    public string? Notes { get; set; }

    public int CreatedByUserId { get; set; }
	[JsonIgnore]
	public virtual User? CreatedByUser { get; set; } = null!;
    [JsonIgnore]
    public virtual TestAppointment? TestAppointment { get; set; } = null!;
}
