using System.ComponentModel.DataAnnotations;

namespace GerenciadorAlunosV2.ViewModels
{
    public class VerificarEmailUsuarioViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório")]
        [EmailAddress]
        public string? Email { get; set; }
    }
}