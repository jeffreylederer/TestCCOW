using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using com.sentillion.sdkweb.webcontextor;

namespace CCOWTest
{
    /// <summary>
    /// This page just shows who is in context. Whenever the patient changes, we end here.
    /// </summary>
    public partial class PatientPage : System.Web.UI.Page
    {

        protected string contextManagerUrl = string.Empty;
        protected string participantCoupon = string.Empty;
        protected string contextCoupon = string.Empty;

        private ContextItemCollection wcic;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["appName"] != null)
                AppNameLabel.Text = (string)Session["appName"];
            else
                return;

            string[] blob = new string[1];
            int i= Request.QueryString.Count;
            if (Session["BLOB"] == null)
                throw new Exception("blob is null");
            blob[0] = (string)Session["BLOB"];

            WebContextor webC = new WebContextor();
            
            participantCoupon = webC.GetParticipantCoupon(blob); // needed by web listener object
            contextCoupon = webC.GetContextCoupon(blob); // needed by web listener object
            contextManagerUrl = webC.GetContextManagerUrl(blob);

            // Get context state and context collection
            switch (webC.GetState(blob))
            {
                case WebContextor.CsNotRunning:
                    break;

                case WebContextor.CsParticipating:
                    {
                        wcic = webC.GetCurrentContext(blob);
                        if (wcic.Count() < 2)
                        {
                            Session["ContextPatient"] = null;
                            break;
                        }
                        try
                        {
                            string patient = wcic.Item("patient.co.patientname").Value;
                            Session["ContextPatient"] = patient;
                        }
                        catch
                        {
                            ErrorLabel.Text = "Could not find patient.co.patientname";
                            Session["ContextPatient"] = "No patient.co.patientname";
                        }
                        CCOWUtility.addNotifierApplet(this, blob);
                    }
                    break;

                case WebContextor.CsSuspended:
                    CCOWUtility.addNotifierApplet(this, blob);
                    break;

                default:
                    throw new Exception("Unexpected value from GetState");
            }
            Session["BLOB"] = blob[0];
        }

        protected void ContextODS_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["context"] = wcic;
        }
    }
}
