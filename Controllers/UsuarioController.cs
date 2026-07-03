using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GerenciadorAlunosV2.ViewModels;
using GerenciadorAlunosV2.Models;
using System.Threading.Tasks;

namespace GerenciadorAlunosV2.Controllers;

public class UsuarioController : Controller
{
    // estou realizando o uso de dois repository previamente criados e realizando a leitura da IdentityUser, também já criado previamente pelo identity. ATUALIZAÇÂO: tive que mudar para UsuarioModel para usufruir do NomeCompleto.
    // user manager: focado em CRUD e demais funcionalidades com dados
    private readonly UserManager<UsuarioModel> _userManager;
    // o signinmanager: já é um repositório mais voltado a sessão (cookies)
    private readonly SignInManager <UsuarioModel> _signInManager;
    
    // faço a injeção de dependencia dos repositórios
    public UsuarioController(UserManager<UsuarioModel> userManager, SignInManager<UsuarioModel> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }    
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
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

            return View(model);
        }
        
        try
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, model.LembrarMe, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "E-mail ou senha inválidos!");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"Ocorreu um erro ao tentar fazer login: {ex.Message}");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Registrar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(NovoUsuarioViewModel model)
    {
        if (ModelState.IsValid)
        {
            // faço a criação de uma variável e instancio a Identity User. Vou usar username e email como email
            var user = new UsuarioModel { UserName = model.Email, Email = model.Email, NomeCompleto = "Novo Usuario" };

            // o repositório do usermanager já tem um método de criar!
            var result = await _userManager.CreateAsync(user, model.Senha);

            // dps de registrar, se deu tudo certo, já deixo o usuário logado - realizar uma validação de email acho q seria bala...
            if (result.Succeeded)
            {
                // fui buscar mais explicações do pq do isPersistent: se deixamos true, o signin fica gravado pelos cookies 
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                // redireciona o usuário p index da home
                TempData["MensagemSucesso"] = "Cadastro realizado com sucesso! Bem-vindo!";
                return RedirectToAction("Index", "Home"); 
            }

            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        
        return View(model);
    }
    [HttpGet]
    public IActionResult VerificarEmail()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult VerificarEmail(VerificarEmailUsuarioViewModel model)
    {
        if (ModelState.IsValid)
        {
            // aqui eu verifico se o email existe no banco de dados
            var user = _userManager.FindByEmailAsync(model.Email).Result;
            if (user != null)
            {
                // aqui eu poderia gerar um token e enviar para o email do usuário, mas por enquanto vou apenas redirecionar para a tela de alterar senha
                return RedirectToAction("AlterarSenha");
            }
            ModelState.AddModelError(string.Empty, "Email não encontrado!");
        }
        return View(model);
    }

    [HttpGet]
    public IActionResult AlterarSenha()
    {
        return View();
    }

    //TESTAR!!!
    [HttpPost]
    public async Task<IActionResult> AlterarSenha(AlterarSenhaViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NovaSenha);
                if (result.Succeeded)
                {
                    TempData["MensagemSucesso"] = "Senha alterada com sucesso!";
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Email não encontrado!");
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login", "Usuario");
    }
}

    
