using AutoMapper;
using ProAgil.Application.Dtos;
using ProAgil.Domain;
using ProAgil.Domain.Identity;

namespace ProAgil.Application.Helpers
{
    public class ProAgilProfile: Profile
    {
        public ProAgilProfile()
        {
            CreateMap<Evento, EventoDto>().ReverseMap();
            CreateMap<Lote, LoteDto>().ReverseMap();
            CreateMap<RedeSocial, RedeSocialDto>().ReverseMap();
            CreateMap<Palestrante, PalestranteDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserLoginDto>().ReverseMap();
        }
    }
}