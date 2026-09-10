using GerenciadorAlunosV2.Models;

namespace GerenciadorAlunosV2.Interfaces;

public interface IMensalidadeRepository
{
    Task LancarMensalidadeAsync (MensalidadeModel mensalidade);
    Task <List<MensalidadeModel>> ListarMensalidadesAsync();
    Task RegistrarPagamentoAsync(int idMensalidade, DateTime dataPagamento);
    Task <List<MensalidadeModel>>VerificaPendenciasAsync(int alunoId);
    Task EditarMensalidadeAsync (MensalidadeModel mensalidadeEditada);
    Task ExcluirMensalidadeAsync(int idMensalidade);
}