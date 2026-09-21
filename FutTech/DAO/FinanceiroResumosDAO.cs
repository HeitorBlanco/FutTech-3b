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

        public List<FinanceiroResumo> Listar()
        {
            var lista = new List<FinanceiroResumo>();

            using var comando = _conexao.CreateCommand(
                "SELECT * FROM Financeiro;"
            );

            using var leitor = (MySqlDataReader)comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearFinanceiroResumo(leitor));
            }

            return lista;
        }

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
    }
}