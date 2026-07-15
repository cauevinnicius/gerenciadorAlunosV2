using GerenciadorAlunosV2.Enums;  
namespace GerenciadorAlunosV2.ViewModels;

public class MensalidadePerfilViewModel
{
    public int IdMensalidade { get; set; }
    public int IdAluno { get; set; }
    public string? NomeAluno { get; set; }
    public string? CpfAluno { get; set; }
    public decimal ValorMensalidade { get; set; }
    public DateTime? DataVencimento { get; set; }
    // tive essa ideia depois de ver um tutorial no youtube onde se criava uma pasta separada até mesmo para enums. Depois eu posso reaproveitar mais facilmente.
    public StatusMensalidade Status { get; set; }
    public DateTime? DataPagamento { get; set; }
    
}