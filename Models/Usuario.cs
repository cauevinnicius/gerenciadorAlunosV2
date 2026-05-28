using System.ComponentModel.DataAnnotations;
namespace GerenciadorAlunosV2.Models;

public class Usuario
{
    public int UserId { get; set; }  
    // quis colocar que isso fosse obrigatório para prevenir.
    public required string Username { get; set; }
    public required string HashPassword { get; set; }

}