namespace FutTech.Models;

public sealed class UsuarioSistema
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public PerfilUsuario Perfil { get; set; }
    public string Cargo { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    public string PerfilDescricao => Perfil switch
    {
        PerfilUsuario.Administrador => "Administrador",
        PerfilUsuario.Responsavel => "Responsável",
        PerfilUsuario.Treinador => "Treinador",
        _ => "Usuário"
    };

    public string RotaInicial => Perfil switch
    {
        PerfilUsuario.Administrador => "/admin/dashboard",
        PerfilUsuario.Responsavel => "/responsavel/dashboard",
        PerfilUsuario.Treinador => "/treinador/dashboard",
        _ => "/login"
    };
}
