using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class TurmaDAO
{
    private readonly Conexao _conexao;

    public TurmaDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<Turma> Listar()
    {
        var lista = new List<Turma>();

        using var comando = _conexao.CreateCommand(
            "SELECT * FROM Turma;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearTurma(leitor));
        }

        return lista;
    }

    private static Turma MapearTurma(MySqlDataReader leitor)
    {
        return new Turma
        {
            Id = leitor.GetInt32("id_tur"),

            Nome = leitor.GetString("nome_tur"),

            Categoria = leitor.GetString("categoria_tur"),

            DiasDeTreino = leitor.GetString("dias_de_treino_tur"),

            Horario = TimeOnly.FromTimeSpan(
                leitor.GetTimeSpan("horario_tur")
            ),

            Ativa = leitor.GetBoolean("ativa_tur"),

            TreinadorId = leitor.GetInt32("id_trei_fk")
        };
    }
}