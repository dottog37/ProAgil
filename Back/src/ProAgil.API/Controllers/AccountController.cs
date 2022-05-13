using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;
using System.Security.Claims;
using ProAgil.API.Extensions;

namespace ProAgil.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ITokenService tokenService;

        public AccountController(IAccountService accountService, ITokenService tokenService)
        {
            this.accountService = accountService;
            this.tokenService = tokenService;
        }

        [HttpGet("GetUser/{userName}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUser(string userName){
            try
            {
                var user = await accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar usuario. Erro{ex.Message}");
            }
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto){
            try
            {
                if (await accountService.UserExists(userDto.Username))
                    return BadRequest("Usuário já existe");
                
                var user = await accountService.CreateAccountAsync(userDto);
                if (user != null)
                    return Ok(new {
                    userName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    token = tokenService.CreateToken(user).Result
                });

                return BadRequest("Usuário não criado, tente novamente mais tarde");
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar registrar usuario. Erro{ex.Message}");
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin){
            try
            {
                var user = await accountService.GetUserByUserNameAsync(userLogin.Username);
                if (user == null)
                    return Unauthorized("Usuário ou senha inválido!");
                
                var result = await accountService.CheckUserPassworsAsync(user, userLogin.Password);
                
                if (!result.Succeeded)
                    return Unauthorized("Usuário ou senha não estão corretos!");

                return Ok(new {
                    userName = user.UserName,
                    PrimeiroNome = user.PrimeiroNome,
                    token = tokenService.CreateToken(user).Result
                });
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar realizar login. Erro{ex.Message}");
            }
        }


        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser(){
            try
            {
                var userName = User.GetUserName();
                var user = await accountService.GetUserByUserNameAsync(userName);
                return Ok(user);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar recuperar usuario. Erro{ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserUpdateDto userUpdateDto){
            try
            {
                if (userUpdateDto.UserName != User.GetUserName()){
                    return Unauthorized("Usuário inválido");
                }
                var user = await accountService.GetUserByUserNameAsync(User.GetUserName());
                if (user == null)
                    return Unauthorized("Usuário inválido!");

                var userReturn = await accountService.UpdateAccount(userUpdateDto);
                if (userReturn == null)
                    return NoContent();
                
                return Ok(new {
                    userName = userReturn.UserName,
                    PrimeiroNome = userReturn.PrimeiroNome,
                    token = tokenService.CreateToken(userReturn).Result
                });
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                $"Erro ao tentar atualizar usuario. Erro{ex.Message}");
            }
        }
    }
}