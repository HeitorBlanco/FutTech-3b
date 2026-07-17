namespace FutTech.Models;

public sealed class MensalidadeDetalhe
{
    public Mensalidade Mensalidade { get; set; } = new();
    public string NomeAluno { get; set; } = string.Empty;
    public string NomeTurma { get; set; } = string.Empty;
}
