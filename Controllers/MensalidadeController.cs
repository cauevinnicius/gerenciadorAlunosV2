using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using GerenciadorAlunosV2.Models;
using GerenciadorAlunosV2.ViewModels;
using GerenciadorAlunosV2.Repositories;
using GerenciadorAlunosV2.Enums;

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
    public async Task<IActionResult> Index()
    {
        var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
        var alunos = await _alunoRepository.ListarAsync();

        // Mapeamos a Model para a ViewModel e já injetamos o nome do aluno aqui!
        var viewModel = faturas.Select(f => new MensalidadePerfilViewModel
        {
            IdMensalidade = f.Id,
            IdAluno = f.AlunoId,
            NomeAluno = alunos.FirstOrDefault(a => a.Id == f.AlunoId)?.Nome ?? "Desconhecido",
            ValorMensalidade = f.ValorMensalidade,
            DataVencimento = f.DataVencimento,
            Status = f.Status
        }).ToList();

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> LancarMensalidade()
    {
        ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LancarMensalidade(MensalidadeModel novaMensalidade)
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
    public async Task<IActionResult> PagarMensalidade(int id)
    {
        try
        {
            // primeiro eu busco a fatura
            var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
            var faturaEncontrada = faturas.FirstOrDefault(m => m.Id == id);

            // dai sim eu altero e faço o salvamento
            if (faturaEncontrada != null && faturaEncontrada.Status != StatusMensalidade.Pago)
            {
                faturaEncontrada.Status = StatusMensalidade.Pago;
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
    public async Task<IActionResult> EditarMensalidade(int id)
    {
        // primeiro busco a mensalidade
        var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
        var faturaEncontrada = faturas.FirstOrDefault(m => m.Id == id);

        if (faturaEncontrada == null)
        {
            return NotFound();
        }

        ViewBag.ListaAlunos = await _alunoRepository.ListarAsync();

        var viewModel = new MensalidadePerfilViewModel
        {
            IdMensalidade = faturaEncontrada.Id,
            IdAluno = faturaEncontrada.AlunoId,
            ValorMensalidade = faturaEncontrada.ValorMensalidade,
            DataVencimento = faturaEncontrada.DataVencimento,
            Status = faturaEncontrada.Status,
            DataPagamento = faturaEncontrada.DataPagamento
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> EditarMensalidade(MensalidadePerfilViewModel faturaEditada)
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
            var busca = await _mensalidadeRepository.ListarMensalidadesAsync();
            var faturaOriginal = busca.FirstOrDefault(m => m.Id == faturaEditada.IdMensalidade);

            if (faturaOriginal == null)
            {
                return NotFound();
            }

            faturaOriginal.ValorMensalidade = faturaEditada.ValorMensalidade;
            faturaOriginal.DataVencimento = faturaEditada.DataVencimento ?? faturaOriginal.DataVencimento;
            faturaOriginal.DataPagamento = faturaEditada.DataPagamento;
            faturaOriginal.Status = faturaEditada.Status;
            // antes estava como faturaEditada
            await _mensalidadeRepository.EditarMensalidadeAsync(faturaOriginal);
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
    public async Task<IActionResult> DeletarMensalidade(int id)
    {
        // primeiro eu busco uma mensalidade específica
        var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
        var faturaEncontrada = faturas.FirstOrDefault(m => m.Id == id);

        if (faturaEncontrada == null)
        {
            return NotFound();
        }
        //com a mensalidade "em maos", eu devo buscar o dono dela.
        var buscaAluno = await _alunoRepository.SelecionarAsync(faturaEncontrada.AlunoId.ToString());
        var alunoEncontrado = buscaAluno.FirstOrDefault();
        if (alunoEncontrado == null)
        {
            return NotFound();
        }

        //por fim, vou criar minha viewmodel
        var viewModel = new MensalidadePerfilViewModel
        {
            IdMensalidade = faturaEncontrada.Id,
            IdAluno = alunoEncontrado.Id,
            NomeAluno = alunoEncontrado.Nome,
            CpfAluno = alunoEncontrado.Cpf,
            ValorMensalidade = faturaEncontrada.ValorMensalidade,
            DataVencimento = faturaEncontrada.DataVencimento
        };

        return View(viewModel);

    }

    [HttpPost]
    public async Task<IActionResult> ConfirmarDelecao(int idMensalidade)
    {
        try
        {
            await _mensalidadeRepository.ExcluirMensalidadeAsync(idMensalidade);
            return RedirectToAction("Index");
        }
        catch (Exception excecao)
        {
            ViewBag.Erro = "Hmm.. parece que não foi possível deletar essa mensalidade. Erro: " + excecao.Message;

            // so q aí, até agora q ainda nao fiz js, preciso reconstruir minha viewmodel pra segurança da tela não quebrar
            var faturas = await _mensalidadeRepository.ListarMensalidadesAsync();
            var faturaEncontrada = faturas.FirstOrDefault(m => m.Id == idMensalidade);

            return View("DeletarMensalidade", faturaEncontrada);
        }

    }
}