using System.Text.Json.Serialization;

namespace DVLD_API.Entities;

public partial class Person
{
    public int PersonId { get; set; }

    public string NationalNo { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string SecondName { get; set; } = null!;

    public string? ThirdName { get; set; }

    public string LastName { get; set; } = null!;

    public string FullName => $"{this.FirstName} {this.SecondName} {this.ThirdName} {this.LastName}";

    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// 0 Male , 1 Femail
    /// </summary>
    public byte Gender { get; set; }

    public string? Address { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? PersonalPicture { get; set; }

    public int CountryId { get; set; }
    public virtual Country Country { get; set; }

    [JsonIgnore]
    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();
    [JsonIgnore]
    public virtual Driver? Driver { get; set; } = new Driver();
    [JsonIgnore]
    public virtual User? User { get; set; }
}
