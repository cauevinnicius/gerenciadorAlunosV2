using GerenciadorAlunosV2.Models;
using System.ComponentModel.DataAnnotations;
namespace GerenciadorAlunosV2.ViewModels;

// duas boas práticas: refatorando, entendi que não fazia sentido incluir a data de cadastro e o id, por exemplo
// lembrando, isso é o que nós apresentaremos na página. Não faz sentido expor minhas models
public class AlunoPerfilViewModel
{
    [Display(Name = "Nome completo")]
    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [MaxLength(100)]
    public string? NomeAluno { get; set; }

    [Display(Name = "CPF do Aluno")]
    [Required(ErrorMessage = "O CPF do aluno é obrigatório.")]
    [MaxLength(14)]
    public string? CpfAluno { get; set; } 

    [Display(Name = "Email do Aluno")]
    [Required(ErrorMessage = "Insira um e-mail válido.")]
    [EmailAddress(ErrorMessage = "O email do aluno não é válido.")]
    public string? EmailAluno { get; set; }
    
    [Display(Name = "Celular")]
    [Required(ErrorMessage = "O celular é obrigatório.")]
    [MaxLength(15)]
    public string? CelularAluno { get; set; }

    [Display(Name = "Data de Nascimento")]
    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    public DateTime DataNascimentoAluno { get; set; }

    [Display(Name = "Rua/Avenida/Travessa/etc.")]
    [MaxLength(100)]
    public string RuaAluno { get; set; } = string.Empty;

    [Display(Name = "Bairro")]
    [MaxLength(100)]
    public string BairroAluno { get; set; } = string.Empty;

    [Display(Name = "Cidade")]
    [MaxLength(50)]
    public string CidadeAluno { get; set; } = string.Empty;

    [Display(Name = "Estado (UF)")]
    [MaxLength(2)]
    public char EstadoAluno { get; set; } = ' ';
    [Display(Name = "CEP")]
    [MaxLength(8)]
    public string CepAluno { get; set; } = string.Empty;
    public List<MensalidadeModel>? HistoricoMensalidades { get; set; }
}