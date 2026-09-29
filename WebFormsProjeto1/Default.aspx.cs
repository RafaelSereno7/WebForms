using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace WebFormsProjeto1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblMensagem.Text = string.Empty;
            }
        }

        protected void btnCadastrar_Click(object sender, EventArgs e)
        {
            var nome = txtNome?.Text?.Trim();
            var cpf = txtCPF?.Text?.Trim();
            var email = txtEmail?.Text?.Trim();
            var telefone = txtTelefone?.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nome))
            {
                lblMensagem.Text = "Nome é obrigatório.";
                return;
            }

            if (string.IsNullOrWhiteSpace(cpf))
            {
                lblMensagem.Text = "CPF é obrigatório.";
                return;
            }

            try
            {
                string conexao = ConfigurationManager
                    .ConnectionStrings["WebFormsDB"]
                    .ConnectionString;

                using (SqlConnection conn = new SqlConnection(conexao))
                {
                    string sql = @"
                        INSERT INTO Pessoa (Nome, CPF, Email, Telefone)
                        VALUES (@Nome, @CPF, @Email, @Telefone)";

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", nome);
                        cmd.Parameters.AddWithValue("@CPF", cpf);
                        cmd.Parameters.AddWithValue("@Email",
                            string.IsNullOrWhiteSpace(email) ? (object)DBNull.Value : email);
                        cmd.Parameters.AddWithValue("@Telefone",
                            string.IsNullOrWhiteSpace(telefone) ? (object)DBNull.Value : telefone);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                lblMensagem.Text = "Pessoa cadastrada com sucesso!";

                txtNome.Text = string.Empty;
                txtCPF.Text = string.Empty;
                txtEmail.Text = string.Empty;
                txtTelefone.Text = string.Empty;
            }
            catch (Exception ex)
            {
                lblMensagem.Text = "Erro ao cadastrar: " + ex.Message;
            }
        }
    }
}