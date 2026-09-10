using GerenciadorAlunosV2.ViewModels;
using System.Linq;
using GerenciadorAlunosV2.Interfaces;

// a camada de servicos faria o trabalho "pesado" de lógica e afins e entregaria para o meu Controller
// isso seria o seguimento do principio SOLID - princípio da responsabilidade única
namespace GerenciadorAlunosV2.Services;

public class DashboardService : IDashboardService
{
    private readonly IAlunoRepository _alunoRepository;
    private readonly IMensalidadeRepository _mensalidadeRepository;

    public DashboardService(IAlunoRepository alunoRepository, IMensalidadeRepository mensalidadeRepository)
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
            TotalPendentes = faturas.Count(f => f.Status.ToString() == "Pendente"),
            FaturamentoTotal = faturas.Where(f => f.Status.ToString() == "Pago").Sum(f => f.ValorMensalidade)
        };

        return dashboardModel;
    }
}

