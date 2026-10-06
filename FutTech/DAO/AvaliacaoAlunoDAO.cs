using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class AvaliacaoAlunoDAO
{
    private readonly Conexao _conexao;

    public AvaliacaoAlunoDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<AvaliacaoAluno> Listar()
    {
        var lista = new List<AvaliacaoAluno>();

        using var comando = _conexao.CreateCommand(
            "SELECT * FROM AvaliacaoAluno;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearAvaliacaoAluno(leitor));
        }

        return lista;
    }

    private static AvaliacaoAluno MapearAvaliacaoAluno(MySqlDataReader leitor)
    {
        return new AvaliacaoAluno
        {
            Id = leitor.GetInt32("id_ava"),
            AlunoId = leitor.GetInt32("id_alu_fk"),
            TurmaId = leitor.GetInt32("id_tur_fk"),
            TreinadorId = leitor.GetInt32("id_trei_fk"),
            Data = DateOnly.FromDateTime(leitor.GetDateTime("data_ava")),
            NotaTecnica = leitor.GetInt32("nota_tecnica_ava"),
            NotaFisica = leitor.GetInt32("nota_fisica_ava"),
            NotaTatica = leitor.GetInt32("nota_tatica_ava"),
            NotaComportamental = leitor.GetInt32("nota_comportamental_ava"),
            Observacoes = leitor.GetString("observacoes_ava"),
            Media = leitor.GetDecimal("media_ava")
        };
    }

    // CREATE — cadastra uma nova avaliação de aluno
    public void Inserir(AvaliacaoAluno avaliacao)
    {
        try
        {
            using var con = _conexao.GetConnection();

            string sql = @"
            INSERT INTO AvaliacaoAluno
            (
                id_alu_fk,
                id_tur_fk,
                id_trei_fk,
                data_ava,
                nota_tecnica_ava,
                nota_fisica_ava,
                nota_tatica_ava,
                nota_comportamental_ava,
                observacoes_ava,
                media_ava
            )
            VALUES
            (
                @aluno,
                @turma,
                @treinador,
                @data,
                @notaTecnica,
                @notaFisica,
                @notaTatica,
                @notaComportamental,
                @observacoes,
                @media
            )";

            using var comando = con.CreateCommand();

            comando.CommandText = sql;

            comando.Parameters.AddWithValue(
                "@aluno",
                avaliacao.AlunoId
            );

            comando.Parameters.AddWithValue(
                "@turma",
                avaliacao.TurmaId
            );

            comando.Parameters.AddWithValue(
                "@treinador",
                avaliacao.TreinadorId
            );

            comando.Parameters.AddWithValue(
                "@data",
                avaliacao.Data
            );

            comando.Parameters.AddWithValue(
                "@notaTecnica",
                avaliacao.NotaTecnica
            );

            comando.Parameters.AddWithValue(
                "@notaFisica",
                avaliacao.NotaFisica
            );

            comando.Parameters.AddWithValue(
                "@notaTatica",
                avaliacao.NotaTatica
            );

            comando.Parameters.AddWithValue(
                "@notaComportamental",
                avaliacao.NotaComportamental
            );

            comando.Parameters.AddWithValue(
                "@observacoes",
                avaliacao.Observacoes
            );

            comando.Parameters.AddWithValue(
                "@media",
                avaliacao.Media
            );

            comando.ExecuteNonQuery();
        }
        catch
        {
            throw;
        }
    }
}