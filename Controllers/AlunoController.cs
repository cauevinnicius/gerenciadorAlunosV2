// aqui seria tipo como se fosse um "garçom", mas que busca e entrega dados.
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GerenciadorAlunosV2.Models; 
using GerenciadorAlunosV2.Repositories; 

namespace GerenciadorAlunosV2.Controllers;

public class AlunoController : Controller
{
    private readonly AlunoRepository _alunoRepository;
    public AlunoController(AlunoRepository alunoRepository)
    {
        _alunoRepository = alunoRepository;
    }

    // essa seria tipo a ação principal da minha página inicial
    public async Task<IActionResult> Index()
    {
        // crio uma variavel pra pedir os dados pro meu db
        var alunosDoBanco = await _alunoRepository.ListarAsync();
        // depois eu "entrego" essa variavel q chamei de alunosDoBanco pra View
        return View(alunosDoBanco);
    }

    // pro método de cadastrar um novo aluno, primeiro eu vou ter que fazer um httpget pra aparecer uma tela sem preenchimento ao usuário.
    [HttpGet]
    public IActionResult CadastrarAluno()
    {
        return View();
    }

    // depois, quando o usuário clicar em "salvar", preciso fazer um httppost pra isso
    [HttpPost]
    // faço um método assincrono pra cadastrar o aluno pq preciso pedir informações "de fora". Pego meu modelo e crio um objeto pra ele
    public async Task <IActionResult> CadastrarAluno(AlunoModel novoAluno)
    {
        try
        {
            // como já tenho meu repository instanciado ali em cima, posso chamar o método pra cadastrar no banco o meu novoAluno
            await _alunoRepository.CadastrarAsync(novoAluno);

            // se deu td certo, a ideia seria retornar à tela principal
            return RedirectToAction("Index");
        }
        // posso por minhas excecoes previstas na minha classe molde de Aluno (incrível!)
        catch (ArgumentException excecao)
        {
            ViewBag.Erro = excecao.Message;
            // pra não recomeçar do zero, a ideia seria manter os dados já digitados
            return View(novoAluno);
        }
        // e, também, minhas excecoes prevista no db
        catch (Exception excecao)
        {
            ViewBag.Erro = "Erro interno " + excecao.Message;
            return View(novoAluno);
        }
    }

    // seguindo, vou criar minha função de editar. Vou fazer um httpget pra buscar o aluno. Já inseri a "localizacao" dele no meu Index.cshtml. 
    [HttpGet]
    public async Task <IActionResult> EditarAluno (int id)
    {
        // como eu tenho um get, já vou aproveitar minha SelecionarAsync q já tinha feito no outro projeto. Pra relembrar, vai retornar uma lista com o primeiro id q bater
        // uma situação importante pra eu não esquecer: estava colocando inicialmente a dupla possibilidade (ou id ou nome), porém estava dando erro. 
        // [...] Ocorre que meu usuário já está vendo uma lista e vai selecionar aquele aluno. Consequentemente,  URL precisaa ter um id único. 
        var busca = await _alunoRepository.SelecionarAsync(id.ToString());
        var alunoEncontrado = busca.FirstOrDefault();
        
        if (alunoEncontrado == null)
        {
            return NotFound();
        }

        return View(alunoEncontrado);
    }

    // depois que o usuário digitou os dados q quer editar, eu vou fazer meu post.
    [HttpPost]
    public async Task <IActionResult> EditarAluno (AlunoModel alunoEditado)
    {
        try
        {
            // mando o aluno editado pro meu repository fazer o uptade por meio do AlterarAsync
            await _alunoRepository.AlterarAsync(alunoEditado);
            return RedirectToAction("Index"); 
        }
        catch (ArgumentException excecao)
        {
            ViewBag.Erro = excecao.Message;
            // a ideia aqui é mostrar na tela de edição as situações que não podem, setadas no meu AlunoModel
            return View(alunoEditado);
        }
        catch (Exception excecao)
        {
            ViewBag.Erro = "Erro interno: " + excecao.Message;
            return View(alunoEditado);
        }
    }

    //blz, agora pra fechar meu crud simples, a deleção
    // seguindo a ideia das demais, primeiro um httpget pra apresentar meu aluno em tela.
    [HttpGet]
    public async Task <IActionResult> DeletarAluno (int id)
    {
        // preciso q seja um id único. Lá eu tinha um string parametroBusca. 
        var busca = await _alunoRepository.SelecionarAsync(id.ToString());
        var alunoEncontrado = busca.FirstOrDefault();

        if (alunoEncontrado == null)
        {
            return NotFound();
        }

        return View(alunoEncontrado);
    }

    // agora pra efetivamente ocorrer a deleção, vou fazer um post. 
    [HttpPost]
    public async Task <IActionResult> ConfirmarDelecao (int id)
    {
        try
        {
            await _alunoRepository.DeletarAsync(id);
            return RedirectToAction("Index");
        }
        catch(Exception excecao)
        {
            ViewBag.Erro = "Hmm.. parece que não foi possível deletar esse aluno. Erro: " + excecao.Message;

            // se der erro, eu preciso buscar novamente o aluno pra apresentar na tela de erro
            var busca = await _alunoRepository.SelecionarAsync(id.ToString());
            return View("DeletarAluno", busca.FirstOrDefault());
        }
    }
}