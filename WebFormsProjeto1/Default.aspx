<%@ Page Title="Cadastro de Pessoa" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="WebFormsProjeto1._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">

        <h2>Cadastro de Pessoa</h2>

        <div class="form-group">
            <label>Nome:</label>
            <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <br />

        <div class="form-group">
            <label>CPF: </label>
                <asp:TextBox ID="txtCPF" runat="server" CssClass="form-control" MaxLength="14" oninput="formatarCPF(this)"> </asp:TextBox> 
        </div>

        <br />

        <div class="form-group">
            <label>E-mail:</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <br />

        <div class="form-group">
            <label>Telefone:</label>
            <asp:TextBox ID="txtTelefone" runat="server" CssClass="form-control"></asp:TextBox>
        </div>

        <br />

        <asp:Button ID="btnCadastrar"
            runat="server"
            Text="Cadastrar"
            CssClass="btn btn-primary"
            OnClick="btnCadastrar_Click" />

        <br />
        <br />

        <asp:Label ID="lblMensagem" runat="server"></asp:Label>

    </div>

    <script>
    function formatarCPF(campo) {
        let cpf = campo.value.replace(/\D/g, '');

        cpf = cpf.substring(0, 11);

        if (cpf.length > 9) {
            cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{1,2})/, '$1.$2.$3-$4');
        } 
        else if (cpf.length > 6) {
            cpf = cpf.replace(/(\d{3})(\d{3})(\d{1,3})/, '$1.$2.$3');
        } 
        else if (cpf.length > 3) {
            cpf = cpf.replace(/(\d{3})(\d{1,3})/, '$1.$2');
        }

        campo.value = cpf;
    }
    </script>

</asp:Content>