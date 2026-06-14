// aqui seria tipo como se fosse um "garçom", mas que busca e entrega dados.
using Microsoft.AspNetCore.Mvc;
using GerenciadorAlunosV2.ViewModels;
using GerenciadorAlunosV2.Models; 
using GerenciadorAlunosV2.Repositories; 

namespace GerenciadorAlunosV2.Controllers;

public class AlunoController : Controller
{
    private readonly AlunoRepository _alunoRepository;
    // na minha nova funcionalidade de ver o perfil do aluno, tive q injetar o repositorio de mensalidade tb
    private readonly MensalidadeRepository _mensalidadeRepository;
    public AlunoController(AlunoRepository alunoRepository, MensalidadeRepository mensalidadeRepository)
    {
        _alunoRepository = alunoRepository;
        _mensalidadeRepository = mensalidadeRepository;
    }

    // essa seria tipo a ação principal da minha página inicial
    public async Task<IActionResult> Index()
    {
        // crio uma variavel pra pedir os dados pro meu db
        var alunosDoBanco = await _alunoRepository.ListarAsync();
        // aqui que entra a situação da ViewModel. Crio uma variavel pra exibir efetivamente o meu alunosDoBanco
        // uso o LINQ pra fazer um select e crio um objeto "a" e faço o respectivo mapeamento 
        var alunosExibidos = alunosDoBanco.Select(a => new AlunoListaViewModel
        {
            IdAluno = a.Id,
            NomeAluno = a.Nome,
            CpfAluno = a.Cpf,
            EmailAluno = a.Email,
            CelularAluno = a.Celular
        }).ToList();
        // depois eu "entrego" essa variavel q chamei de alunosExibidos pra View
        return View(alunosExibidos);
    }

    [HttpGet]
    public async Task <IActionResult> PerfilAluno (int id)
    {
        var buscaAluno = await _alunoRepository.SelecionarAsync(id.ToString());
        var aluno = buscaAluno.FirstOrDefault();

        if (aluno == null)
        {
            return NotFound();
        }

        var buscaMensalidade = await _mensalidadeRepository.ListarMensalidadesAsync();
        var historicoMensalidade = buscaMensalidade.Where(m => m.AlunoId == id).ToList();

        var viewModel = new AlunoPerfilViewModel
        {
            IdAluno = aluno.Id,
            NomeAluno = aluno.Nome,
            CpfAluno = aluno.Cpf,
            EmailAluno = aluno.Email,
            CelularAluno = aluno.Celular,
            DataNascimentoAluno = aluno.DataNascimento,
            DataCadastroAluno = aluno.DataCadastro,
            HistoricoMensalidades = historicoMensalidade
        };

        return View(viewModel);
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
        // Como lá na minha Repository tinha os try/catch ainda do meu appconsole, nao tava dando pra entender o que tava errado na hora de cadastrar
        // Então eu retirei todos e pesquisei uma maneira mais eficiente de apresentar os erros ao usuário
        // a ideia seria q o aspnet nos "avisa" se algum set da nossa Model barrou algo
        if (!ModelState.IsValid)
        {
            // procuro qual erro que a model gerou
            var erro = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();

            if (erro != null && erro.Exception != null)
            {
                ViewBag.Erro = erro.Exception.Message; // aqui busca do ArgumentException
            }
            else if (erro != null)
            {
                ViewBag.Erro = erro.ErrorMessage;
            }

            return View(novoAluno);
        }
        // se deu tudo certo, aí sim posso seguir pro banco
        try
        {
            // como já tenho meu repository instanciado ali em cima, posso chamar o método pra cadastrar no banco o meu novoAluno
            await _alunoRepository.CadastrarAsync(novoAluno);

            // se deu td certo, a ideia seria retornar à tela principal
            return RedirectToAction("Index");
        }
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
        // uma situação importante pra eu não esquecer: estava colocando inicialmente a dupla possibilidade (ou id ou nome), porém estava dando erro [...] 
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
        if (!ModelState.IsValid)
        {
            // procuro qual erro que a model gerou
            var erro = ModelState.Values.SelectMany(v => v.Errors).FirstOrDefault();

            if (erro != null && erro.Exception != null)
            {
                ViewBag.Erro = erro.Exception.Message; // aqui busca do ArgumentException
            }
            else if (erro != null)
            {
                ViewBag.Erro = erro.ErrorMessage;
            }

            return View(alunoEditado);
        }
        try
        {
            // mando o aluno editado pro meu repository fazer o uptade por meio do AlterarAsync
            await _alunoRepository.AlterarAsync(alunoEditado);
            // quando eu fiz minha implementação do PerfilAluno, invés de voltar pra index, volto pra tela q o aluno teve sua edição
            // dai eu me deparei com o OBJETO ANONIMO: um "envelope" temporário e sem nome, que servirá pra transportar meu dado
            // a minha propriedade id precisa ser idêntica ao nome do parâmetro que o PerfilAluno (int id) espera receber.
            return RedirectToAction("PerfilAluno", new { id = alunoEditado.Id}); 
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