
using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace FutTech.DAO;

public class ComunicadosDAO
{
    private readonly Conexao _conexao;

    public ComunicadosDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<Comunicado> Listar()
    {
        var lista = new List<Comunicado>();

        using var comando = _conexao.CreateCommand(
            "SELECT * FROM Comunicado;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearComunicado(leitor));
        }

        return lista;
    }

    private static Comunicado MapearComunicado(MySqlDataReader leitor)
    {
        return new Comunicado
        {
            Id = leitor.GetInt32("id_com"),

            Titulo = leitor.GetString("titulo_com"),

            Conteudo = leitor.GetString("conteudo_com"),

            PublicadoEm = DateOnly.FromDateTime(
                leitor.GetDateTime("publicado_em_com")
            ),

            PublicadoAs = TimeOnly.FromTimeSpan(
                leitor.GetTimeSpan("publicado_as_com")
            ),

            Autor = leitor.GetString("autor_com"),

            Categoria = leitor.GetString("categoria_com"),

            Destacado = leitor.GetBoolean("destacado_com"),

            Ativo = leitor.GetBoolean("ativo_com")
        };
    }
}
