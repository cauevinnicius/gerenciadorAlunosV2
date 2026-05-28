using Microsoft.EntityFrameworkCore;
using GerenciadorAlunosV2.Contexts;
using GerenciadorAlunosV2.Repositories;

var builder = WebApplication.CreateBuilder(args);

// adição da configuração da connection, meu dbcontext e o mysql
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GerenciadorAlunosDbContext>(options => options.UseMySQL(connectionString));


// preciso registrar meus repositorios na injeção de dependência do aspnet
// o addscoped cria uma instância do repositório por requisição web
builder.Services.AddScoped<AlunoRepository>();
builder.Services.AddScoped<MensalidadeRepository>();
builder.Services.AddScoped<UsuarioRepository>();

// preciso incluir os servicos do padrão mvc, q seriam minhas controllers e minhas views
builder.Services.AddControllersWithViews();

var app = builder.Build();

// configuração de ambiente - já veio automaticamente com a criação do projeto
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

// definição da minha rota padrão
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
