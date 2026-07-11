using FutTech.Models;

namespace FutTech.Services;

public sealed class DemoMenuService
{
    private const string IconeDashboard = "M4 19h16M7 16V9m5 7V5m5 11v-4M5 13l4-4 4 3 6-7";
    private const string IconeUsuarios = "M17 21v-2a4 4 0 0 0-4-4H7a4 4 0 0 0-4 4v2M10 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8m11 10v-2a4 4 0 0 0-3-3.87M17 3.13a4 4 0 0 1 0 7.75";
    private const string IconeCamadas = "m12 3 9 5-9 5-9-5 9-5Zm-7 9 7 4 7-4M5 16l7 4 7-4";
    private const string IconePresenca = "M9 11l2 2 4-5M5 4h14v17l-7-3-7 3V4Z";
    private const string IconeFinanceiro = "M3 7h18v12H3V7Zm0 4h18M7 15h3";
    private const string IconeComunicados = "M3 11v2h4l10 6V5L7 11H3Zm14 1h4M20 7l-2 2m0 6 2 2";
    private const string IconeEstrela = "m12 3 2.7 5.48 6.05.88-4.38 4.27 1.03 6.02L12 16.81 6.6 19.65l1.03-6.02-4.38-4.27 6.05-.88L12 3Z";
    private const string IconeAtleta = "M12 12a4 4 0 1 0 0-8 4 4 0 0 0 0 8Zm-7 9a7 7 0 0 1 14 0M12 14v7";

    public IReadOnlyList<MenuItemSistema> ObterMenu(PerfilUsuario perfil)
    {
        return perfil switch
        {
            PerfilUsuario.Administrador => ObterMenuAdmin(),
            PerfilUsuario.Responsavel => ObterMenuResponsavel(),
            PerfilUsuario.Treinador => ObterMenuTreinador(),
            _ => []
        };
    }

    private static IReadOnlyList<MenuItemSistema> ObterMenuAdmin() =>
    [
        Criar("dashboard", "Dashboard", "/admin/dashboard", IconeDashboard),
        Criar("usuarios", "Usuários", "/admin/usuarios", IconeUsuarios),
        Criar("financeiro", "Financeiro", "/admin/financeiro", IconeFinanceiro, false),
        Criar("comunicados", "Comunicados", "/admin/comunicados", IconeComunicados, false),
        Criar("avaliacoes", "Avaliações", "/admin/avaliacoes", IconeEstrela, false)
    ];

    private static IReadOnlyList<MenuItemSistema> ObterMenuResponsavel() =>
    [
        Criar("atletas", "Meus Atletas", "/responsavel/dashboard", IconeAtleta),
        Criar("comunicados", "Comunicados", "/responsavel/comunicados", IconeComunicados, false),
        Criar("financeiro", "Financeiro", "/responsavel/financeiro", IconeFinanceiro, false)
    ];

    private static IReadOnlyList<MenuItemSistema> ObterMenuTreinador() =>
    [
        Criar("dashboard", "Dashboard", "/treinador/dashboard", IconeDashboard),
        Criar("alunos", "Alunos", "/treinador/alunos", IconeUsuarios),
        Criar("turmas", "Minhas Turmas", "/treinador/turmas", IconeCamadas, false),
        Criar("presenca", "Presença", "/treinador/presenca", IconePresenca, false),
        Criar("avaliacoes", "Avaliações", "/treinador/avaliacoes", IconeEstrela, false),
        Criar("comunicados", "Comunicados", "/treinador/comunicados", IconeComunicados, false)
    ];

    private static MenuItemSistema Criar(
        string chave,
        string texto,
        string url,
        string iconePath,
        bool disponivel = true)
    {
        return new MenuItemSistema
        {
            Chave = chave,
            Texto = texto,
            Url = url,
            IconePath = iconePath,
            Disponivel = disponivel
        };
    }
}
