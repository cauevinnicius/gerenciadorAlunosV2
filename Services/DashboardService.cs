using GerenciadorAlunosV2.ViewModels;
using GerenciadorAlunosV2.Repositories;
using System.Linq;

// a camada de servicos faria o trabalho "pesado" de lógica e afins e entregaria para o meu Controller
// isso seria o seguimento do principio SOLID - princípio da responsabilidade única
namespace GerenciadorAlunosV2.Services;

public class DashboardService
{
    private readonly AlunoRepository _alunoRepository;
    private readonly MensalidadeRepository _mensalidadeRepository;

    public DashboardService(AlunoRepository alunoRepository, MensalidadeRepository mensalidadeRepository)
    {
        _alunoRepository = alunoRepository;
        _mensalidadeRepository = mensalidadeRepository;
    }

    public async Task<DashboardViewModel> ObterDashboardAsync()
    {
        var alunos = await _alunoRepository.ListarAsync();
        var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();

        var dashboardModel = new DashboardViewModel
        {
            TotalAlunos = alunos.Count,
            TotalPendentes = faturas.Count(f => f.Status == "pendente"),
            FaturamentoTotal = faturas.Where(f => f.Status == "pago").Sum(f => f.ValorMensalidade)
        };

        return dashboardModel;
    }
}

