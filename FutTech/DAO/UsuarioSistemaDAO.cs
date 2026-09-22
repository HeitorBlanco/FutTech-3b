using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class UsuariosSistemaDAO
{
    private readonly Conexao _conexao;

    public UsuariosSistemaDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<UsuarioSistema> Listar()
    {
        var lista = new List<UsuarioSistema>();

        using var comando = _conexao.CreateCommand(
            "SELECT * FROM UsuarioSistema;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearUsuarioSistema(leitor));
        }

        return lista;
    }

    private static UsuarioSistema MapearUsuarioSistema(MySqlDataReader leitor)
    {
        return new UsuarioSistema
        {
            Id = leitor.GetInt32("id_usu"),
            Nome = leitor.GetString("nome_usu"),
            Email = leitor.GetString("email_usu"),
            PerfilDescricao = leitor.GetString("perfil_usu"),
            Cargo = leitor.GetString("cargo_usu"),
            Ativo = leitor.GetBoolean("ativo_usu")
        };
    }
}