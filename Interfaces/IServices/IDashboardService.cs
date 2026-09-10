namespace GerenciadorAlunosV2.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardViewModel> ObterDashboardAsync();
    }
}