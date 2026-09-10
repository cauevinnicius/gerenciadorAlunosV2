using GerenciadorAlunosV2.Models;

namespace GerenciadorAlunosV2.Interfaces;

public interface IAlunoService
{
    Task<List<AlunoModel>> ListarAlunosAsync();
    Task<List<AlunoModel>> SelecionarAsync(string parametroBusca);
    Task CadastrarAsync(AlunoModel aluno);
    Task AlterarAsync(AlunoModel aluno);
    Task DeletarAsync(int id);
}