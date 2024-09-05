using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portafolio.Application.StoreDB.Interface;
using Portafolio.Domain.Entities;
using Portafolio.Domain.Entities.StoreDB;
using System.Security.Claims;

namespace Portafolio.Service.StoreDB_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        public IUserApplication _userApplication;
        public UserController(IUserApplication userApplication) 
        {
            _userApplication = userApplication;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(string username, string password)
        {
            return Ok(await _userApplication.Login(username, password));
        }
        [HttpGet]
        [Authorize]
        [Route("GetById")]
        public async Task<ActionResult> GetById(int idUser)
        {
            var valTok = await _userApplication.ValidateToken(HttpContext.Request.Headers.Authorization);
            if (!valTok.IsValidate)
            {
                return Unauthorized(new { valTok });
            }
            else
            {
                var response = await _userApplication.GetUserById(idUser);
                response.IsValidate = valTok.IsValidate;
                response.MessageValidate = valTok.Message;
                return Ok(response);
            }
        }
        [HttpGet]
        [Authorize]
        [Route("GetList")]
        public async Task<ActionResult> GetList()
        {
            var valTok = await _userApplication.ValidateToken(HttpContext.Request.Headers.Authorization);
            if (!valTok.IsValidate)
            {
                return Unauthorized(new { valTok });
            }
            else
            {
                var response = await _userApplication.GetUserList();
                response.IsValidate = valTok.IsValidate;
                response.MessageValidate = valTok.Message;
                return Ok(response);
            }
        }
    }
}
