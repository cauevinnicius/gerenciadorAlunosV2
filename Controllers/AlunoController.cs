// aqui seria tipo como se fosse um "garçom", mas que busca e entrega dados.
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
// Ajuste os usings abaixo para o namespace exato das suas pastas copiadas
using GerenciadorAlunosV2.Models; 
using GerenciadorAlunosV2.Repositories; 

namespace GerenciadorAlunosV2.Controllers;

public class AlunoController : Controller
{
    private readonly AlunoRepository _alunoRepository;

    // A Injeção de Dependência brilhando! O ASP.NET entrega o repositório pronto aqui.
    public AlunoController(AlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    // essa seria tipo a ação principal da minha página inicial
    public async Task<IActionResult> Index()
    {
        // crio uma variavel pra pedir os dados pro meu db
        var alunosDoBanco = await _alunoRepository.ListarAsync();
        // depois eu "entrego" meus alunosDoBanco pra View
        return View(alunosDoBanco);
    }
}