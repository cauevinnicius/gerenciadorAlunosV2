using GerenciadorAlunosV2.Models;
using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;
namespace GerenciadorAlunosV2.ViewModels;

// duas boas práticas: refatorando, entendi que não fazia sentido incluir a data de cadastro e o id, por exemplo
// lembrando, isso é o que nós apresentaremos na página. Não faz sentido expor minhas models
public class AlunoPerfilViewModel
{
    public int IdAluno { get; set; }

    [Name("Nome")]
    [Display(Name = "Nome completo")]
    [Required(ErrorMessage = "O nome completo é obrigatório.")]
    [MaxLength(100)]
    public string? NomeAluno { get; set; }

    [Name("Cpf")]
    [Display(Name = "CPF do Aluno")]
    [Required(ErrorMessage = "O CPF do aluno é obrigatório.")]
    [MaxLength(14)]
    public string? CpfAluno { get; set; } 

    [Name("Email")]
    [Display(Name = "Email do Aluno")]
    [Required(ErrorMessage = "Insira um e-mail válido.")]
    [EmailAddress(ErrorMessage = "O email do aluno não é válido.")]
    public string? EmailAluno { get; set; }
    
    [Name("Celular")]
    [Display(Name = "Celular")]
    [Required(ErrorMessage = "O celular é obrigatório.")]
    [MaxLength(15)]
    public string? CelularAluno { get; set; }

    [Name("DataNascimento")]
    [Display(Name = "Data de Nascimento")]
    [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
    public DateTime DataNascimentoAluno { get; set; }

    [Name("Rua")]
    [Display(Name = "Rua/Avenida/Travessa/etc.")]
    [MaxLength(100)]
    public string RuaAluno { get; set; } = string.Empty;

    [Name("Bairro")]
    [Display(Name = "Bairro")]
    [MaxLength(100)]
    public string BairroAluno { get; set; } = string.Empty;

    [Name("Cidade")]
    [Display(Name = "Cidade")]
    [MaxLength(50)]
    public string CidadeAluno { get; set; } = string.Empty;

    [Name("Estado")]
    [Display(Name = "Estado (UF)")]
    [MaxLength(2)]
    public string EstadoAluno { get; set; } = string.Empty;
    
    [Name("Cep")]
    [Display(Name = "CEP")]
    [MaxLength(8)]
    public string CepAluno { get; set; } = string.Empty;
    public List<MensalidadeModel>? HistoricoMensalidades { get; set; }
}