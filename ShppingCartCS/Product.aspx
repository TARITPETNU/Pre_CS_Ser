<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Product.aspx.cs" Inherits="ShppingCartCS.Product" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ThaiCreate.Com</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:Label ID="Product" runat="server" Font-Size="X-Large" Text="Product List"></asp:Label>
        <br />
    
        <asp:GridView ID="myGridView" runat="server" AutoGenerateColumns="False" 
            onrowcommand="myGridView_RowCommand" onrowdatabound="myGridView_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Picture">
                    <ItemTemplate>
                        <asp:Image ID="imgPicture" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProductID">
                    <ItemTemplate>
                        <asp:Label ID="lblProductID" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProductName">
                    <ItemTemplate>
                        <asp:Label ID="lblProductName" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Price">
                    <ItemTemplate>
                        <asp:Label ID="lblPrice" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AddCart">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkAddToCart" runat="server"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    </div>
    <asp:HyperLink ID="hplViewCart" runat="server" NavigateUrl="ViewCart.aspx">View Cart</asp:HyperLink>
    </form>
</body>
</html>