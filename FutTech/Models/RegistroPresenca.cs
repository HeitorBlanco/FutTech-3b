namespace FutTech.Models;

public sealed class RegistroPresenca
{
    public int Id { get; set; }

    public int AlunoId { get; set; }

    public int TurmaId { get; set; }

    public DateOnly Data { get; set; }

    public bool Presente { get; set; }

    public string NomeAluno { get; set; } = string.Empty;

    public string NomeTurma { get; set; } = string.Empty;
}