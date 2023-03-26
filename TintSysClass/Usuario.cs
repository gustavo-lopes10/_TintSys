using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TintSysClass1
{
    public class Usuario
    {
        private int id;
        private string nome;
        private string email;
        private string senha;
        private Nivel nivel;
        private bool ativo;

        // propriedades
        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Email { get => email; set => email = value; }
        public string Senha { get => senha; set => senha = value; }
        public Nivel Nivel { get => nivel; set => nivel = value; }
        public bool Ativo { get => ativo; set => ativo = value; }

        public Usuario() 
        {
            ativo = false;
            nivel = Nivel.ObterPorId(2);
        }
        
        public Usuario(int id, string nome, string email, string senha, Nivel nivel, bool ativo)
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Nivel = nivel;
            Ativo = ativo;
        }
        public Usuario(int id, string nome, string email, string senha, Nivel nivel) 
        {
            Id = id;
            Nome = nome;
            Email = email;
            Senha = senha;
            Nivel = nivel;
        }

        public Usuario(string nome, string email, string senha, Nivel nivel, bool ativo)
        {
            Nome = nome;
            Email = email;
            Senha = senha;
            Nivel = nivel;
            Ativo = ativo;
        }

        public static Usuario EfetuarLogin(string _email, string _senha)
        {
            Usuario usuario = null;
            MySqlCommand cmd = Banco.Abrir(); // var é usado quando você não quer declarar o tipo
            cmd.CommandText = "select id, nome, nivel from usuarios" +
                "where email = @email and senha = md5(@senha) and ativo = 1";
            cmd.Parameters.AddWithValue("@email", _email);
            cmd.Parameters.AddWithValue("@senha", _senha);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                usuario = new Usuario();
                usuario.Id = dr.GetInt32(0);
                usuario.Nome = dr.GetString(1);
                usuario.Email = dr.GetString(2);
                usuario.Nivel = Nivel.ObterPorId(dr.GetInt32(3));
            }
            return usuario;
        }
        public void Inserir() 
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "insert into usuarios (nome, email, senha, nivel_id, ativo) " +
                "values(@nome, @email, md5(@senha), @nivel, 1";
            cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = Nome;
            cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;
            cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = Senha;
            cmd.Parameters.Add("@nivel", MySqlDbType.Int32).Value = Nivel.Id;
            cmd.ExecuteNonQuery();
            cmd.CommandText = "select @@identity";
            Id = Convert.ToInt32(cmd.ExecuteScalar());
            Banco.Fechar(cmd);
        }
        public static Usuario ObterPorId(int _id)
        {
            Usuario usuario = null;
            var cmd = Banco.Abrir();
            cmd.CommandText = "select * from usuarios where id = "+_id;
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                usuario = new Usuario(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetString(2),
                        dr.GetString(3),
                        Nivel.ObterPorId(dr.GetInt32(4)),
                        dr.GetBoolean(5)
                    );
            }
            Banco.Fechar(cmd);
            return usuario;
        }

        
        public static List<Usuario> Listar(string _nome = "")
        {
            List<Usuario> lista = new List<Usuario>();
            var cmd = Banco.Abrir();
            if (_nome!=string.Empty)
            {
                cmd.CommandText = "select * from usuarios where nome like '%' = " + _nome + "'%'";
            }
            else
            {
                cmd.CommandText = "select * from usuarios";
            }
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Usuario(
                        dr.GetInt32(0),
                        dr.GetString(1),
                        dr.GetString(2),
                        dr.GetString(3),
                        Nivel.ObterPorId(dr.GetInt32(4)),
                        dr.GetBoolean(5)
                    )
                );
            }
            Banco.Fechar(cmd);
            return lista;
        }
        public void Atualizar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "update usuarios set nome = @nome, senha = md5(@senha)," +
                " nivel_id = @nivel where id = "+Id;
            cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = Nome;
            cmd.Parameters.Add("@senha", MySqlDbType.VarChar).Value = Senha;
            cmd.Parameters.Add("@nivel", MySqlDbType.Int32).Value = Nivel.Id;
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
        public void Arquivar(int _id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "update usuarios set ativo = 0 where id = "+_id;
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
        public static void Restaurar(int _id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "update usuarios set ativo = 1 where id = " + _id;
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
        public void Excluir(int _id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "delete from usuarios where id = " + _id;
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
    }
}
