using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO;

public class MensalidadeDAO
{
    private readonly Conexao _conexao;

    public MensalidadeDAO(Conexao conexao)
    {
        _conexao = conexao;
    }
    public void Inserir(Mensalidade mensalidade)
    {
        const string sql = """
            INSERT INTO Mensalidade
            (
                id_alu_fk,
                competencia_men,
                valor_men,
                vencimento_men,
                data_pagamento_men,
                status_men
            )
            VALUES
            (
                @aluno,
                @competencia,
                @valor,
                @vencimento,
                @dataPagamento,
                @status
            );
            """;

        using var comando =
            _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue(
            "@aluno",
            mensalidade.AlunoId
        );

        comando.Parameters.AddWithValue(
            "@competencia",
            mensalidade.Competencia.ToDateTime(
                TimeOnly.MinValue
            )
        );

        comando.Parameters.AddWithValue(
            "@valor",
            mensalidade.Valor
        );

        comando.Parameters.AddWithValue(
            "@vencimento",
            mensalidade.Vencimento.ToDateTime(
                TimeOnly.MinValue
            )
        );

        comando.Parameters.AddWithValue(
            "@dataPagamento",
            mensalidade.DataPagamento.HasValue
                ? mensalidade.DataPagamento.Value.ToDateTime(
                    TimeOnly.MinValue
                )
                : DBNull.Value
        );

        comando.Parameters.AddWithValue(
            "@status",
            mensalidade.Status.ToString()
        );

        comando.ExecuteNonQuery();
    }
    public List<MensalidadeDetalhe> Listar()
    {
        var lista = new List<MensalidadeDetalhe>();

        const string sql = """
            SELECT
                m.id_men,
                m.id_alu_fk,
                m.competencia_men,
                m.valor_men,
                m.vencimento_men,
                m.data_pagamento_men,
                CASE
                    WHEN m.status_men <> 'Pago' AND m.vencimento_men < CURDATE()
                        THEN 'Atrasada'
                    ELSE m.status_men
                END AS status_men,
                a.nome_alu,
                t.nome_tur
            FROM Mensalidade AS m
            INNER JOIN Aluno AS a ON a.id_alu = m.id_alu_fk
            LEFT JOIN Turma AS t ON t.id_tur = a.id_tur_fk
            ORDER BY m.vencimento_men DESC, a.nome_alu;
            """;

        using var comando = _conexao.CreateCommand(sql);
        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        while (leitor.Read())
        {
            lista.Add(MapearDetalhe(leitor));
        }

        return lista;
    }

    public FinanceiroResumo ObterResumo(DateOnly competencia)
    {
        const string sql = """
            SELECT
                COALESCE(SUM(
                    CASE
                        WHEN status_men = 'Pago'
                         AND data_pagamento_men >= @inicio
                         AND data_pagamento_men < DATE_ADD(@inicio, INTERVAL 1 MONTH)
                        THEN valor_men
                        ELSE 0
                    END
                ), 0) AS recebido_mes,

                COALESCE(SUM(
                    CASE
                        WHEN status_men <> 'Pago' THEN 1
                        ELSE 0
                    END
                ), 0) AS pendentes,

                COALESCE(SUM(
                    CASE
                        WHEN status_men <> 'Pago' THEN valor_men
                        ELSE 0
                    END
                ), 0) AS total_a_receber,

                COALESCE(SUM(
                    CASE
                        WHEN status_men = 'Pago' THEN valor_men
                        ELSE 0
                    END
                ), 0) AS total_pago

            FROM Mensalidade;
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue(
            "@inicio",
            competencia.ToDateTime(TimeOnly.MinValue)
        );

        using var leitor = (MySqlDataReader)comando.ExecuteReader();

        if (!leitor.Read())
        {
            return new FinanceiroResumo();
        }

        return new FinanceiroResumo
        {
            RecebidoMesAtual = leitor.GetDecimal("recebido_mes"),
            Pendentes = Convert.ToInt32(leitor.GetDecimal("pendentes")),
            TotalAReceber = leitor.GetDecimal("total_a_receber"),
            TotalPago = leitor.GetDecimal("total_pago")
        };
    }

    public int GerarMensalidades(DateOnly competencia)
    {
        const string sql = """
            INSERT INTO Mensalidade
            (
                id_alu_fk,
                competencia_men,
                valor_men,
                vencimento_men,
                data_pagamento_men,
                status_men
            )
            SELECT
                a.id_alu,
                @competencia,

                COALESCE(
                    (
                        SELECT m2.valor_men
                        FROM Mensalidade AS m2
                        WHERE m2.id_alu_fk = a.id_alu
                        ORDER BY m2.competencia_men DESC
                        LIMIT 1
                    ),
                    150.00
                ),

                DATE_ADD(@competencia, INTERVAL 9 DAY),

                NULL,

                'Pendente'

            FROM Aluno AS a

            WHERE a.ativo_alu = TRUE

              AND NOT EXISTS
              (
                  SELECT 1
                  FROM Mensalidade AS m3
                  WHERE m3.id_alu_fk = a.id_alu
                    AND YEAR(m3.competencia_men) = YEAR(@competencia)
                    AND MONTH(m3.competencia_men) = MONTH(@competencia)
              );
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue(
            "@competencia",
            competencia.ToDateTime(TimeOnly.MinValue)
        );

        return comando.ExecuteNonQuery();
    }

    public void RegistrarPagamento(int id)
    {
        const string sql = """
            UPDATE Mensalidade
            SET
                data_pagamento_men = CURDATE(),
                status_men = 'Pago'
            WHERE id_men = @id;
            """;

        using var comando = _conexao.CreateCommand(sql);

        comando.Parameters.AddWithValue("@id", id);

        comando.ExecuteNonQuery();
    }

    private static MensalidadeDetalhe MapearDetalhe(
        MySqlDataReader leitor)
    {
        var dataPagamento =
            DAOHelper.GetDateTime(
                leitor,
                "data_pagamento_men"
            );

        return new MensalidadeDetalhe
        {
            Mensalidade = new Mensalidade
            {
                Id = leitor.GetInt32("id_men"),

                AlunoId =
                    leitor.GetInt32("id_alu_fk"),

                Competencia =
                    DateOnly.FromDateTime(
                        leitor.GetDateTime("competencia_men")
                    ),

                Valor =
                    leitor.GetDecimal("valor_men"),

                Vencimento =
                    DateOnly.FromDateTime(
                        leitor.GetDateTime("vencimento_men")
                    ),

                DataPagamento =
                    dataPagamento.HasValue
                        ? DateOnly.FromDateTime(dataPagamento.Value)
                        : null,

                Status =
                    ConverterStatus(
                        leitor.GetString("status_men")
                    )
            },

            NomeAluno =
                DAOHelper.GetString(
                    leitor,
                    "nome_alu"
                ),

            NomeTurma =
                DAOHelper.GetString(
                    leitor,
                    "nome_tur"
                )
        };
    }

    private static StatusMensalidade ConverterStatus(
        string status)
    {
        return status switch
        {
            "Pago" => StatusMensalidade.Pago,

            "Atrasada" =>
                StatusMensalidade.Atrasada,

            _ =>
                StatusMensalidade.Pendente
        };
    }
}