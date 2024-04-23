using AutoMapper;
using LibraryWebApi.Entities;
using LibraryWebApi.Dtos;

namespace LibraryWebApi.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Borrower, BorrowerDto>().ReverseMap();
            CreateMap<BorrowBooks, BorrowBooksDto>().ReverseMap();
        }
    }
}
