namespace FutTech.Models;

public sealed class Turma
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string DiasDeTreino { get; set; } = string.Empty;
    public TimeOnly Horario { get; set; }
    public int TreinadorId { get; set; }
    public bool Ativa { get; set; } = true;
}
