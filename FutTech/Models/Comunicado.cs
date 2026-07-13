namespace FutTech.Models;

public sealed class Comunicado
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Conteudo { get; set; } = string.Empty;
    public DateOnly PublicadoEm { get; set; }
    public TimeOnly PublicadoAs { get; set; }
    public string Autor { get; set; } = string.Empty;
    public string Categoria { get; set; } = "Geral";
    public bool Destacado { get; set; }
    public bool Ativo { get; set; } = true;
}
