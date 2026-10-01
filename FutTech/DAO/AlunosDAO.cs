using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class AlunoDAO
{
    private readonly Conexao _conexao;

    public AlunoDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<Aluno> Listar()
    {
        var lista = new List<Aluno>();

        using var comando = _conexao.CreateCommand(
            "SELECT * FROM Aluno ORDER BY nome_alu;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearAluno(leitor));
        }

        return lista;
    }

    public void Inserir(Aluno aluno)
    {
        using var comando = _conexao.CreateCommand(@"
            INSERT INTO Aluno
            (
                nome_alu,
                responsavel_alu,
                data_nascimento_alu,
                id_tur_fk,
                ativo_alu
            )
            VALUES
            (
                @nome,
                @responsavel,
                @dataNascimento,
                @turmaId,
                @ativo
            );
        ");

        comando.Parameters.AddWithValue("@nome", aluno.Nome);
        comando.Parameters.AddWithValue("@responsavel", aluno.Responsavel);

        comando.Parameters.AddWithValue(
            "@dataNascimento",
            aluno.DataNascimento!.Value.ToDateTime(TimeOnly.MinValue)
        );

        comando.Parameters.AddWithValue("@turmaId", aluno.TurmaId);
        comando.Parameters.AddWithValue("@ativo", aluno.Ativo);

        comando.ExecuteNonQuery();
    }

    private static Aluno MapearAluno(MySqlDataReader leitor)
    {
        return new Aluno
        {
            Id = leitor.GetInt32("id_alu"),

            Nome = leitor.GetString("nome_alu"),

            Responsavel = leitor.GetString("responsavel_alu"),

            DataNascimento = DateOnly.FromDateTime(
                leitor.GetDateTime("data_nascimento_alu")
            ),

            TurmaId = leitor.GetInt32("id_tur_fk"),

            Ativo = leitor.GetBoolean("ativo_alu")
        };
    }
}