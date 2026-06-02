using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using GerenciadorAlunosV2.Models;
using GerenciadorAlunosV2.Repositories;

namespace GerenciadorAlunosV2.Controllers;

public class MensalidadeController : Controller
{
    private readonly MensalidadeRepository _mensalidadeRepository;
    private readonly AlunoRepository _alunoRepository;

    public MensalidadeController(MensalidadeRepository mensalidadeRepository, AlunoRepository alunoRepository)
    {
        _mensalidadeRepository = mensalidadeRepository;
        _alunoRepository = alunoRepository;
    }

    // msm ideia: pra simplificar, incialmente, uma listagem geral
    public async Task <IActionResult> Index()
    {
        var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
        ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();
        return View(faturas);
    }

    [HttpGet]
    public async Task <IActionResult> LancarMensalidade()
    {
        ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();
        return View();
    }

    [HttpPost]
    public async Task <IActionResult> LancarMensalidade(MensalidadeModel novaMensalidade)
    {
        // aproveitei minha ideia de buscar os argumentexception setados no molde como fiz com a de alunos
        if (!ModelState.IsValid)
        {
            ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();
            var erro = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();

            if (erro != null && erro.Exception != null)
            {
                ViewBag.Erro = erro.Exception.Message;
            }
            else if (erro != null)
            {
                ViewBag.Erro = erro.ErrorMessage;
            }
            return View(novaMensalidade);
        }
        try
        {
            await _mensalidadeRepository.LancarMensalidadeAsync(novaMensalidade);
            return RedirectToAction("Index");
        }
        catch (Exception excecao)
        {
            ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();
            ViewBag.Erro = "Erro interno: " + excecao.Message;
            return View(novaMensalidade);
        }
    }
}