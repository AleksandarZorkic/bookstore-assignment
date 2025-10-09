using AutoMapper;
using BookstoreApplication.DTOs;
using BookstoreApplication.Models;

namespace BookstoreApplication.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>()
                .ForMember(d => d.AuthorFullName, m => m.MapFrom(s => s.Author.FullName))
                .ForMember(d => d.PublisherName, m => m.MapFrom(s => s.Publisher.Name))
                .ForMember(d => d.YearsAgo, m => m.MapFrom(s => s.PublishedDate));

            CreateMap<Book, BookDetailsDto>()
                .ForMember(d => d.AuthorId, m => m.MapFrom(s => s.AuthorId))
                .ForMember(d => d.AuthorFullName, m => m.MapFrom(s => s.Author.FullName))
                .ForMember(d => d.PublisherId, m => m.MapFrom(s => s.PublisherId))
                .ForMember(d => d.PublisherName, m => m.MapFrom(s => s.Publisher.Name));
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
