using FutTech.Models;

namespace FutTech.Services;

public sealed class DemoAdminDashboardService
{
    public AdminDashboardResumo ObterResumo()
    {
        var periodo = new DateOnly(2026, 7, 1);

        var treinadores = new List<Treinador>
        {
            new() { Id = 1, Nome = "Rafael Costa", Cargo = "Treinador Sub-13" }
        };

        var turmas = new List<Turma>
        {
            new()
            {
                Id = 1,
                Nome = "Sub-13 Tarde",
                Categoria = "Sub-13",
                DiasDeTreino = "Segunda e quarta",
                Horario = new TimeOnly(16, 30),
                TreinadorId = 1
            }
        };

        var alunos = new List<Aluno>
        {
            new()
            {
                Id = 1,
                Nome = "Lucas Martins",
                Responsavel = "Roberto Pai do Lucas",
                DataNascimento = new DateOnly(2013, 5, 12),
                TurmaId = 1
            }
        };

        var mensalidades = new List<Mensalidade>
        {
            new()
            {
                Id = 1,
                AlunoId = 1,
                Competencia = periodo,
                Valor = 180,
                Vencimento = new DateOnly(2026, 7, 10),
                Status = StatusMensalidade.Pendente
            }
        };

        var comunicados = new List<Comunicado>
        {
            new()
            {
                Id = 1,
                Titulo = "Treino cancelado amanhã",
                Conteudo = "Campo em manutenção no período da tarde.",
                PublicadoEm = new DateOnly(2026, 7, 3),
                Autor = "Carlos Admin",
                Destacado = true
            }
        };

        var presencas = new List<RegistroPresenca>
        {
            new()
            {
                Id = 1,
                AlunoId = 1,
                TurmaId = 1,
                Data = new DateOnly(2026, 7, 1),
                Presente = true
            }
        };

        return new AdminDashboardResumo
        {
            NomeEscolinha = "Escolinha Brasil FC (Demo)",
            NomeAdministrador = "Carlos Admin",
            CargoAdministrador = "Coordenador",
            PeriodoReferencia = periodo,
            Alunos = alunos,
            Turmas = turmas,
            Treinadores = treinadores,
            Mensalidades = mensalidades,
            Comunicados = comunicados,
            Presencas = presencas
        };
    }
}
