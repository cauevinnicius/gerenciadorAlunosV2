using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GerenciadorAlunosV2.Models;
using GerenciadorAlunosV2.Repositories;
using GerenciadorAlunosV2.Services;

namespace GerenciadorAlunosV2.Controllers;

public class HomeController : Controller
{
    /* A partir do momento que inseri meu Services, eu não precisaria mais das minhas aluno e mensalidade repositorio
    private readonly AlunoRepository _alunoRepository;
    private readonly MensalidadeRepository _mensalidadeRepository;
    Mas sim a da própria dashboard service, que já faz a junção dos dois repositórios e me retorna o viewmodel pronto para a view.
    */
    private readonly DashboardService _dashboardService;
    private readonly SignInManager<UsuarioModel> _signInManager;
    // faço a injeção de dependência do repositório de alunos e do repositório de mensalidades no construtor do controlador   
    public HomeController(SignInManager<UsuarioModel> signInManager, DashboardService dashboardService)
    {
        _signInManager = signInManager;
        _dashboardService = dashboardService;
    }


    public async Task<IActionResult> Index()
    {
        // a ideia aqui é, se o usuário não estiver logado, apareça a index de visitantes 
        if (!_signInManager.IsSignedIn(User))
        {
            return View();
        }
        
        // se o usuário estiver logado, eu chamo o serviço que vai me retornar o viewmodel do dashboard
        // ou seja, nao preciso mais chamar todos os meus outros repositorios. 
        var dashboardModel = await _dashboardService.ObterDashboardAsync();
        return View(dashboardModel);        
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
