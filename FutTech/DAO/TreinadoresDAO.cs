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
            "SELECT * FROM Treinador;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearTreinador(leitor));
        }

        return lista;
    }

    private static Treinador MapearTreinador(MySqlDataReader leitor)
    {
        return new Treinador
        {
            Id = leitor.GetInt32("id_trei"),
            Nome = leitor.GetString("nome_trei"),
            Cargo = leitor.GetString("cargo_trei"),
            Ativo = leitor.GetBoolean("ativo_trei")
        };
    }
}