using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TintSysClass1
{
    public class Cliente
    {
        // Criando as variáveis
        private int id;
        private string nome;
        private string cpf;
        private string email;
        private DateTime datacad;
        private int ativo;

        // criando as propriedades
        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Email { get => email; set => email = value; }
        public DateTime Datacad { get => datacad; set => datacad = value; }
        public int Ativo { get => ativo; set => ativo = value; }

        // criando os métodos construtores
        /// <summary>
        /// Método vazio
        /// </summary>
        public Cliente() { } //vazio

        /// <summary>
        /// Método com todas as informações
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <param name="nome">nome do cliente</param>
        /// <param name="cpf">cpf do cliente</param>
        /// <param name="email">email do cliente</param>
        /// <param name="datacad">data do cadastro</param>
        /// <param name="ativo">se o cliente está ativo</param>
        public Cliente(int id, string nome, string cpf, string email, DateTime datacad, int ativo)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Email = email;
            Datacad = datacad;
            Ativo = ativo;
        }
        /// <summary>
        /// Método sem id e ativo
        /// </summary>
        /// <param name="nome">nome do cliente</param>
        /// <param name="cpf">cpf do cliente</param>
        /// <param name="email">email do cliente</param>
        /// <param name="datacad">data do cadastro</param>
        public Cliente(string nome, string cpf, string email, DateTime datacad)
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            Datacad = datacad;
        }
        /// <summary>
        /// Método sem id
        /// </summary>
        /// <param name="nome">nome do cliente</param>
        /// <param name="cpf">cpf do cliente</param>
        /// <param name="email">email do cliente</param>
        /// <param name="datacad">data do cadastro</param>
        /// <param name="ativo">se o cliente está ativo</param>

        public Cliente(string nome, string cpf, string email, DateTime datacad, int ativo)
        {
            Nome = nome;
            Cpf = cpf;
            Email = email;
            Datacad = datacad;
            Ativo = ativo;
        }


        // Criando os Métodos da Classe



        /// <summary>
        /// Inserir o cliente
        /// </summary>
        public void Inserir()
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "insert into clientes (nome, cpf, email, datacad, ativo) " +
                "values(@nome, @cpf, @email, @datacad, @ativo";
            cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = Nome;
            cmd.Parameters.Add("@cpf", MySqlDbType.VarChar).Value = Cpf;
            cmd.Parameters.Add("@email", MySqlDbType.VarChar).Value = Email;
            cmd.Parameters.Add("@datacad", MySqlDbType.DateTime).Value = Datacad;
            cmd.Parameters.AddWithValue("@ativo", Ativo);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "select @@identity";
            Id = Convert.ToInt32(cmd.ExecuteScalar());
            Banco.Fechar(cmd);
        }
        /// <summary>
        /// Obter o Cliente Pelo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Retorna todas as informações do cliente com o id solicitado</returns>

        public static Cliente ObterPorId(int id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "select * from clientes where id = " + id;
            var dr = cmd.ExecuteReader();
            Cliente cliente = null;
            while (dr.Read())
            {
                cliente = new Cliente(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetString(2),
                    dr.GetString(3),
                    dr.GetDateTime(4),
                    dr.GetInt32(5)
                    );
            }
            Banco.Fechar(cmd);
            return cliente;
        }

        
        /// <summary>
        /// Listar o Cliente pelo nome
        /// </summary>
        /// <param name="_nome"></param>
        /// <returns>Retorna o cliente de acordo com o nome inserido</returns>
        public static List<Cliente> Listar(string _nome = "")
        {
            List<Cliente> lista = new List<Cliente>();
            var cmd = Banco.Abrir();
            if (_nome != string.Empty)
            {
                cmd.CommandText = "select * from clientes where nome like '%' = " + _nome + "'%'";
            }
            else
            {
                cmd.CommandText = "select * from clientes";
            }
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Cliente(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetString(2),
                    dr.GetString(3),
                    dr.GetDateTime(4),
                    dr.GetInt32(5)
                    )
                );
            }
            Banco.Fechar(cmd);
            return lista;
        }
        /// <summary>
        /// Atualizar o cliente
        /// </summary>
        public void Atualizar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "update clientes set nome = @nome" +
                "where id = " + Id;
            cmd.Parameters.AddWithValue("nome", Nome);
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
        /// <summary>
        /// Arquivar o cliente 
        /// </summary>
        /// <param name="_id"></param>
        public void Arquivar(int _id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "update clientes set ativo = 0 where id = " + _id;
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
        /// <summary>
        /// Restaura o cliente
        /// </summary>
        /// <param name="_id"></param>
        public static void Restaurar(int _id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "update clientes set ativo = 1 where id = " + _id;
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
        /// <summary>
        /// Exclui o cliente
        /// </summary>
        /// <param name="_id"></param>
        public void Excluir(int _id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandText = "delete from clientes where id = " + _id;
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
    }
}
