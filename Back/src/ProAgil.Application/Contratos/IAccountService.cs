using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProAgil.Application.Dtos;

namespace ProAgil.Application.Contratos
{
    public interface IAccountService
    {
         Task<bool> UserExists(string username);
         Task<UserUpdateDto> GetUserByUserNameAsync(string username);
         Task<SignInResult> CheckUserPassworsAsync(UserUpdateDto userUpdateDto, string password);
         Task<UserUpdateDto> CreateAccountAsync(UserDto userDto);
         Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto);
    }
}