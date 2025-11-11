using AutoMapper;
using BookstoreApplication.DTOs;
using BookstoreApplication.DTOs.UserDto;
using BookstoreApplication.Models.Entities;

namespace BookstoreApplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(d => d.AuthorFullName, m => m.MapFrom(s => s.Author != null ? s.Author.FullName : ""))
                .ForMember(d => d.PublisherName, m => m.MapFrom(s => s.Publisher != null ? s.Publisher.Name : ""))
                .ForMember(d => d.YearsAgo, m => m.MapFrom(s => YearsSince(s.PublishedDate)));

            CreateMap<Book, BookDetailsDto>()
                .ForMember(d => d.AuthorFullName, m => m.MapFrom(s => s.Author != null ? s.Author.FullName : ""))
                .ForMember(d => d.PublisherName, m => m.MapFrom(s => s.Publisher != null ? s.Publisher.Name : ""));

            CreateMap<ApplicationUser, ProfileDto>()
                .ForMember(d => d.Username, opt => opt.MapFrom(s => s.UserName));
        }

        private static int YearsSince(DateTime date)
        {
            var now = DateTime.UtcNow.Date;
            var years = now.Year - date.Year;
            if (now < date.AddYears(years)) years--;
            return Math.Max(0, years);
        }
    }
}
