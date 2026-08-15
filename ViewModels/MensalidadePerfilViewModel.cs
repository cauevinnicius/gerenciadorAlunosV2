using System.ComponentModel.DataAnnotations;
using GerenciadorAlunosV2.Enums;

namespace GerenciadorAlunosV2.ViewModels;

public class MensalidadePerfilViewModel
{
    public int IdMensalidade { get; set; }
    
    [Required(ErrorMessage = "O aluno é obrigatório")]
    [Display(Name = "Aluno")]
    public int IdAluno { get; set; }
    
    public string? NomeAluno { get; set; }
    public string? CpfAluno { get; set; }

    [Required(ErrorMessage = "O valor é obrigatório")]
    [Display(Name = "Valor da Mensalidade")]
    public decimal ValorMensalidade { get; set; }

    [Required(ErrorMessage = "A data de vencimento é obrigatória")]
    [Display(Name = "Data de Vencimento")]
    [DataType(DataType.Date)] 
    public DateTime? DataVencimento { get; set; }

    [Required(ErrorMessage = "O status é obrigatório")]
    public StatusMensalidade Status { get; set; }

    [Display(Name = "Data de Pagamento")]
    [DataType(DataType.Date)]
    public DateTime? DataPagamento { get; set; }
}