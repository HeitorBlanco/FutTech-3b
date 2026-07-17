using FutTech.Models;

namespace FutTech.Services;

public sealed class AuthService
{
    private static readonly IReadOnlyList<(string Email, string Senha, UsuarioSistema Usuario)> Credenciais =
    [
        ("admin@futtech.com", "admin123", new UsuarioSistema
        {
            Id = 1,
            Nome = "Administrador",
            Email = "admin@futtech.com",
            Perfil = PerfilUsuario.Administrador,
            PerfilDescricao = "Administrador",
            Cargo = "Coordenador",
            RotaInicial = "/admin/dashboard"
        }),
        ("responsavel@futtech.com", "resp123", new UsuarioSistema
        {
            Id = 2,
            Nome = "Responsável",
            Email = "responsavel@futtech.com",
            Perfil = PerfilUsuario.Responsavel,
            PerfilDescricao = "Responsável",
            Cargo = "Responsável",
            RotaInicial = "/responsavel/dashboard"
        }),
        ("treinador@futtech.com", "treinador123", new UsuarioSistema
        {
            Id = 3,
            Nome = "Treinador",
            Email = "treinador@futtech.com",
            Perfil = PerfilUsuario.Treinador,
            PerfilDescricao = "Treinador",
            Cargo = "Treinador",
            RotaInicial = "/treinador/dashboard"
        })
    ];

    public UsuarioSistema? Entrar(string email, string senha)
    {
        return Credenciais
            .FirstOrDefault(credencial =>
                string.Equals(credencial.Email, email.Trim(), StringComparison.OrdinalIgnoreCase) &&
                credencial.Senha == senha)
            .Usuario;
    }
}
