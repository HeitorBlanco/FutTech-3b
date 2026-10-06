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
            @"SELECT 
                id_trei,
                nome_trei,
                cargo_trei,
                ativo_trei
              FROM Treinador
              ORDER BY nome_trei;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(new Treinador
            {
                Id = leitor.GetInt32("id_trei"),
                Nome = leitor.GetString("nome_trei"),
                Cargo = leitor.GetString("cargo_trei"),
                Ativo = leitor.GetBoolean("ativo_trei")
            });
        }

        return lista;
    }

    public void Inserir(Treinador treinador)
    {
        using var comando = _conexao.CreateCommand(
            @"INSERT INTO Treinador
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
            );"
        );

        comando.Parameters.AddWithValue(
            "@nome",
            treinador.Nome
        );

        comando.Parameters.AddWithValue(
            "@cargo",
            treinador.Cargo
        );

        comando.Parameters.AddWithValue(
            "@ativo",
            treinador.Ativo
        );

        comando.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        using var comando = _conexao.CreateCommand(
            @"DELETE FROM Treinador
              WHERE id_trei = @id;"
        );

        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }
}