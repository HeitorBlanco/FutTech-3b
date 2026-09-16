using FutTech.Components;
using FutTech.Services;
using FutTech.Configs;
using FutTech.DAO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<AuthService>();

builder.Services.AddScoped<Conexao>();
builder.Services.AddScoped<AlunoDAO>();
builder.Services.AddScoped<TurmaDAO>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

try
{
    using var scope = app.Services.CreateScope();

    var conexao = scope.ServiceProvider.GetRequiredService<Conexao>();

    using var conexaoMySql = conexao.GetConnection();

    Console.WriteLine("CONEXÃO COM O BANCO FUNCIONANDO!");
}
catch (Exception ex)
{
    Console.WriteLine("ERRO NA CONEXÃO:");
    Console.WriteLine(ex.Message);
}

app.Run();