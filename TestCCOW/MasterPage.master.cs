using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.sentillion.sdkweb.webcontextor;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected string contextManagerUrl = string.Empty;
    protected string participantCoupon = string.Empty;
    protected string contextCoupon = string.Empty;
 
    protected void Page_Load(object sender, EventArgs e)
    {
        string[] blob = new string[1];
        if (Session["BLOB"] == null)
        {
              return;
        }
        blob[0] = (string)Session["BLOB"];
        WebContextor webC = new WebContextor();

        try
        {
            participantCoupon = webC.GetParticipantCoupon(blob); // needed by web listener object
            contextCoupon = webC.GetContextCoupon(blob); // needed by web listener object
            contextManagerUrl = webC.GetContextManagerUrl(blob);
        }
        catch
        {
            MsgLabel.Text = "No context";
        }

    }
}
