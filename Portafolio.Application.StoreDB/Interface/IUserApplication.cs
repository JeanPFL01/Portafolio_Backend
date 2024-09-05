using Portafolio.Application.DTO;
using Portafolio.Application.DTO.Response;
using Portafolio.Domain.Entities;

namespace Portafolio.Application.StoreDB.Interface
{
    public interface IUserApplication
    {
        Task<Response<GetLoginResponseDto>> Login(string username, string password);
        Task<Response<GetUserResponseDto>> GetUserById(int idUser);
        Task<Response<List<GetUserResponseDto>>> GetUserList();
        Task<TokenValidation> ValidateToken(string token);
    }
}
