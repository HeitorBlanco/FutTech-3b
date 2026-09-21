using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class EscolinhasDAO
{
    private readonly Conexao _conexao;

    public EscolinhasDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<Escolinha> Listar()
    {
        var lista = new List<Escolinha>();

        using var comando = _conexao.CreateCommand(
            "SELECT * FROM Escolinha;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearEscolinha(leitor));
        }

        return lista;
    }

    private static Escolinha MapearEscolinha(MySqlDataReader leitor)
    {
        return new Escolinha
        {
            Id = leitor.GetInt32("id_esc"),

            Nome = leitor.GetString("nome_esc"),

            Cnpj = leitor.GetString("cnpj_esc"),

            Ativo = leitor.GetBoolean("ativo_esc")
        };
    }
}