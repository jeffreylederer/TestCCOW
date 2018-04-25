<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCOWTest.Default"  MasterPageFile="~/MasterPage.master" Codebehind="Default.aspx.cs" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1" >
    <h1>Application Page</h1>
    <br />
    This page represents a typical application page. When the context 
    changes to a different user, the application redirects to the patient page.<br />
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Abandon.aspx">Abandon Session</asp:HyperLink>
    <br />
    <br />
        <table cellpadding='1' border="1">
        <tr>
        <td align="right">Error Message</td>
        <td><asp:Label ID="ErrorLabel" runat="server" EnableViewState="False"></asp:Label></td>
        </tr>
        <tr>
        <td align="right">Current State</td>
        <td><asp:Label ID="StateLabel" runat="server" style="font-weight: 700"></asp:Label></td>
        </tr>
        <tr>
        <td align="right">AppName</td>
        <td><asp:Label ID="AppNameLabel" runat="server"></asp:Label></td>
        </tr>
        <tr>
        <td align="right">Context Manager URL</td>
        <td><%=contextManagerUrl%></td>
        </tr>
        <tr>
        <td align="right">Partipant Coupon</td>
        <td><%=participantCoupon%></td>
        </tr>
        <tr>
        <td align="right">Context Coupon</td>
        <td><%=contextCoupon%></td>
        </tr>                
        </table>
        <br />   <asp:Button ID="ChangeStateButton" runat="server" 
        onclick="ChangeStateButton_Click" />
    <br />
    <br />
    <asp:Button ID="ChangeContextButton" runat="server" 
        onclick="ChangeContextButton_Click" Text="Change Context" />
    <br />
    <br />
    <div>
    
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetPatient" 
            TypeName="CCOWTest.CCOWPatient">
            <SelectParameters>
                <asp:SessionParameter Name="patient" SessionField="ContextPatient" 
                    Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
    
    </div>
    <asp:GridView ID="ContextGridView" runat="server"  EmptyDataText="No Context"
            DataSourceID="ContextODS" Caption="Context" >
        </asp:GridView>
    <asp:ObjectDataSource ID="ContextODS" runat="server" 
             OldValuesParameterFormatString="original_{0}" 
               SelectMethod="GetCollection" 
           TypeName="CCOWTest.ContextValues" onselecting="ContextODS_Selecting">
           <SelectParameters>
               <asp:Parameter Name="context" Type="Object" />
           </SelectParameters>
         </asp:ObjectDataSource>       
</asp:Content>