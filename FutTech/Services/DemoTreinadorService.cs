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
        },
        new()
        {
            Id = 2,
            Nome = "Sub-15 Manhã",
            Categoria = "Sub-15",
            DiasDeTreino = "Terça e quinta",
            Horario = new TimeOnly(8, 0),
            TreinadorId = 1
        },
        new()
        {
            Id = 3,
            Nome = "Turma Manhã A",
            Categoria = "Sub-11",
            DiasDeTreino = "Terça e quinta",
            Horario = new TimeOnly(9, 0),
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
        },
        new()
        {
            Id = 2,
            Nome = "Miguel Andrade",
            Responsavel = "Patrícia Andrade",
            DataNascimento = new DateOnly(2011, 9, 21),
            TurmaId = 2
        },
        new()
        {
            Id = 3,
            Nome = "Gabriel Santos",
            Responsavel = "Ana Santos",
            DataNascimento = new DateOnly(2015, 2, 6),
            TurmaId = 3
        }
    ];

    private readonly List<RegistroPresenca> _presencas =
    [
        new()
        {
            Id = 1,
            AlunoId = 1,
            TurmaId = 1,
            Data = new DateOnly(2026, 7, 7),
            Presente = true
        },
        new()
        {
            Id = 2,
            AlunoId = 2,
            TurmaId = 2,
            Data = new DateOnly(2026, 7, 7),
            Presente = false
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

    public IReadOnlyList<Aluno> ObterAlunosDaTurma(int turmaId, int treinadorId = 1)
    {
        var turmaDoTreinador = _turmas.Any(turma =>
            turma.Id == turmaId &&
            turma.TreinadorId == treinadorId &&
            turma.Ativa);

        if (!turmaDoTreinador)
        {
            return [];
        }

        return _alunos
            .Where(aluno => aluno.Ativo && aluno.TurmaId == turmaId)
            .OrderBy(aluno => aluno.Nome)
            .ToList();
    }

    public IReadOnlyList<RegistroPresenca> ObterPresencasDaTurma(int turmaId, DateOnly data, int treinadorId = 1)
    {
        return ObterAlunosDaTurma(turmaId, treinadorId)
            .Select(aluno =>
                _presencas.FirstOrDefault(presenca =>
                    presenca.AlunoId == aluno.Id &&
                    presenca.TurmaId == turmaId &&
                    presenca.Data == data)
                ?? new RegistroPresenca
                {
                    AlunoId = aluno.Id,
                    TurmaId = turmaId,
                    Data = data,
                    Presente = false
                })
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

    public bool RemoverAluno(int alunoId)
    {
        var aluno = _alunos.FirstOrDefault(item => item.Id == alunoId && item.Ativo);

        if (aluno is null)
        {
            return false;
        }

        aluno.Ativo = false;
        return true;
    }

    public bool SalvarChamada(int turmaId, DateOnly data, IReadOnlyDictionary<int, bool> presencasPorAluno)
    {
        var alunosDaTurma = ObterAlunosDaTurma(turmaId)
            .Select(aluno => aluno.Id)
            .ToHashSet();

        if (alunosDaTurma.Count == 0)
        {
            return false;
        }

        foreach (var alunoId in alunosDaTurma)
        {
            var registro = _presencas.FirstOrDefault(presenca =>
                presenca.AlunoId == alunoId &&
                presenca.TurmaId == turmaId &&
                presenca.Data == data);

            if (registro is null)
            {
                registro = new RegistroPresenca
                {
                    Id = _presencas.Count == 0 ? 1 : _presencas.Max(item => item.Id) + 1,
                    AlunoId = alunoId,
                    TurmaId = turmaId,
                    Data = data
                };

                _presencas.Add(registro);
            }

            registro.Presente = presencasPorAluno.TryGetValue(alunoId, out var presente) && presente;
        }

        return true;
    }

    public bool AtualizarTurma(
        int turmaId,
        string nome,
        string categoria,
        string diasDeTreino,
        TimeOnly horario)
    {
        var turma = _turmas.FirstOrDefault(item => item.Id == turmaId && item.Ativa);

        if (turma is null)
        {
            return false;
        }

        turma.Nome = nome.Trim();
        turma.Categoria = categoria.Trim();
        turma.DiasDeTreino = diasDeTreino.Trim();
        turma.Horario = horario;
        return true;
    }

    public bool RemoverTurma(int turmaId)
    {
        var turma = _turmas.FirstOrDefault(item => item.Id == turmaId && item.Ativa);

        if (turma is null)
        {
            return false;
        }

        turma.Ativa = false;
        return true;
    }

    public string ObterNomeTurma(int turmaId)
    {
        return _turmas.FirstOrDefault(turma => turma.Id == turmaId)?.Nome ?? "Sem turma";
    }
}
