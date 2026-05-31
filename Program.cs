using Microsoft.EntityFrameworkCore;
using FinanceiroApp.Data;

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var builder = WebApplication.CreateBuilder(args);

// ---- CONFIGURAÇÕES DE SERVIÇOS ----

// Adiciona suporte a Controllers e Views (padrão MVC)
builder.Services.AddControllersWithViews();

// Configura o banco de dados SQLite
// Em produção (Railway), usa a variável de ambiente; localmente usa o arquivo padrão
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default")
    ?? "Data Source=financeiro.db";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

// Configura o sistema de sessão (para manter o usuário logado)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8); // Sessão dura 8 horas
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// ---- CONFIGURAÇÕES DO PIPELINE HTTP ----

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles(); // Permite servir arquivos CSS, JS, imagens
app.UseRouting();
app.UseSession();     // Ativa o sistema de sessão

// Define a rota padrão: Controller/Action/Id
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

// Cria o banco de dados automaticamente na primeira execução
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated(); // Cria as tabelas se não existirem
}

app.Run();
