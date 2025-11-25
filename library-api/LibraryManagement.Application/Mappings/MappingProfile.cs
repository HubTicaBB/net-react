using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Book mappings
        CreateMap<Book, BookDto>();
        CreateMap<CreateBookDto, Book>();
        CreateMap<UpdateBookDto, Book>();

        // Borrowing mappings
        CreateMap<Borrowing, BorrowingDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
        CreateMap<CreateBorrowingDto, Borrowing>();
        CreateMap<UpdateBorrowingDto, Borrowing>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (Domain.Entities.BorrowingStatus)src.Status));
    }
}

