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

        var comando = _conexao.CreateCommand(
            "SELECT rp.id_pre, rp.id_alu_fk, rp.id_tur_fk, rp.data_pre, rp.presente_pre, a.nome_alu, t.nome_tur FROM RegistroPresenca AS rp INNER JOIN Aluno AS a ON a.id_alu = rp.id_alu_fk LEFT JOIN Turma AS t ON t.id_tur = rp.id_tur_fk ORDER BY rp.data_pre DESC, a.nome_alu;"
        );

        var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        return lista;
    }

    public List<RegistroPresenca> ListarPorTurma(int turmaId, DateOnly data)
    {
        var lista = new List<RegistroPresenca>();

        var comando = _conexao.CreateCommand(
            "SELECT rp.id_pre, rp.id_alu_fk, rp.id_tur_fk, rp.data_pre, rp.presente_pre, a.nome_alu, t.nome_tur FROM RegistroPresenca AS rp INNER JOIN Aluno AS a ON a.id_alu = rp.id_alu_fk LEFT JOIN Turma AS t ON t.id_tur = rp.id_tur_fk WHERE rp.id_tur_fk = @turmaId AND rp.data_pre = @data ORDER BY a.nome_alu;"
        );

        comando.Parameters.AddWithValue("@turmaId", turmaId);
        comando.Parameters.AddWithValue("@data", data.ToDateTime(TimeOnly.MinValue));

        var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        return lista;
    }

    public void SalvarChamada(int turmaId, DateOnly data, IEnumerable<RegistroPresenca> registros)
    {
        foreach (var registro in registros)
        {
            if (registro.AlunoId <= 0)
            {
                continue;
            }

            var idExiste = ObterIdPorAlunoData(turmaId, registro.AlunoId, data);

            if (idExiste > 0)
            {
                const string sqlAtualizar = """
                    UPDATE RegistroPresenca
                    SET presente_pre = @presente,
                        id_tur_fk = @turmaId
                    WHERE id_pre = @id;
                    """;

                using var comandoAtualizar = _conexao.CreateCommand(sqlAtualizar);
                comandoAtualizar.Parameters.AddWithValue("@presente", registro.Presente);
                comandoAtualizar.Parameters.AddWithValue("@turmaId", turmaId);
                comandoAtualizar.Parameters.AddWithValue("@id", idExiste);
                comandoAtualizar.ExecuteNonQuery();

                continue;
            }

            const string sqlInserir = """
                INSERT INTO RegistroPresenca (id_alu_fk, id_tur_fk, data_pre, presente_pre)
                VALUES (@alunoId, @turmaId, @data, @presente);
                """;

            using var comandoInserir = _conexao.CreateCommand(sqlInserir);
            comandoInserir.Parameters.AddWithValue("@alunoId", registro.AlunoId);
            comandoInserir.Parameters.AddWithValue("@turmaId", turmaId);
            comandoInserir.Parameters.AddWithValue("@data", data.ToDateTime(TimeOnly.MinValue));
            comandoInserir.Parameters.AddWithValue("@presente", registro.Presente);
            comandoInserir.ExecuteNonQuery();
        }
    }

    private int ObterIdPorAlunoData(int turmaId, int alunoId, DateOnly data)
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
        comando.Parameters.AddWithValue("@data", data.ToDateTime(TimeOnly.MinValue));

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        return leitor.Read() ? leitor.GetInt32("id_pre") : 0;
    }

    private static RegistroPresenca Mapear(MySqlDataReader leitor)
    {
        return new RegistroPresenca
        {
            Id = leitor.GetInt32("id_pre"),
            AlunoId = leitor.GetInt32("id_alu_fk"),
            TurmaId = leitor.GetInt32("id_tur_fk"),
            Data = DateOnly.FromDateTime(leitor.GetDateTime("data_pre")),
            Presente = leitor.GetBoolean("presente_pre"),
            NomeAluno = DAOHelper.GetString(leitor, "nome_alu"),
            NomeTurma = DAOHelper.GetString(leitor, "nome_tur")
        };
    }
}
