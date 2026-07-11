namespace FutTech.Models;

public sealed class AdminDashboardResumo
{
    public string NomeEscolinha { get; set; } = string.Empty;
    public string NomeAdministrador { get; set; } = string.Empty;
    public string CargoAdministrador { get; set; } = string.Empty;
    public DateOnly PeriodoReferencia { get; set; }
    public IReadOnlyList<Aluno> Alunos { get; set; } = [];
    public IReadOnlyList<Turma> Turmas { get; set; } = [];
    public IReadOnlyList<Treinador> Treinadores { get; set; } = [];
    public IReadOnlyList<Mensalidade> Mensalidades { get; set; } = [];
    public IReadOnlyList<Comunicado> Comunicados { get; set; } = [];
    public IReadOnlyList<RegistroPresenca> Presencas { get; set; } = [];

    public int AlunosAtivos => Alunos.Count(aluno => aluno.Ativo);

    public int TurmasAtivas => Turmas.Count(turma => turma.Ativa);

    public int MensalidadesPendentes => Mensalidades.Count(mensalidade =>
        mensalidade.Status is StatusMensalidade.Pendente or StatusMensalidade.Atrasada);

    public decimal ValorAReceber => Mensalidades
        .Where(mensalidade => mensalidade.Status is StatusMensalidade.Pendente or StatusMensalidade.Atrasada)
        .Sum(mensalidade => mensalidade.Valor);

    public decimal RecebidoMesAtual => Mensalidades
        .Where(mensalidade =>
            mensalidade.Status == StatusMensalidade.Pago &&
            mensalidade.DataPagamento.HasValue &&
            mensalidade.DataPagamento.Value.Month == PeriodoReferencia.Month &&
            mensalidade.DataPagamento.Value.Year == PeriodoReferencia.Year)
        .Sum(mensalidade => mensalidade.Valor);

    public double FrequenciaMediaPercentual
    {
        get
        {
            if (Presencas.Count == 0)
            {
                return 0;
            }

            var presencasConfirmadas = Presencas.Count(presenca => presenca.Presente);
            return presencasConfirmadas / (double)Presencas.Count * 100;
        }
    }

    public IEnumerable<Comunicado> UltimosComunicados => Comunicados
        .OrderByDescending(comunicado => comunicado.PublicadoEm)
        .Take(3);

    public IEnumerable<Turma> TurmasEmDestaque => Turmas
        .Where(turma => turma.Ativa)
        .Take(3);
}
