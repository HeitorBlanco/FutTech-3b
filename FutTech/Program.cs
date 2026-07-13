using FutTech.Components;
using FutTech.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<DemoComunicadoService>();
builder.Services.AddSingleton<DemoAdminDashboardService>();
builder.Services.AddSingleton<DemoAuthService>();
builder.Services.AddSingleton<DemoMenuService>();
builder.Services.AddSingleton<DemoTreinadorService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
