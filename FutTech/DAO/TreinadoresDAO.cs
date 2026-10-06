using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class TreinadoresDAO
{
    private readonly Conexao _conexao;

    public TreinadoresDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<Treinador> Listar()
    {
        var lista = new List<Treinador>();

        using var comando = _conexao.CreateCommand(
            "SELECT id_trei, nome_trei, cargo_trei, ativo_trei FROM Treinador"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            var treinador = new Treinador();

            treinador.Id = leitor.GetInt32("id_trei");
            treinador.Nome = leitor.GetString("nome_trei");
            treinador.Cargo = leitor.GetString("cargo_trei");
            treinador.Ativo = leitor.GetBoolean("ativo_trei");

            lista.Add(treinador);
        }

        return lista;
    }

    public void Inserir(Treinador treinador)
    {
        using var conexao = _conexao.GetConnection();

        string sql = @"
            INSERT INTO Treinador
            (
                nome_trei,
                cargo_trei,
                ativo_trei
            )
            VALUES
            (
                @nome,
                @cargo,
                @ativo
            )";

        using var comando = conexao.CreateCommand();

        comando.CommandText = sql;

        comando.Parameters.AddWithValue("@nome", treinador.Nome);
        comando.Parameters.AddWithValue("@cargo", treinador.Cargo);
        comando.Parameters.AddWithValue("@ativo", treinador.Ativo);

        comando.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var conexao = _conexao.GetConnection();

        string sql = @"
            DELETE FROM Treinador
            WHERE id_trei = @id";

        using var comando = conexao.CreateCommand();

        comando.CommandText = sql;

        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }
}