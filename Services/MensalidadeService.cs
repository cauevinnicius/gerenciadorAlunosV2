using GerenciadorAlunosV2.Interfaces;
using GerenciadorAlunosV2.Models;

namespace GerenciadorAlunosV2.Services;

public class MensalidadeService : IMensalidadeService
{
    private readonly IMensalidadeRepository _mensalidadeRepository;

    public MensalidadeService(IMensalidadeRepository mensalidadeRepository)
    {
        _mensalidadeRepository = mensalidadeRepository;
    }

    public async Task LancarMensalidadeAsync(MensalidadeModel mensalidade)
    {
        await _mensalidadeRepository.LancarMensalidadeAsync(mensalidade);
    }

    public async Task<List<MensalidadeModel>> ListarMensalidadesAsync()
    {
        return await _mensalidadeRepository.ListarMensalidadesAsync();
    }

    public async Task RegistrarPagamentoAsync(int idMensalidade, DateTime dataPagamento)
    {
        await _mensalidadeRepository.RegistrarPagamentoAsync(idMensalidade, dataPagamento);
    }

    public async Task<List<MensalidadeModel>> VerificaPendenciasAsync(int alunoId)
    {
        return await _mensalidadeRepository.VerificaPendenciasAsync(alunoId);
    }

    public async Task EditarMensalidadeAsync(MensalidadeModel mensalidadeEditada)
    {
        await _mensalidadeRepository.EditarMensalidadeAsync(mensalidadeEditada);
    }

    public async Task ExcluirMensalidadeAsync(int idMensalidade)
    {
        await _mensalidadeRepository.ExcluirMensalidadeAsync(idMensalidade);
    }
}
