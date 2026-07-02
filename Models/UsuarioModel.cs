using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace GerenciadorAlunosV2.Models;

public class UsuarioModel : IdentityUser
{
    [Required(ErrorMessage = "O campo {0} é obrigatório.")]
    public string? NomeCompleto { get; set; } = string.Empty;
}