using System.Text.Json.Serialization;

namespace DVLD_API.Entities;
public partial class Country
{
    public int CountryId { get; set; }

    public string CountryName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Person> People { get; set; } = new List<Person>();
}
