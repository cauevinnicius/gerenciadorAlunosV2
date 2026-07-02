using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GerenciadorAlunosV2.Models;
using GerenciadorAlunosV2.Repositories;

namespace GerenciadorAlunosV2.Controllers;

public class HomeController : Controller
{
    private readonly AlunoRepository _alunoRepository;
    private readonly MensalidadeRepository _mensalidadeRepository;
    private readonly SignInManager<UsuarioModel> _signInManager;
    // faço a injeção de dependência do repositório de alunos e do repositório de mensalidades no construtor do controlador   
    public HomeController(AlunoRepository alunoRepository, MensalidadeRepository mensalidadeRepository, SignInManager<UsuarioModel> signInManager)
    {
        _alunoRepository = alunoRepository;
        _mensalidadeRepository = mensalidadeRepository;
        _signInManager = signInManager;
    }


    public async Task<IActionResult> Index()
    {
        // a ideia aqui é, se o usuário estiver logado, aí sim apareça a index
        if (_signInManager.IsSignedIn(User))
        {
            var alunos = await _alunoRepository.ListarAsync();
            var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();

            var dashboard = new DashboardViewModel
            {
                TotalAlunos = alunos.Count,
                TotalPendentes = faturas.Count(f => f.Status == "pendente"),
                FaturamentoTotal = faturas.Where(f => f.Status == "pago").Sum(f => f.ValorMensalidade)
            };
            
            return View(dashboard);
        }

        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
