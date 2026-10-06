
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
        // CREATE — cadastra um novo comunicado
public void Inserir(Comunicado comunicado)
    {
        try
        {
            using var con = _conexao.GetConnection();

            string sql = @"
            INSERT INTO Comunicado
            (
                titulo_com,
                conteudo_com,
                publicado_em_com,
                publicado_as_com,
                autor_com,
                categoria_com,
                destacado_com,
                ativo_com
            )
            VALUES
            (
                @titulo,
                @conteudo,
                @data,
                @hora,
                @autor,
                @categoria,
                @destacado,
                @ativo
            )";

            using var comando = con.CreateCommand();

            comando.CommandText = sql;

            comando.Parameters.AddWithValue(
                "@titulo",
                comunicado.Titulo
            );

            comando.Parameters.AddWithValue(
                "@conteudo",
                comunicado.Conteudo
            );

            comando.Parameters.AddWithValue(
                "@data",
                comunicado.PublicadoEm
            );

            comando.Parameters.AddWithValue(
                "@hora",
                comunicado.PublicadoAs
            );

            comando.Parameters.AddWithValue(
                "@autor",
                comunicado.Autor
            );

            comando.Parameters.AddWithValue(
                "@categoria",
                comunicado.Categoria
            );

            comando.Parameters.AddWithValue(
                "@destacado",
                comunicado.Destacado
            );

            comando.Parameters.AddWithValue(
                "@ativo",
                comunicado.Ativo
            );

            comando.ExecuteNonQuery();
        }
        catch
        {
            throw;
        }
    }
}

