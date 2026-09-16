using FutTech.Configs;
using FutTech.Models;
using FutTech.Configs;
using MySql.Data.MySqlClient;

namespace FutTech.DAO
{
    public class AlunoDAO
    {
        private readonly Conexao _conexao;

        public AlunoDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // READ — lista todos os alunos
        public List<Aluno> Listar()
        {
            var lista = new List<Aluno>();
            var comando = _conexao.CreateCommand("SELECT * FROM Aluno;");

            var leitor = (MySqlDataReader)comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearAluno(leitor));
            }

            return lista;
        }

        // Método auxiliar: converte a linha atual do leitor em um objeto Aluno
        private static Aluno MapearAluno(MySqlDataReader leitor)
        {
            return new Aluno
            {
            };
        }
    }
}