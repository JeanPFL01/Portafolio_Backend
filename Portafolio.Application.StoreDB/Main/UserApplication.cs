using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Portafolio.Application.DTO;
using Portafolio.Application.DTO.Response;
using Portafolio.Application.StoreDB.Interface;
using Portafolio.Domain.Entities;
using Portafolio.Domain.Entities.StoreDB;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Portafolio.Application.StoreDB.Main
{
    public class UserApplication : IUserApplication
    {
        public readonly IConfiguration _configuration;
        public IMapper _mapper;
        public UserApplication(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<Response<GetUserResponseDto>> GetUserById(int idUser)
        {
            var response = new Response<GetUserResponseDto>();
            try
            {
                using var context = new StoreDbContext();
                var user = await context.Users.Where(x => x.UserId == idUser).FirstOrDefaultAsync();
                if (user != null)
                {
                    response.Data = _mapper.Map<GetUserResponseDto>(user);
                    response.Message = "Usuario encontrado";
                }
                else
                {
                    response.Message = "Usuario no encontrado";
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Error: " + ex.Message;
            }
            return response;
        }

        public async Task<Response<List<GetUserResponseDto>>> GetUserList()
        {
            var response = new Response<List<GetUserResponseDto>>();
            try
            {
                using var context = new StoreDbContext();
                var users = await context.Users.ToListAsync();
                response.Data = _mapper.Map<List<GetUserResponseDto>>(users);
                response.Message = "Usuarios Listados.";
            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<Response<GetLoginResponseDto>> Login(string username, string password)
        {
            var response = new Response<GetLoginResponseDto>();
            try
            {
                using var context = new StoreDbContext();
                var user = await context.Users
                    .Where(x => x.Username == username)
                    .FirstOrDefaultAsync();

                if (user != null && user.Password == password) // Verifica la contraseña
                {
                    response.Message = "Usuario encontrado.";

                    var jwt = new JwtClass
                    {
                        Audience = _configuration["Jwt:Audience"],
                        Issuer = _configuration["Jwt:Issuer"],
                        Key = _configuration["Jwt:Key"],
                        Subject = _configuration["Jwt:Subject"],
                    };
                    DateTime utcNow = DateTime.UtcNow;
                    DateTimeOffset unixEpoch = new DateTimeOffset(new DateTime(1970, 1, 1), TimeSpan.Zero).LocalDateTime;
                    DateTimeOffset nowOffset = new DateTimeOffset(utcNow, TimeSpan.Zero).LocalDateTime;
                    long timestamp = (long)(nowOffset - unixEpoch).TotalSeconds + 3600;


                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim("IdUser", user.UserId.ToString()),
                        new Claim("ExpireTime", timestamp.ToString())
                    };

                    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwt.Key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken
                        (
                            jwt.Issuer,
                            jwt.Audience,
                            claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: signIn
                        );

                    response.Data = new GetLoginResponseDto
                    {
                        Success = true,
                        Token = new JwtSecurityTokenHandler().WriteToken(token)
                    };
                }
                else
                {
                    response.Data = new GetLoginResponseDto
                    {
                        Success = false,
                        Token = ""
                    };
                    response.Message = "Usuario no encontrado o contraseña incorrecta";
                }
            }
            catch (Exception ex)
            {
                // Maneja los errores de manera segura
                response.IsSuccess = false;
                response.Message = "Error interno del servidor. Por favor, inténtelo de nuevo más tarde.";
            }
            return response;
        }

        public async Task<TokenValidation> ValidateToken(string token)
        {
            var response = new TokenValidation();
            using var context = new StoreDbContext();
            token = token.Replace("Bearer ", "");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = false, 
                    ClockSkew = TimeSpan.Zero
                };

                // Valida y decodifica el token
                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                if(principal != null)
                {
                    var expireDate = long.Parse(principal.Identities.FirstOrDefault().Claims.Where(x => x.Type == "ExpireTime").FirstOrDefault().Value);
                    DateTime dateTime = DateTimeOffset.UnixEpoch.AddSeconds(expireDate).LocalDateTime;
                    if(dateTime < DateTime.Now.ToLocalTime())
                    {
                        response.IsValidate = false;
                        response.Message = "Token Expirado";
                    }

                    var idUser = principal.Identities.FirstOrDefault().Claims.Where(x => x.Type == "IdUser").FirstOrDefault().Value;
                    var user = await context.Users.Where(x => x.UserId == int.Parse(idUser)).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        response.IsValidate = true;
                        response.Message = "Token Valido";
                    }
                }
                else
                {
                    response.ExpiredTime = true;
                    response.Message = "Token no valido";
                }

            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Token inválido", ex);
            }
            return response;
        }


    }
}
