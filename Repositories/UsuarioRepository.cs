namespace GerenciadorAlunosV2.Repositories;
using GerenciadorAlunosV2.Contexts;
using GerenciadorAlunosV2.Models;
using Microsoft.EntityFrameworkCore;
// aqui eu busquei uma ideia mais simples pra garantir a segurança da senha. Encontrei o BCrypt como uma indicação.
//using BCrypt.Net;



public class UsuarioRepository
{
    private readonly GerenciadorAlunosDbContext _context;
    public UsuarioRepository(GerenciadorAlunosDbContext context)
    {
        _context = context;
    }

    // bom, a primeira coisa é eu criar um método pra autenticar o usuario
    public async Task <bool> AutenticarAsync(string usernameDigitado, string senhaDigitada)
    {
        //vou criar uma variável que faça a busca do usuário digitado e compare com o que tenho lá no db
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == usernameDigitado);

        //aí claramente se nao tem ninguem, já vou retornar false
        if (usuario == null) return false;

        // dai vou comparar se a senha digitada é a mesma que tenho no banco. Mas aí vou usar um método que o BCrypt oportuniza
        //bool senhaCorreta = (senhaDigitada, usuario.HashPassword);
        // ARRUMAR ISSO AQUI DPS
        return true;
    }

    // dps um método para cadastrar
    }
    // dps um pra se esqueceu a senha

