using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProAgil.Application.Contratos;
using ProAgil.Application.Dtos;
using ProAgil.Domain.Identity;
using ProAgil.Persistence.Contratos;

namespace ProAgil.Application
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUserPersist _userPersist;

        public AccountService(UserManager<User> userManager,
                                SignInManager<User> signInManager,
                                IMapper mapper, IUserPersist userPersist)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _userPersist = userPersist;
        }
        public async Task<SignInResult> CheckUserPassworsAsync(UserUpdateDto userUpdateDto, string password)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(u => u.UserName == userUpdateDto.UserName.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, true);
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar verificar password. Erro {ex.Message}");
            }
        }

        public async Task<UserDto> CreateAccountAsync(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded){
                    var userToReturn = _mapper.Map<UserDto>(user);
                    return userToReturn;
                }
                return null;
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar criar conta. Erro {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> GetUserByUserNameAsync(string userName)
        {
            try
            {
                var user = await _userPersist.GetUserByUsernameAsync(userName);
                if (user == null) return null;
                var userUpdateDto = _mapper.Map<UserUpdateDto>(user);
                return userUpdateDto;
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar criar usuario. Erro {ex.Message}");
            }
        }

        public async Task<UserUpdateDto> UpdateAccount(UserUpdateDto userUpdateDto)
        {
            try
            {
                var user = await _userPersist.GetUserByUsernameAsync(userUpdateDto.UserName);
                if (user == null) return null;

                _mapper.Map(userUpdateDto, user);
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateDto.Password);
                _userPersist.Update<User>(user);

                if (await _userPersist.SaveChangesAsync()){
                    var userRetorno = await _userPersist.GetUserByUsernameAsync(user.UserName);
                    return _mapper.Map<UserUpdateDto>(userRetorno);
                }
                return null;
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao tentar atualizar usuario. Erro {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string username)
        {
            try
            {
                return await _userManager.Users.AnyAsync(u => u.UserName == username.ToLower());
            }
            catch (System.Exception ex)
            {
                
                throw new System.Exception($"Erro ao verificar se usuário existe. Erro {ex.Message}");
            }
        }
    }
}