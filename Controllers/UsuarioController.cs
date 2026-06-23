using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using GerenciadorAlunosV2.ViewModels;
using System.Threading.Tasks;

namespace GerenciadorAlunosV2.Controllers;

public class UsuarioController : Controller
{
    // estou realizando o uso de dois repository previamente criados e realizando a leitura da IdentityUser, também já criado previamente pelo identity
    // user manager: focado em CRUD e demais funcionalidades com dados
    private readonly UserManager<IdentityUser> _userManager;
    // o signinmanager: já é um repositório mais voltado a sessão (cookies)
    private readonly SignInManager <IdentityUser> _signInManager;
    
    // faço a injeção de dependencia dos repositórios
    public UsuarioController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Registrar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(RegistroViewModel model)
    {
        if (ModelState.IsValid)
        {
            // faço a criação de uma variável e instancio a Identity User. Vou usar username e email como email
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };

            // o repositório do usermanager já tem um método de criar!
            var result = await _userManager.CreateAsync(user, model.Senha);

            // dps de registrar, se deu tudo certo, já deixo o usuário logado - realizar uma validação de email acho q seria bala...
            if (result.Succeeded)
            {
                // fui buscar mais explicações do pq do isPersistent: se deixamos true, o signin fica gravado pelos cookies 
                await _signInManager.SignInAsync(user, isPersistent: false);
                
                // redireciona o usuário p index da home
                return RedirectToAction("Index", "Home"); 
            }

            
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        
        return View(model);
    }

    public IActionResult VerificarEmail()
    {
        return View();
    }
}

    
