using FutTech.Models;

namespace FutTech.Services;

public sealed class DemoTreinadorService
{
    private readonly List<Turma> _turmas =
    [
        new()
        {
            Id = 1,
            Nome = "Sub-13 Tarde",
            Categoria = "Sub-13",
            DiasDeTreino = "Segunda e quarta",
            Horario = new TimeOnly(16, 30),
            TreinadorId = 1
        }
    ];

    private readonly List<Aluno> _alunos =
    [
        new()
        {
            Id = 1,
            Nome = "Lucas Martins",
            Responsavel = "Roberto Pai do Lucas",
            DataNascimento = new DateOnly(2013, 5, 12),
            TurmaId = 1
        }
    ];

    public IReadOnlyList<Turma> ObterTurmasDoTreinador(int treinadorId = 1)
    {
        return _turmas
            .Where(turma => turma.TreinadorId == treinadorId && turma.Ativa)
            .OrderBy(turma => turma.Nome)
            .ToList();
    }

    public IReadOnlyList<Aluno> ObterAlunosDoTreinador(int treinadorId = 1)
    {
        var turmasDoTreinador = ObterTurmasDoTreinador(treinadorId)
            .Select(turma => turma.Id)
            .ToHashSet();

        return _alunos
            .Where(aluno => aluno.Ativo && turmasDoTreinador.Contains(aluno.TurmaId))
            .OrderBy(aluno => aluno.Nome)
            .ToList();
    }

    public Aluno AdicionarAluno(
        string nome,
        string responsavel,
        DateOnly dataNascimento,
        int turmaId)
    {
        var aluno = new Aluno
        {
            Id = _alunos.Count == 0 ? 1 : _alunos.Max(item => item.Id) + 1,
            Nome = nome.Trim(),
            Responsavel = responsavel.Trim(),
            DataNascimento = dataNascimento,
            TurmaId = turmaId,
            Ativo = true
        };

        _alunos.Add(aluno);
        return aluno;
    }

    public string ObterNomeTurma(int turmaId)
    {
        return _turmas.FirstOrDefault(turma => turma.Id == turmaId)?.Nome ?? "Sem turma";
    }
}
