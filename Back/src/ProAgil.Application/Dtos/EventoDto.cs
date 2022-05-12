using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }    
        public string Local { get; set; }
        public string DataEvento { get; set; }
        
        [Required(ErrorMessage ="O campo {0} é obrigatório"),
         MinLength(3, ErrorMessage ="{0} precisa ter no mínimo 4 caracteres."),
         MaxLength(50, ErrorMessage ="{0} pode ter no máximo 50 caracteres.")]
        public string Tema { get; set; }

        [Range(1,120000, ErrorMessage = "{0} precisa estar no intervalo de 1 a 120000.")]
        [Display(Name ="Qtd pessoas")]
        public int QtdPessoas { get; set; }

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage ="Não é uma imagem válida (gif, jpg, bmp ou png).")]
        public string ImagemURL { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Phone(ErrorMessage ="O campo {0} não é inválido")]
        public string Telefone { get; set; }   

        [Required(ErrorMessage ="O campo {0} é obrigatório"),
         Display(Name = "e-mail"),
         EmailAddress(ErrorMessage = "O campo precisa ser um {0} válido")] 
        public string Email { get; set; }
        public int UserId { get; set; }
        public UserDto UserDto { get; set; }
        public IEnumerable<LoteDto> Lote { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}