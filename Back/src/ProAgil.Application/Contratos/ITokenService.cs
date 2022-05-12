using System.Threading.Tasks;
using ProAgil.Application.Dtos;

namespace ProAgil.Application.Contratos
{
    public interface ITokenService
    {
         Task<string> CreateToken(UserUpdateDto userUpdateDto);
    }
}