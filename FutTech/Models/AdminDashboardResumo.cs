namespace FutTech.Models;

public sealed class AdminDashboardResumo
{
    public string NomeEscolinha { get; set; } = string.Empty;
    public string NomeAdministrador { get; set; } = string.Empty;
    public string CargoAdministrador { get; set; } = string.Empty;
    public DateOnly PeriodoReferencia { get; set; }
    public int AlunosAtivos { get; set; }
    public int TurmasAtivas { get; set; }
    public int MensalidadesPendentes { get; set; }
    public decimal ValorAReceber { get; set; }
    public decimal RecebidoMesAtual { get; set; }
    public double FrequenciaMediaPercentual { get; set; }
    public IReadOnlyList<Aluno> Alunos { get; set; } = [];
    public IReadOnlyList<Turma> Turmas { get; set; } = [];
    public IReadOnlyList<Treinador> Treinadores { get; set; } = [];
    public IReadOnlyList<Mensalidade> Mensalidades { get; set; } = [];
    public IReadOnlyList<Comunicado> Comunicados { get; set; } = [];
    public IReadOnlyList<RegistroPresenca> Presencas { get; set; } = [];
}
