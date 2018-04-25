using System;
using System.Configuration;
using com.sentillion.sdkweb.webcontextor;


namespace CCOWTest
{
    /// <summary>
    /// This page sets the registers with context server. It should only be run once. Should only get here
    /// if CCOWInit.aspx found a context server
    /// </summary>
    public partial class GetContextPage : System.Web.UI.Page
    {

        protected string contextManagerUrl = string.Empty;
        protected string participantCoupon = string.Empty;
        protected string contextCoupon = string.Empty;

        protected void Page_Load(object sender, System.EventArgs e)
        {
            string appName = ConfigurationManager.AppSettings["CCOWAppName"];
            string passcode = ConfigurationManager.AppSettings["CCOWAppPasscode"];

            ViewState["ContextPatient"] = null;
            string[] blob = new string[1];

            // get the context information passed from the first page, which contains
            // the web locator ojbect
            contextManagerUrl = Request.QueryString["contextManagerUrl"];

            // create a web context object
            WebContextor webC = new WebContextor();
            
           
            //if first time through, context blob is empty; therefore put this program
            //in context
            if (Session["BLOB"] == null)
            {
                appName = webC.UniqueName(appName);
                Session["appName"] = appName;
                     webC.Run(blob, contextManagerUrl, ContextParticipantUrl, appName, passcode, true);
                if (string.IsNullOrEmpty(blob[0]))
                {
                    throw new Exception("empty blob after run method");
                }
            }
            else
                blob[0] = (string)Session["BLOB"];

            // Get Context Manager URL from Web context object, if not found
            // on this page's query string
            if (contextManagerUrl == null)
                contextManagerUrl = webC.GetContextManagerUrl(blob); // needed by web listener object

            // get other values needed by web listener object parameter values
            participantCoupon = webC.GetParticipantCoupon(blob); // needed by web listener object
            contextCoupon = webC.GetContextCoupon(blob); // needed by web listener object

            // Get context state and context collection
            switch (webC.GetState(blob))
            {
                case WebContextor.CsNotRunning:
                    break;

                case WebContextor.CsParticipating:
                    {
                        ContextItemCollection wcic = webC.GetCurrentContext(blob);
                        Session["BLOB"] = blob[0];
                        // no one in context, go directly to the default page
                        if (wcic.Count() < 2)
                        {
                            Session["ContextPatient"] = null;
                            Response.Redirect("default.aspx");
                        }

                        // found patient in context, go to patient search found page
                        try
                        {
                            Session["ContextPatient"] = wcic.Item("patient.co.patientname").Value;
                        }
                        catch
                        {
                           
                            Session["ContextPatient"] = "No patient.co.patientname on Context Page";
                        }
                        Response.Redirect("PatientPage.aspx");
                    }
                    break;

                case WebContextor.CsSuspended:
                    break;

                default:
                    throw new Exception("Unexpected value from GetState");
            }


            // remember context blob value
            Session["BLOB"] = blob[0];
            Response.Redirect("default.aspx");
        }

        /// <summary>
        /// property that returns the current page's absolute address, used by Web Listener refreshURL parameter
        /// </summary>
        public string ContextParticipantUrl
        {
            get
            {
                return Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/GetContextPage.aspx")) +
                "/ContextParticipant.aspx";
            }
        }
    }
}
