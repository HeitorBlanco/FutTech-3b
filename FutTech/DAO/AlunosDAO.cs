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
            "SELECT * FROM Aluno;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearAluno(leitor));
        }

        return lista;
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