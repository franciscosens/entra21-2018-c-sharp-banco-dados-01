using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExemploBancoDados01
{
    public partial class Form1 : Form
    {
        private string caminhoConexao = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\Users\fsens\Documents\ExemploBancoDados01.mdf;Integrated Security=True;Connect Timeout=30";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(caminhoConexao);
            sqlConnection.Open();
            SqlCommand command = new SqlCommand();
            command.Connection = sqlConnection;
            string corDigitada = txtCor.Text;
            command.CommandText = "INSERT INTO cores VALUES (@NOME)";

            command.Parameters.AddWithValue("@NOME", corDigitada);
            command.ExecuteNonQuery();
            sqlConnection.Close();

        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            SqlConnection conexao = new SqlConnection();
            conexao.ConnectionString = caminhoConexao;
            conexao.Open();
            SqlCommand comando = new SqlCommand("SELECT nome FROM cores;");
            comando.Connection = conexao;

            // Cria em memória uma tabela 
            DataTable dataTable = new DataTable();
            // ExecuteReader - Executa o comando e retorna um SQLDataReader
            // Load - define para a tabela em memória as colunas 
            // e registros(retornados do select)
            // para poder trabalhar com elas depois.
            dataTable.Load(comando.ExecuteReader());

            StringBuilder sb = new StringBuilder();
            // Percorre os registros da tabela em memória.
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                // Obtém o conteúdo de uma linha.
                // Da primeira coluna do sql
                string cor = dataTable.Rows[i][0].ToString();
                sb.AppendLine(cor);
            }
            richTextBox1.Clear();
            richTextBox1.AppendText(sb.ToString());
            conexao.Close();
        }

        private void btnApagar_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(caminhoConexao);
            connection.Open();

            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM cores WHERE nome = @NOMEDACOR";
            string corParaApagar = cbCorApagar.SelectedItem.ToString();
            command.Parameters.AddWithValue("@NOMEDACOR", corParaApagar);
            command.ExecuteNonQuery();
            cbCorApagar.SelectedIndex = - 1;
            connection.Close();
        }

        private void cbCorApagar_DropDown(object sender, EventArgs e)
        {
            SqlConnection conexao = new SqlConnection(caminhoConexao);
            conexao.Open();

            SqlCommand command = new SqlCommand("SELECT nome FROM cores ORDER BY nome", conexao);
            DataTable table = new DataTable();
            table.Load(command.ExecuteReader());
            cbCorApagar.Items.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                cbCorApagar.Items.Add(table.Rows[i][0].ToString());
            }
            conexao.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void cbCorApagar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbCorApagar.SelectedIndex != -1)
            {
                txtNovoNome.Text = cbCorApagar.SelectedItem.ToString();
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            SqlConnection conexao = new SqlConnection(caminhoConexao);
            conexao.Open();
            SqlCommand comando = new SqlCommand();
            comando.Connection = conexao;
            comando.CommandText = "UPDATE cores SET nome = @NOVONOME WHERE nome = @ANTIGONOME";

            string novoNome = txtNovoNome.Text;
            string antigoNome = cbCorApagar.SelectedItem.ToString();

            comando.Parameters.AddWithValue("@NOVONOME", novoNome);
            comando.Parameters.AddWithValue("@ANTIGONOME", antigoNome);
            comando.ExecuteNonQuery();

            cbCorApagar.SelectedIndex = -1;
            txtNovoNome.Clear();
            conexao.Close();
        }
    }
}
