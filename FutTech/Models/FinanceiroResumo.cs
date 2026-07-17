namespace FutTech.Models;

public sealed class FinanceiroResumo
{
    public decimal RecebidoMesAtual { get; set; }
    public int Pendentes { get; set; }
    public decimal TotalAReceber { get; set; }
    public decimal TotalPago { get; set; }
}
