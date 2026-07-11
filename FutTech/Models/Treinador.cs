namespace FutTech.Models;

public sealed class Treinador
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
}
