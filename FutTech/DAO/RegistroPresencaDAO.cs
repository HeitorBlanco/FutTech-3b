using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class RegistroPresencaDAO
{
    private readonly Conexao _conexao;

    public RegistroPresencaDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<RegistroPresenca> Listar()
    {
        var lista = new List<RegistroPresenca>();

        const string sql = """
            SELECT
                rp.id_pre,
                rp.id_alu_fk,
                rp.id_tur_fk,
                rp.data_pre,
                rp.presente_pre,
                a.nome_alu,
                t.nome_tur
            FROM RegistroPresenca AS rp
            INNER JOIN Aluno AS a
                ON a.id_alu = rp.id_alu_fk
            LEFT JOIN Turma AS t
                ON t.id_tur = rp.id_tur_fk
            ORDER BY rp.data_pre DESC, a.nome_alu;
            """;

        using var comando = _conexao.CreateCommand(sql);
        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        return lista;
    }

    public List<Aluno> ListarAlunos()
    {
        var lista = new List<Aluno>();

        const string sql = """
            SELECT
                id_alu,
                nome_alu
            FROM Aluno
            ORDER BY nome_alu;
            """;

        using var comando = _conexao.CreateCommand(sql);
        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(new Aluno
            {
                Id = leitor.GetInt32("id_alu"),
                Nome = leitor.GetString("nome_alu")
            });
        }

        return lista;
    }

    public List<Turma> ListarTurmas()
    {
        var lista = new List<Turma>();

        const string sql = """
            SELECT
                id_tur,
                nome_tur
            FROM Turma
            ORDER BY nome_tur;
            """;

        using var comando = _conexao.CreateCommand(sql);
        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(new Turma
            {
                Id = leitor.GetInt32("id_tur"),
                Nome = leitor.GetString("nome_tur")
            });
        }

        return lista;
    }

    public void Inserir(RegistroPresenca registro)
    {
        const string sql = """
            INSERT INTO RegistroPresenca
            (
                id_alu_fk,
                id_tur_fk,
                data_pre,
                presente_pre
            )
            VALUES
            (
                @alunoId,
                @turmaId,
                @data,
                @presente
            );
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue("@alunoId", registro.AlunoId);
        comando.Parameters.AddWithValue("@turmaId", registro.TurmaId);
        comando.Parameters.AddWithValue(
            "@data",
            registro.Data.ToDateTime(TimeOnly.MinValue)
        );
        comando.Parameters.AddWithValue("@presente", registro.Presente);

        comando.ExecuteNonQuery();
    }

    public List<RegistroPresenca> ListarPorTurma(
        int turmaId,
        DateOnly data)
    {
        var lista = new List<RegistroPresenca>();

        const string sql = """
            SELECT
                rp.id_pre,
                rp.id_alu_fk,
                rp.id_tur_fk,
                rp.data_pre,
                rp.presente_pre,
                a.nome_alu,
                t.nome_tur
            FROM RegistroPresenca AS rp
            INNER JOIN Aluno AS a
                ON a.id_alu = rp.id_alu_fk
            LEFT JOIN Turma AS t
                ON t.id_tur = rp.id_tur_fk
            WHERE rp.id_tur_fk = @turmaId
              AND rp.data_pre = @data
            ORDER BY a.nome_alu;
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue("@turmaId", turmaId);
        comando.Parameters.AddWithValue(
            "@data",
            data.ToDateTime(TimeOnly.MinValue)
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        return lista;
    }

    public void SalvarChamada(
        int turmaId,
        DateOnly data,
        IEnumerable<RegistroPresenca> registros)
    {
        foreach (var registro in registros)
        {
            if (registro.AlunoId <= 0)
            {
                continue;
            }

            int idExistente = ObterIdPorAlunoData(
                turmaId,
                registro.AlunoId,
                data
            );

            if (idExistente > 0)
            {
                const string sql = """
                    UPDATE RegistroPresenca
                    SET presente_pre = @presente
                    WHERE id_pre = @id;
                    """;

                using var comando = _conexao.CreateCommand(sql);

                comando.Parameters.AddWithValue(
                    "@presente",
                    registro.Presente
                );

                comando.Parameters.AddWithValue("@id", idExistente);

                comando.ExecuteNonQuery();
            }
            else
            {
                registro.TurmaId = turmaId;
                registro.Data = data;

                Inserir(registro);
            }
        }
    }

    public void Excluir(int id)
    {
        const string sql = """
            DELETE FROM RegistroPresenca
            WHERE id_pre = @id;
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }

    private int ObterIdPorAlunoData(
        int turmaId,
        int alunoId,
        DateOnly data)
    {
        const string sql = """
            SELECT id_pre
            FROM RegistroPresenca
            WHERE id_alu_fk = @alunoId
              AND id_tur_fk = @turmaId
              AND data_pre = @data
            LIMIT 1;
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue("@alunoId", alunoId);
        comando.Parameters.AddWithValue("@turmaId", turmaId);
        comando.Parameters.AddWithValue(
            "@data",
            data.ToDateTime(TimeOnly.MinValue)
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        if (leitor.Read())
        {
            return leitor.GetInt32("id_pre");
        }

        return 0;
    }

    private static RegistroPresenca Mapear(
        MySqlDataReader leitor)
    {
        return new RegistroPresenca
        {
            Id = leitor.GetInt32("id_pre"),
            AlunoId = leitor.GetInt32("id_alu_fk"),
            TurmaId = leitor.GetInt32("id_tur_fk"),
            Data = DateOnly.FromDateTime(
                leitor.GetDateTime("data_pre")
            ),
            Presente = leitor.GetBoolean("presente_pre"),
            NomeAluno = DAOHelper.GetString(
                leitor,
                "nome_alu"
            ),
            NomeTurma = DAOHelper.GetString(
                leitor,
                "nome_tur"
            )
        };
    }
}