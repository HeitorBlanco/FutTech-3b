using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class AvaliacaoAlunoDAO
{
    private readonly Conexao _conexao;

    public AvaliacaoAlunoDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<AvaliacaoAluno> Listar()
    {
        var lista = new List<AvaliacaoAluno>();

        using var comando = _conexao.CreateCommand(
            "SELECT * FROM AvaliacaoAluno;"
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearAvaliacaoAluno(leitor));
        }

        return lista;
    }

    private static AvaliacaoAluno MapearAvaliacaoAluno(MySqlDataReader leitor)
    {
        return new AvaliacaoAluno
        {
            Id = leitor.GetInt32("id_ava"),
            AlunoId = leitor.GetInt32("id_alu_fk"),
            TurmaId = leitor.GetInt32("id_tur_fk"),
            TreinadorId = leitor.GetInt32("id_trei_fk"),
            Data = DateOnly.FromDateTime(leitor.GetDateTime("data_ava")),
            NotaTecnica = leitor.GetInt32("nota_tecnica_ava"),
            NotaFisica = leitor.GetInt32("nota_fisica_ava"),
            NotaTatica = leitor.GetInt32("nota_tatica_ava"),
            NotaComportamental = leitor.GetInt32("nota_comportamental_ava"),
            Observacoes = leitor.GetString("observacoes_ava"),
            Media = leitor.GetDecimal("media_ava")
        };
    }
}