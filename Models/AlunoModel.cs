// meu molde de aluno no DB
using System.ComponentModel.DataAnnotations;
namespace GerenciadorAlunosV2.Models;
public class AlunoModel
{
    // Realizada a refatoração. Farei o usufruto das ViewModels para não expor minhas
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Nome { get; set; } = string.Empty;
    [Required]
    [MaxLength(14)]
    public string? Cpf { get; set; } = string.Empty;
    [Required]
    public string? Email { get; set; } = string.Empty;
    [Required]
    [MaxLength(15)]
    public string? Celular { get; set; } = string.Empty;
    [Required]
    public DateTime DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    [MaxLength(100)]
    public string Rua { get; set; } = string.Empty;
    [MaxLength(100)]
    public string Bairro { get; set; } = string.Empty;
    [MaxLength(50)]
    public string Cidade { get; set; } = string.Empty;
    [MaxLength(2)]
    public char Estado { get; set; } = ' ';
    [MaxLength(8)]
    public string Cep { get; set; } = string.Empty;

}