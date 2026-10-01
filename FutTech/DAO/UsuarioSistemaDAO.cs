using FutTech.Configs;
using FutTech.Models;
using MySql.Data.MySqlClient;

namespace FutTech.DAO
{
    public class UsuariosSistemaDAO
    {
        private readonly Conexao _conexao;

        public UsuariosSistemaDAO(Conexao conexao)
        {
            _conexao = conexao;
        }

        // READ — lista todos os usuários do sistema
        public List<UsuarioSistema> Listar()
        {
            var lista = new List<UsuarioSistema>();

            var comando = _conexao.CreateCommand(
                "SELECT * FROM UsuarioSistema;"
            );

            var leitor = (MySqlDataReader)comando.ExecuteReader();

            while (leitor.Read())
            {
                lista.Add(MapearUsuarioSistema(leitor));
            }

            return lista;
        }

        // Método auxiliar: converte a linha atual do leitor em um objeto UsuarioSistema
        private static UsuarioSistema MapearUsuarioSistema(MySqlDataReader leitor)
        {
            return new UsuarioSistema
            {
                Id = leitor.GetInt32("id_usu"),

                Nome = leitor.GetString("nome_usu"),

                Email = leitor.GetString("email_usu"),

                Perfil = Enum.Parse<PerfilUsuario>(
                    leitor.GetString("perfil_usu")
                ),

                Cargo = leitor.GetString("cargo_usu"),

                Ativo = leitor.GetBoolean("ativo_usu"),

                PerfilDescricao = leitor.GetString(
                    "perfil_descricao_usu"
                ),

                RotaInicial = leitor.GetString(
                    "rota_inicial_usu"
                )
            };
        }

        // CREATE — cadastra um novo usuário do sistema
        public void Inserir(UsuarioSistema usuario)
        {
            try
            {
                using var con = _conexao.GetConnection();

                string sql = @"
                    INSERT INTO UsuarioSistema
                    (
                        nome_usu,
                        email_usu,
                        perfil_usu,
                        cargo_usu,
                        ativo_usu,
                        perfil_descricao_usu,
                        rota_inicial_usu
                    )
                    VALUES
                    (
                        @nome,
                        @email,
                        @perfil,
                        @cargo,
                        @ativo,
                        @perfilDescricao,
                        @rotaInicial
                    )";

                using var comando = con.CreateCommand();

                comando.CommandText = sql;

                comando.Parameters.AddWithValue(
                    "@nome",
                    usuario.Nome
                );

                comando.Parameters.AddWithValue(
                    "@email",
                    usuario.Email
                );

                comando.Parameters.AddWithValue(
                    "@perfil",
                    usuario.Perfil.ToString()
                );

                comando.Parameters.AddWithValue(
                    "@cargo",
                    usuario.Cargo
                );

                comando.Parameters.AddWithValue(
                    "@ativo",
                    usuario.Ativo
                );

                comando.Parameters.AddWithValue(
                    "@perfilDescricao",
                    usuario.PerfilDescricao
                );

                comando.Parameters.AddWithValue(
                    "@rotaInicial",
                    usuario.RotaInicial
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