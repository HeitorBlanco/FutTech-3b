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

        var comando = _conexao.CreateCommand(
            "SELECT id_tur, nome_tur, categoria_tur, dias_de_treino_tur, horario_tur, ativa_tur, id_trei_fk FROM Turma;"
        );

        var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        leitor.Close();

        return lista;
    }

    public void Inserir(Turma turma)
    {
        var comando = _conexao.CreateCommand(
            "INSERT INTO Turma (nome_tur, categoria_tur, dias_de_treino_tur, horario_tur, ativa_tur, id_trei_fk) VALUES (@nome, @categoria, @diasDeTreino, @horario, @ativa, @treinadorId);"
        );

        comando.Parameters.AddWithValue("@nome", turma.Nome);
        comando.Parameters.AddWithValue("@categoria", turma.Categoria);
        comando.Parameters.AddWithValue("@diasDeTreino", turma.DiasDeTreino);
        comando.Parameters.AddWithValue("@horario", turma.Horario.ToTimeSpan());
        comando.Parameters.AddWithValue("@ativa", turma.Ativa);
        comando.Parameters.AddWithValue("@treinadorId", turma.TreinadorId);

        comando.ExecuteNonQuery();
    }

    public void Atualizar(Turma turma)
    {
        var comando = _conexao.CreateCommand(
            "UPDATE Turma SET nome_tur = @nome, categoria_tur = @categoria, dias_de_treino_tur = @diasDeTreino, horario_tur = @horario, ativa_tur = @ativa, id_trei_fk = @treinadorId WHERE id_tur = @id;"
        );

        comando.Parameters.AddWithValue("@nome", turma.Nome);
        comando.Parameters.AddWithValue("@categoria", turma.Categoria);
        comando.Parameters.AddWithValue("@diasDeTreino", turma.DiasDeTreino);
        comando.Parameters.AddWithValue("@horario", turma.Horario.ToTimeSpan());
        comando.Parameters.AddWithValue("@ativa", turma.Ativa);
        comando.Parameters.AddWithValue("@treinadorId", turma.TreinadorId);
        comando.Parameters.AddWithValue("@id", turma.Id);

        comando.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        var comando = _conexao.CreateCommand(
            "DELETE FROM Turma WHERE id_tur = @id;"
        );

        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }

    private static Turma Mapear(MySqlDataReader leitor)
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