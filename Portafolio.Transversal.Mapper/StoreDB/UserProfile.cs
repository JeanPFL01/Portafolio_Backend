using AutoMapper;
using Portafolio.Application.DTO.Response;
using Portafolio.Domain.Entities.StoreDB;

namespace Portafolio.Transversal.Mapper.StoreDB
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<GetUserResponseDto, User>()
            .ReverseMap();
        }
    }
}
