using AutoMapper;
using DVLD_API.DTOs;
using DVLD_API.Entities;

namespace DVLD_API.Mapper
{
	public class MappingProfile : Profile
	{
		public MappingProfile() 
		{
			// People.
			CreateMap<Person, PersonDTO>()
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == 0 ? "Male" : "Female"))
				.ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country.CountryName))
				.ReverseMap();
			// Applications.
			CreateMap<Application, ApplicationDTO>()
				.ForMember(dest => dest.PersonFullName, opt => opt.MapFrom(src => src.ApplicantPerson != null ? src.ApplicantPerson.FullName : "Unknown Person"))
				.ForMember(dest => dest.ApplicationType, opt => opt.MapFrom(src => src.ApplicationType != null ? src.ApplicationType.ApplicationTypeTitle : "Unknown App"))
				.ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.Person.FullName : "Unknown User"))
				.ReverseMap();
			// Drivers.
			CreateMap<Driver, DriverDTO>()
				.ForMember(dest => dest.NationalNo, opt => opt.MapFrom(src => src.Person != null ? src.Person.NationalNo : "Unknown"))
				.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Person != null ? src.Person.FullName : "Unknown"))
				.ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.Person != null ? src.Person.DateOfBirth.ToShortDateString() : "Unknown"))
				.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Person != null ? src.Person.Gender == 0 ? "Male" : "Female" : "Unknown"))
				.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Person != null ? src.Person.Phone : "Unknown"))
				.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Person != null ? src.Person.Email : "Unknown"))
				.ForMember(
					dest => dest.Country,
					opt => opt.MapFrom(src => src.Person != null ? src.Person.Country != null ? src.Person.Country.CountryName : "Unknown" : "Unknown")
				)
				.ForMember(dest => dest.CreatedByUser, opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.Username : "Unknown"))
				.ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToShortDateString()));
		}
	}
}
