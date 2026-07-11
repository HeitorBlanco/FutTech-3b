namespace FutTech.Models;

public sealed class Mensalidade
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public DateOnly Competencia { get; set; }
    public decimal Valor { get; set; }
    public DateOnly Vencimento { get; set; }
    public DateOnly? DataPagamento { get; set; }
    public StatusMensalidade Status { get; set; }
}
