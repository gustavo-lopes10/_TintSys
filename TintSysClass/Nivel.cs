using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace TintSysClass1
{
    public class Nivel //Abstração
    {
        // Atributos
        // private (-) dentro da classe
        // protected (#)
        // public (+)
        private int id;
        private string nome;
        private string sigla;

        // Propriedades (Encapsulamento, pode-se usar get e set) impotante para permisões
        // public int Id { get { return id; } set { if (System.Environment.UserDomainName == "Senac") { id = value; } } }
        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Sigla { get => sigla; set => sigla = value; }

        // Médotos construtores}

        //métodos construtores 
        public Nivel() { }  //vazio
        public Nivel(string _nome, string _sigla)
        {
            Nome = _nome;
            Sigla = _sigla;
        }

        public Nivel(int _id, string _nome, string _sigla)
        {
            Id = _id;
            Nome = _nome;
            Sigla = _sigla;
        }


        // Médotos da Classe(inserir, alterar, consultar, porId, por nome, etc...)
        public void Inserir()
        {
            // cria uma variável com conexão de banco aberta
            var cmd = Banco.Abrir();
            // define o tipo de comando/intrução MySQL a ser processada pelo server banco de dados
            cmd.CommandType = CommandType.Text;
            // define a query sql especificada com parâmetros (exemplo: @nome...)
            cmd.CommandText = "insert niveis (nome, sigla) values (@nome, @sigla)";
            // cria o parâmetro e associa ao valor
            cmd.Parameters.Add("@nome", MySqlDbType.VarChar).Value = Nome;
            cmd.Parameters.AddWithValue("@sigla", Sigla);
            // executa a instrução SQL na conexão aberta
            cmd.ExecuteNonQuery();
            // obtendo o id do nível recém inserido
            cmd.CommandText = "select @@identity";
            // recupera o id na propriedade
            Id = Convert.ToInt32(cmd.ExecuteScalar());
            // fecha a conexão 
            Banco.Fechar(cmd);
        }
        public static Nivel ObterPorId(int _id)
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from niveis where id = @id";
            cmd.Parameters.AddWithValue("@id", _id);
            var dr = cmd.ExecuteReader();
            Nivel nivel = null;
            while(dr.Read())
            {
                nivel = new Nivel(
                    dr.GetInt32(0),
                    dr.GetString(1),
                    dr.GetString(2)
                    );
            }
            Banco.Fechar(cmd);
            return nivel;
        }
        public static List<Nivel> Listar()
        {
            List<Nivel> lista = new List<Nivel>();
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from niveis";
            var dr = cmd.ExecuteReader();
            while(dr.Read())
            {
                lista.Add(new Nivel(dr.GetInt32(0), dr.GetString(1), dr.GetString(2)));
            }
            Banco.Fechar(cmd);
            return lista;
        }

        public void Atualizar()
        {
            var cmd = Banco.Abrir();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update niveis set nome = @nome, sigla = @sigla where id = @id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nome",nome);
            cmd.Parameters.AddWithValue("@sigla",sigla);
            cmd.ExecuteNonQuery();
            Banco.Fechar(cmd);
        }
        public bool Excluir(int _id)
        {
            bool confirma = false;
            var cmd = Banco.Abrir();
            cmd.CommandText = "delete from niveis where id ="+_id;
            try
            {
                if (cmd.ExecuteNonQuery() > 0)
                {
                    confirma = true;
                }

            }
            catch (Exception e)
            {
                
            }


            Banco.Fechar(cmd);
            return confirma;
        }

    }
}
