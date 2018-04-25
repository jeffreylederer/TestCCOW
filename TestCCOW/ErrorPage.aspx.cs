using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

public partial class ErrorPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError().GetBaseException();
        if (ex != null)
        {
            if (HttpContext.Current.User.Identity.AuthenticationType == "Forms")
                ErrorLabel.Text = ex.Message + "<br/>" + Regex.Replace(ex.StackTrace, "\\r", "<br/>");
            else
                ErrorLabel.Text = ex.Message;
        }
        else
            ErrorLabel.Text = "Unknown Exception Occurred";
        Server.ClearError();
    }
}
