namespace FutTech.Models;

public sealed class Comunicado
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public DateOnly PublicadoEm { get; set; }
    public string Autor { get; set; } = string.Empty;
    public bool Destacado { get; set; }
}
