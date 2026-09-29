<%@ Page Title="Administração de Pessoas"
    Language="C#"
    MasterPageFile="~/Site.Master"
    AutoEventWireup="true"
    CodeBehind="Admin.aspx.cs"
    Inherits="WebFormsProjeto1.Admin" %>

<asp:Content ID="BodyContent"
    ContentPlaceHolderID="MainContent"
    runat="server">

    <div class="container">

        <h2>Administração de Pessoas</h2>


        <asp:HyperLink
            ID="lnkCadastro"
            runat="server"
            NavigateUrl="~/Default.aspx"
            CssClass="btn btn-primary">
            Voltar para Cadastro
        </asp:HyperLink>



        <br />


                <div class="form-group">
                    <asp:TextBox
                        ID="txtPesquisa"
                        runat="server"
                        CssClass="form-control"
                        Placeholder="Pesquisar por nome, ID ou CPF">
                    </asp:TextBox>

                    <br />

                    &nbsp;

                    </div>

                    <asp:Button
                        ID="btnLimpar"
                        runat="server"
                        Text="Limpar"
                        CssClass="btn btn-secondary"
                        OnClick="btnLimpar_Click" />

                    <asp:Button
                        ID="btnPesquisar"
                        runat="server"
                        Text="Pesquisar"
                        CssClass="btn btn-primary"
                        OnClick="btnPesquisar_Click" />

        <br />
        <br />

        <asp:GridView ID="gvPessoas"
            runat="server"
            AutoGenerateColumns="False"
            CssClass="table table-bordered table-striped"
            DataKeyNames="Id"
            OnRowEditing="gvPessoas_RowEditing"
            OnRowCancelingEdit="gvPessoas_RowCancelingEdit"
            OnRowUpdating="gvPessoas_RowUpdating"
            OnRowDeleting="gvPessoas_RowDeleting">

            <Columns>

                <asp:BoundField
                    DataField="Id"
                    HeaderText="ID"
                    ReadOnly="True" />

                <asp:BoundField
                    DataField="Nome"
                    HeaderText="Nome" />

                <asp:BoundField
                    DataField="CPF"
                    HeaderText="CPF" />

                <asp:BoundField
                    DataField="Email"
                    HeaderText="E-mail" />

                <asp:BoundField
                    DataField="Telefone"
                    HeaderText="Telefone" />

                <asp:CommandField
                    HeaderText="Ações"
                    ShowEditButton="True"
                    ShowDeleteButton="True"
                    EditText="Alterar"
                    DeleteText="Excluir"
                    CancelText="Cancelar"
                    UpdateText="Salvar"
                    ButtonType="Button" />

            </Columns>

        </asp:GridView>

    </div>

</asp:Content>