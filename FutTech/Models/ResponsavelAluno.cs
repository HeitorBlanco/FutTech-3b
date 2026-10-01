namespace FutTech.Models;

public sealed class ResponsavelAluno
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public int AlunoId { get; set; }
    public string Parentesco { get; set; } = string.Empty;
    public bool Principal { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string NomeAluno { get; set; } = string.Empty;
}