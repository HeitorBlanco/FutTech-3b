using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO
{
    public class FinanceiroResumosDAO
    {
        private readonly Conexao _conexao;

        public FinanceiroResumosDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // READ — lista todos os registros financeiros
        public List<FinanceiroResumo> Listar()
        {
            var lista = new List<FinanceiroResumo>();

            var comando = _conexao.CreateCommand(
                "SELECT * FROM Financeiro;"
            );

            var leitor = (MySqlDataReader)comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearFinanceiroResumo(leitor));
            }

            return lista;
        }

        // Método auxiliar: converte a linha atual do leitor em um objeto FinanceiroResumo
        private static FinanceiroResumo MapearFinanceiroResumo(MySqlDataReader leitor)
        {
            return new FinanceiroResumo
            {
                RecebidoMesAtual = leitor.GetDecimal("recebido_mes_atual_fin"),
                Pendentes = leitor.GetInt32("pendentes_fin"),
                TotalAReceber = leitor.GetDecimal("total_a_receber_fin"),
                TotalPago = leitor.GetDecimal("total_pago_fin")
            };
        }

        // CREATE — cadastra um novo registro financeiro
        public void Inserir(FinanceiroResumo financeiro)
        {
            try
            {
                using var con = _conexao.GetConnection();

                string sql = @"
                    INSERT INTO Financeiro
                    (
                        recebido_mes_atual_fin,
                        pendentes_fin,
                        total_a_receber_fin,
                        total_pago_fin
                    )
                    VALUES
                    (
                        @recebido,
                        @pendentes,
                        @totalReceber,
                        @totalPago
                    )";

                using var comando = con.CreateCommand();

                comando.CommandText = sql;

                comando.Parameters.AddWithValue(
                    "@recebido",
                    financeiro.RecebidoMesAtual
                );

                comando.Parameters.AddWithValue(
                    "@pendentes",
                    financeiro.Pendentes
                );

                comando.Parameters.AddWithValue(
                    "@totalReceber",
                    financeiro.TotalAReceber
                );

                comando.Parameters.AddWithValue(
                    "@totalPago",
                    financeiro.TotalPago
                );

                comando.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
        }
    }
}