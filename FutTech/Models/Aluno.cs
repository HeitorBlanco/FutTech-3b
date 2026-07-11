namespace FutTech.Models;

public sealed class Aluno
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Responsavel { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public int TurmaId { get; set; }
    public bool Ativo { get; set; } = true;
}
