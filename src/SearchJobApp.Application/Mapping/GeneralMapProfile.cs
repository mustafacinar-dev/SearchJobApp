using AutoMapper;
using SearchJobApp.Application.Commands;
using SearchJobApp.Application.DTO;
using SearchJobApp.Domain.Entities;

namespace SearchJobApp.Application.Mapping;

public class GeneralMapProfile : Profile
{
    public GeneralMapProfile()
    {
        CreateMap<Employer, EmployerDto>().ReverseMap();
        CreateMap<Employer, EmployerWithPostsDto>().ReverseMap();
        CreateMap<Employer, CreateEmployerCommand>().ReverseMap();
        
        CreateMap<Post, PostDto>().ReverseMap();
        CreateMap<Post, CreatePostCommand>().ReverseMap();
    }
}