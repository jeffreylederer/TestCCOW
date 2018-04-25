using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;

namespace CCOWTest
{
    /// <summary>
    /// Summary description for _Default.
    /// </summary>
    public partial class CCOWInit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            Session["SLAVEAPPS"] = "";
            Session["PatientPage"] = PatientPageUrl;
        }

        /// <summary>
        /// Used on aspx page
        /// </summary>
        public string ContextParticipantUrl
        {
            get
            {
                return Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/")) + "/ContextParticipant.aspx";
            }
        }
        
        /// <summary>
        /// Used on aspx page
        /// </summary>
        private string PatientPageUrl
        {
            get
            {
                return Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/")) + "/PatientPage.aspx";
            }
        }

    }
}
