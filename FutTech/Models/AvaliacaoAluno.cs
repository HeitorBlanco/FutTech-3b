namespace FutTech.Models;

public sealed class AvaliacaoAluno
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public int TurmaId { get; set; }
    public int TreinadorId { get; set; }
    public DateOnly Data { get; set; }
    public int NotaTecnica { get; set; }
    public int NotaFisica { get; set; }
    public int NotaTatica { get; set; }
    public int NotaComportamental { get; set; }
    public string Observacoes { get; set; } = string.Empty;

    public decimal Media => (NotaTecnica + NotaFisica + NotaTatica + NotaComportamental) / 4m;
}
