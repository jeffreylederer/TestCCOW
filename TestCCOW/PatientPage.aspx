<%@ Page Language="C#" AutoEventWireup="true" Inherits="CCOWTest.PatientPage"  MasterPageFile="~/MasterPage.master" Codebehind="PatientPage.aspx.cs" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" 
            OldValuesParameterFormatString="original_{0}" SelectMethod="GetPatient" 
            TypeName="CCOWTest.CCOWPatient">
            <SelectParameters>
                <asp:SessionParameter Name="patient" SessionField="ContextPatient" 
                    Type="Object" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <h1>Patient Page</h1>
        <br />
        This is the landing page when the patient context switches.<br /><br />
        <asp:Label runat="server" ID="ErrorLabel" ForeColor="Red" EnableViewState="false" />
        <table cellpadding='1' border="1">
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
        <br /><br />
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
    <br />
    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Abandon.aspx">Abandon Session</asp:HyperLink>
    <br />
    <asp:HyperLink ID="HyperLink2" runat="server" NavigateUrl="~/Default.aspx">Back to application page</asp:HyperLink>
    <br />
</asp:Content>