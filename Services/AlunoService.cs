using GerenciadorAlunosV2.Interfaces;
using GerenciadorAlunosV2.Models;

namespace GerenciadorAlunosV2.Services;

public class AlunoService : IAlunoService
{
    private readonly IAlunoRepository _alunoRepository;

    public AlunoService(IAlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    public async Task<List<AlunoModel>> ListarAlunosAsync()
    {
        return await _alunoRepository.ListarAsync();
    }

    public async Task<List<AlunoModel>> SelecionarAsync(string parametroBusca)
    {
        return await _alunoRepository.SelecionarAsync(parametroBusca);
    }

    public async Task CadastrarAsync(AlunoModel aluno)
    {
        await _alunoRepository.CadastrarAsync(aluno);
    }

    public async Task AlterarAsync(AlunoModel aluno)
    {
        await _alunoRepository.AlterarAsync(aluno);
    }

    public async Task DeletarAsync(int id)
    {
        await _alunoRepository.DeletarAsync(id);
    }
}
