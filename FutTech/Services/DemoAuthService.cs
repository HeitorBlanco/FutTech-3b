using FutTech.Models;

namespace FutTech.Services;

public sealed class DemoAuthService
{
    public const string AdminEmail = "admin@futtech.com";
    public const string AdminPassword = "admin123";
    public const string ResponsavelEmail = "responsavel@futtech.com";
    public const string ResponsavelPassword = "resp123";
    public const string TreinadorEmail = "treinador@futtech.com";
    public const string TreinadorPassword = "treinador123";

    private readonly List<CredencialDemo> _credenciais =
    [
        new(
            AdminPassword,
            new UsuarioSistema
            {
                Id = 1,
                Nome = "Carlos Admin",
                Email = AdminEmail,
                Perfil = PerfilUsuario.Administrador,
                Cargo = "Coordenador"
            }),
        new(
            ResponsavelPassword,
            new UsuarioSistema
            {
                Id = 2,
                Nome = "Roberto Pai do Lucas",
                Email = ResponsavelEmail,
                Perfil = PerfilUsuario.Responsavel,
                Cargo = "Responsável"
            }),
        new(
            TreinadorPassword,
            new UsuarioSistema
            {
                Id = 3,
                Nome = "Rafael Costa",
                Email = TreinadorEmail,
                Perfil = PerfilUsuario.Treinador,
                Cargo = "Treinador Sub-13"
            })
    ];

    public UsuarioSistema? Entrar(string email, string senha)
    {
        var emailInformado = email.Trim();

        return _credenciais
            .FirstOrDefault(credencial =>
                credencial.Usuario.Ativo &&
                string.Equals(credencial.Usuario.Email, emailInformado, StringComparison.OrdinalIgnoreCase) &&
                credencial.Senha == senha)
            ?.Usuario;
    }

    public IReadOnlyList<UsuarioSistema> ListarUsuarios()
    {
        return _credenciais
            .Select(credencial => credencial.Usuario)
            .Where(usuario => usuario.Ativo)
            .OrderBy(usuario => usuario.Perfil)
            .ThenBy(usuario => usuario.Nome)
            .ToList();
    }

    public bool EmailJaCadastrado(string email)
    {
        return _credenciais.Any(credencial =>
            credencial.Usuario.Ativo &&
            string.Equals(credencial.Usuario.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public UsuarioSistema CadastrarUsuario(
        string nome,
        string email,
        string senha,
        PerfilUsuario perfil,
        string cargo)
    {
        var usuario = new UsuarioSistema
        {
            Id = _credenciais.Max(credencial => credencial.Usuario.Id) + 1,
            Nome = nome.Trim(),
            Email = email.Trim(),
            Perfil = perfil,
            Cargo = cargo.Trim(),
            Ativo = true
        };

        _credenciais.Add(new CredencialDemo(senha, usuario));
        return usuario;
    }

    public bool RemoverUsuario(int usuarioId)
    {
        var usuario = _credenciais
            .Select(credencial => credencial.Usuario)
            .FirstOrDefault(item => item.Id == usuarioId && item.Ativo);

        if (usuario is null || usuario.Perfil == PerfilUsuario.Administrador)
        {
            return false;
        }

        usuario.Ativo = false;
        return true;
    }

    private sealed record CredencialDemo(string Senha, UsuarioSistema Usuario);
}
