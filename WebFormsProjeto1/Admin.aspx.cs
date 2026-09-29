using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormsProjeto1
{
    public partial class Admin : Page
    {
        // Evento executado na carga da página
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Carrega a lista de pessoas apenas na primeira carga
                CarregarPessoas();
            }
        }

        // Carrega dados da tabela Pessoa aplicando filtro opcional de pesquisa
        private void CarregarPessoas(string pesquisa = "")
        {
            // Lê connection string do Web.config
            string conexao = ConfigurationManager
                .ConnectionStrings["WebFormsDB"]
                .ConnectionString;

            // Normaliza texto de pesquisa
            pesquisa = pesquisa?.Trim() ?? "";

            // Remove pontos e hífen caso o usuário pesquise um CPF formatado.
            string cpfPesquisa = new string(
                pesquisa.Where(char.IsDigit).ToArray()
            );

            int idPesquisa;
            object valorId = DBNull.Value;

            // Se a pesquisa for um número, tenta usar como Id
            if (int.TryParse(pesquisa, out idPesquisa))
            {
                valorId = idPesquisa;
            }

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                // Consulta parametrizada para evitar SQL Injection
                string sql = @"
                    SELECT Id, Nome, CPF, Email, Telefone
                    FROM Pessoa
                    WHERE
                        @Pesquisa = ''

                        OR Nome LIKE '%' + @Pesquisa + '%'

                        OR (
                            @CpfPesquisa <> ''
                            AND CPF LIKE '%' + @CpfPesquisa + '%'
                        )

                        OR (
                            @IdPesquisa IS NOT NULL
                            AND Id = @IdPesquisa
                        )

                    ORDER BY Id DESC";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Adiciona parâmetros tipados aos comandos
                    cmd.Parameters.Add(
                        "@Pesquisa",
                        SqlDbType.NVarChar,
                        200
                    ).Value = pesquisa;

                    cmd.Parameters.Add(
                        "@CpfPesquisa",
                        SqlDbType.VarChar,
                        20
                    ).Value = cpfPesquisa;

                    cmd.Parameters.Add(
                        "@IdPesquisa",
                        SqlDbType.Int
                    ).Value = valorId;

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable tabela = new DataTable();

                        // Preenche DataTable com os resultados da consulta
                        adapter.Fill(tabela);

                        // Vincula dados ao GridView
                        gvPessoas.DataSource = tabela;
                        gvPessoas.DataBind();
                    }
                }
            }
        }
            
        protected void btnPesquisar_Click(object sender, EventArgs e)
        {
            gvPessoas.EditIndex = -1;

            CarregarPessoas(txtPesquisa.Text);
        }

        protected void btnLimpar_Click(object sender, EventArgs e)
        {
            txtPesquisa.Text = "";
            gvPessoas.EditIndex = -1;

            CarregarPessoas();
        }

        // Inicia edição de linha no GridView
        protected void gvPessoas_RowEditing(
            object sender,
            GridViewEditEventArgs e)
        {
            // Define índice da linha a ser editada
            gvPessoas.EditIndex = e.NewEditIndex;

            // Recarrega dados para mostrar controles de edição
            CarregarPessoas(txtPesquisa.Text);
        }

        // Cancela edição de linha no GridView
        protected void gvPessoas_RowCancelingEdit(
            object sender,
            GridViewCancelEditEventArgs e)
        {
            // Sai do modo de edição
            gvPessoas.EditIndex = -1;

            // Recarrega dados (mantendo filtro atual)
            CarregarPessoas(txtPesquisa.Text);
        }

        // Atualiza registro editado no GridView
        protected void gvPessoas_RowUpdating(
            object sender,
            GridViewUpdateEventArgs e)
        {
            // Obtém Id da linha editada a partir das chaves do GridView
            int id = Convert.ToInt32(
                gvPessoas.DataKeys[e.RowIndex].Value
            );

            // Lê novos valores fornecidos pelo usuário durante a edição
            string nome = e.NewValues["Nome"]?.ToString().Trim();
            string cpf = e.NewValues["CPF"]?.ToString().Trim();
            string email = e.NewValues["Email"]?.ToString().Trim();
            string telefone = e.NewValues["Telefone"]?.ToString().Trim();

            // Validações simples (poderia mostrar mensagem ao usuário)
            if (string.IsNullOrWhiteSpace(nome))
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(cpf))
            {
                return;
            }

            string conexao = ConfigurationManager
                .ConnectionStrings["WebFormsDB"]
                .ConnectionString;

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                // Comando UPDATE parametrizado
                string sql = @"
                    UPDATE Pessoa
                    SET Nome = @Nome,
                        CPF = @CPF,
                        Email = @Email,
                        Telefone = @Telefone
                    WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Adiciona parâmetros com tipos e tamanhos definidos
                    cmd.Parameters.Add(
                        "@Nome",
                        SqlDbType.NVarChar,
                        200
                    ).Value = nome;

                    cmd.Parameters.Add(
                        "@CPF",
                        SqlDbType.VarChar,
                        14
                    ).Value = cpf;

                    cmd.Parameters.Add(
                        "@Email",
                        SqlDbType.NVarChar,
                        255
                    ).Value =
                        string.IsNullOrWhiteSpace(email)
                            ? (object)DBNull.Value
                            : email;

                    cmd.Parameters.Add(
                        "@Telefone",
                        SqlDbType.VarChar,
                        20
                    ).Value =
                        string.IsNullOrWhiteSpace(telefone)
                            ? (object)DBNull.Value
                            : telefone;

                    cmd.Parameters.Add(
                        "@Id",
                        SqlDbType.Int
                    ).Value = id;

                    // Executa atualização no banco
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Sai do modo de edição após salvar
            gvPessoas.EditIndex = -1;

            // Recarrega dados com o filtro atual
            CarregarPessoas(txtPesquisa.Text);
        }

        // Exclui registro selecionado no GridView
        protected void gvPessoas_RowDeleting(
            object sender,
            GridViewDeleteEventArgs e)
        {
            // Obtém Id da linha a ser excluída
            int id = Convert.ToInt32(
                gvPessoas.DataKeys[e.RowIndex].Value
            );

            string conexao = ConfigurationManager
                .ConnectionStrings["WebFormsDB"]
                .ConnectionString;

            using (SqlConnection conn = new SqlConnection(conexao))
            {
                string sql = "DELETE FROM Pessoa WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    // Parâmetro de Id
                    cmd.Parameters.Add(
                        "@Id",
                        SqlDbType.Int
                    ).Value = id;

                    // Executa exclusão
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            // Recarrega lista após exclusão
            CarregarPessoas(txtPesquisa.Text);
        }
    }
}