using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class ResponsavelAlunoDAO
{
    private readonly Conexao _conexao;

    public ResponsavelAlunoDAO(Conexao conexao)
    {
        _conexao = conexao;
    }

    public List<ResponsavelAluno> Listar()
    {
        var lista = new List<ResponsavelAluno>();

        var comando = _conexao.CreateCommand(
            "SELECT ra.id_res_alu, ra.id_usu_fk, ra.id_alu_fk, ra.parentesco_res_alu, ra.principal_res_alu, u.nome_usu, a.nome_alu FROM ResponsavelAluno AS ra INNER JOIN UsuarioSistema AS u ON u.id_usu = ra.id_usu_fk INNER JOIN Aluno AS a ON a.id_alu = ra.id_alu_fk ORDER BY a.nome_alu, u.nome_usu;"
        );

        var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        return lista;
    }

    public List<ResponsavelAluno> ListarPorAluno(int alunoId)
    {
        var lista = new List<ResponsavelAluno>();

        var comando = _conexao.CreateCommand(
            "SELECT ra.id_res_alu, ra.id_usu_fk, ra.id_alu_fk, ra.parentesco_res_alu, ra.principal_res_alu, u.nome_usu, a.nome_alu FROM ResponsavelAluno AS ra INNER JOIN UsuarioSistema AS u ON u.id_usu = ra.id_usu_fk INNER JOIN Aluno AS a ON a.id_alu = ra.id_alu_fk WHERE ra.id_alu_fk = @alunoId ORDER BY ra.principal_res_alu DESC, u.nome_usu;"
        );

        comando.Parameters.AddWithValue("@alunoId", alunoId);

        var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        return lista;
    }

    public List<ResponsavelAluno> ListarPorUsuario(int usuarioId)
    {
        var lista = new List<ResponsavelAluno>();

        var comando = _conexao.CreateCommand(
            "SELECT ra.id_res_alu, ra.id_usu_fk, ra.id_alu_fk, ra.parentesco_res_alu, ra.principal_res_alu, u.nome_usu, a.nome_alu FROM ResponsavelAluno AS ra INNER JOIN UsuarioSistema AS u ON u.id_usu = ra.id_usu_fk INNER JOIN Aluno AS a ON a.id_alu = ra.id_alu_fk WHERE ra.id_usu_fk = @usuarioId ORDER BY a.nome_alu;"
        );

        comando.Parameters.AddWithValue("@usuarioId", usuarioId);

        var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(Mapear(leitor));
        }

        return lista;
    }

    public void Salvar(ResponsavelAluno responsavel)
    {
        if (responsavel.Id > 0)
        {
            const string sqlAtualizar = """
                UPDATE ResponsavelAluno
                SET id_usu_fk = @usuarioId,
                    id_alu_fk = @alunoId,
                    parentesco_res_alu = @parentesco,
                    principal_res_alu = @principal
                WHERE id_res_alu = @id;
                """;

            using var comandoAtualizar = _conexao.CreateCommand(sqlAtualizar);

            comandoAtualizar.Parameters.AddWithValue("@usuarioId", responsavel.UsuarioId);
            comandoAtualizar.Parameters.AddWithValue("@alunoId", responsavel.AlunoId);
            comandoAtualizar.Parameters.AddWithValue("@parentesco", responsavel.Parentesco);
            comandoAtualizar.Parameters.AddWithValue("@principal", responsavel.Principal);
            comandoAtualizar.Parameters.AddWithValue("@id", responsavel.Id);

            comandoAtualizar.ExecuteNonQuery();

            return;
        }

        const string sqlInserir = """
            INSERT INTO ResponsavelAluno
            (
                id_usu_fk,
                id_alu_fk,
                parentesco_res_alu,
                principal_res_alu
            )
            VALUES
            (
                @usuarioId,
                @alunoId,
                @parentesco,
                @principal
            );
            """;

        using var comandoInserir = _conexao.CreateCommand(sqlInserir);

        comandoInserir.Parameters.AddWithValue("@usuarioId", responsavel.UsuarioId);
        comandoInserir.Parameters.AddWithValue("@alunoId", responsavel.AlunoId);
        comandoInserir.Parameters.AddWithValue("@parentesco", responsavel.Parentesco);
        comandoInserir.Parameters.AddWithValue("@principal", responsavel.Principal);

        comandoInserir.ExecuteNonQuery();
    }

    public void Excluir(int id)
    {
        const string sql = """
            DELETE FROM ResponsavelAluno
            WHERE id_res_alu = @id;
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }

    private static ResponsavelAluno Mapear(MySqlDataReader leitor)
    {
        return new ResponsavelAluno
        {
            Id = leitor.GetInt32("id_res_alu"),
            UsuarioId = leitor.GetInt32("id_usu_fk"),
            AlunoId = leitor.GetInt32("id_alu_fk"),
            Parentesco = DAOHelper.GetString(leitor, "parentesco_res_alu"),
            Principal = leitor.GetBoolean("principal_res_alu"),
            NomeUsuario = DAOHelper.GetString(leitor, "nome_usu"),
            NomeAluno = DAOHelper.GetString(leitor, "nome_alu")
        };
    }
}