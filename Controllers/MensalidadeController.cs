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

    // replicando o que escrevi lá na index da mensalidade: minha primeira ideia era uma nova página de confirmação de pagamento
    // dps achei mais interessante apenas clicar no botão e já mudar o status. Então eu só preciso de um post
    [HttpPost]
    public async Task <IActionResult> PagarMensalidade (int id)
    {
        try
        {
            // primeiro eu busco a fatura
            var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
            var faturaEncontrada = faturas.FirstOrDefault(m => m.Id == id);

            // dai sim eu altero e faço o salvamento
            if (faturaEncontrada != null && faturaEncontrada.Status == "pendente")
            {
                faturaEncontrada.Status = "pago";
                faturaEncontrada.DataPagamento = DateTime.Now;

                await _mensalidadeRepository.EditarMensalidadeAsync(faturaEncontrada);
            }

            return RedirectToAction("Index");
        }
        catch (Exception excecao)
        {
            // aqui eu dou um alerta temporário pro usuário que deu erro ao pagar 
            TempData["ErroPagamento"] = "Erro ao pagar a mensalidade: " + excecao.Message;
            return RedirectToAction("Index");
        }
    }

    [HttpGet]
    public async Task <IActionResult> EditarMensalidade (int id)
    {
        // primeiro busco a mensalidade
        var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
        var faturaEncontrada = faturas.FirstOrDefault(m => m.Id == id);

        if (faturaEncontrada == null)
        {
            return NotFound();
        }

        // tenho q mandar minha lista de alunos tb
        ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();

        return View(faturaEncontrada);
    }

    [HttpPost]
    public async Task<IActionResult> EditarMensalidade (MensalidadeModel faturaEditada)
    {
        // reaproveitei a validação que fiz na de cadastro.
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
            return View(faturaEditada);
        }
        
        try
        {
            await _mensalidadeRepository.EditarMensalidadeAsync(faturaEditada);
            return RedirectToAction("Index");
        }
        catch (Exception excecao)
        {
            ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();
            ViewBag.Erro = "Erro interno: " + excecao.Message;
            return View(faturaEditada);
        }
    }

    [HttpGet]
    public async Task <IActionResult> DeletarMensalidade (int id)
    {
        var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
        var faturaEncontrada = faturas.FirstOrDefault(m => m.Id == id);

        if (faturaEncontrada == null)
        {
            return NotFound();
        }

        ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();

        return View(faturaEncontrada);

    }

    [HttpPost]
    public async Task <IActionResult> ConfirmarDelecao (int id)
    {
        try
        {
            await _mensalidadeRepository.ExcluirMensalidadeAsync(id);
            return RedirectToAction("Index");
        }
        catch(Exception excecao)
        {
            ViewBag.Erro = "Hmm.. parece que não foi possível deletar essa mensalidade. Erro: " + excecao.Message;

            var todasFaturas = await _mensalidadeRepository.ListarMensalidadesAsync();
            var fatura = todasFaturas.FirstOrDefault(m => m.Id == id);
            return View("DeletarMensalidade", fatura);
        }

    }
}