using com.sentillion.sdkweb.webcontextor;
using System;
using System.Web.UI.WebControls;

namespace CCOWTest
{
    // page for testing changing state and changing context.
    // when context changes and it is a new patient, redirects to
    // patient page
    public partial class Default : System.Web.UI.Page
    {
        
        // members used to web listener object for setting parameters
        protected string contextManagerUrl = string.Empty;
        protected string participantCoupon = string.Empty;
        protected string contextCoupon = string.Empty;

        protected ContextItemCollection wcic;


        #region page event handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["appName"] != null)
                AppNameLabel.Text = (string)Session["appName"];
            else
                return;
            string[] blob = new string[1];
            if (Session["BLOB"] == null)
            {
                ChangeStateButton.Visible = false;
                ChangeContextButton.Visible = false;
                StateLabel.Text = "No Appliciance";
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
                ChangeStateButton.Text = "Not Running";
                StateLabel.Text = "Start Running";
                ChangeContextButton.Visible = false;
                return;
            }

            int state = webC.GetState(blob);
            switch (state)
            {
                case WebContextor.CsNotRunning:
                    if (!IsPostBack)
                    {
                        ChangeStateButton.Text = "Not Running";
                        StateLabel.Text = "Start Running";
                        ChangeStateButton.Visible = true;
                    }
                    ChangeContextButton.Visible = false;
                    StateLabel.Text = "Not Running";
                    break;

                case WebContextor.CsParticipating:
                    ChangeStateButton.Visible = true;
                    if (!IsPostBack)
                    {
                        ChangeStateButton.Text = "Suspend";
                        StateLabel.Text = "Participating";
                        ChangeContextButton.Visible = true;
                        CCOWUtility.addNotifierApplet(this, blob);
                    }
                    wcic = webC.GetCurrentContext(blob);
                    if (wcic.Count() <  2 )
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
                    break;

                case WebContextor.CsSuspended:
                    if (!IsPostBack)
                    {
                        ChangeStateButton.Visible = true;
                        ChangeStateButton.Text = "Resume";
                        ChangeContextButton.Visible = false;
                        CCOWUtility.addNotifierApplet(this, blob);
                    }
                    StateLabel.Text = "Suspended";
                    break;

                default:
                    ChangeStateButton.Visible = false;
                    ChangeContextButton.Visible = false;
                    StateLabel.Text = "Unknown State";
                    break;
            }
            Session["BLOB"] = blob[0];
        }
        #endregion

        #region Button click event handlers
        /// <summary>
        /// Event to change Context from screen button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ChangeStateButton_Click(object sender, EventArgs e)
        {
            WebContextor webC = new WebContextor();
            string[] blob = new string[1];
            
            try
            {
                int state = 0;
                blob[0] = (string)Session["BLOB"];
                try
                {
                    state = webC.GetState(blob);
                }
                catch
                {
                    Response.Redirect("CCOWInit.aspx");
                }

                switch (state)
                {
                    case WebContextor.CsParticipating:
                        try
                        {
                            webC.CancelContextChange(blob);
                        }
                        catch (com.sentillion.sdkweb.webcontextor.TransactionNotInProgressException)
                        {
                            // no transaction in progress
                        }
                        webC.Suspend(blob);
                        ChangeStateButton.Text = "Resume";
                        StateLabel.Text = "Suspended";
                        ChangeContextButton.Visible = false;
                        break;
                    case WebContextor.CsSuspended:
                        webC.Resume(blob);
                        ChangeContextButton.Visible = true;
                        ChangeStateButton.Text = "Suspend";
                        StateLabel.Text = "Participating";
                        break;
                    case WebContextor.CsNotRunning:
                        Response.Redirect("CCOWInit.aspx");
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.Text = ex.Message;
            }
            finally
            {
                Session["BLOB"] = blob[0];
            }
        }

        /// <summary>
        /// Event for handling button for changing context. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// create made up patient to put into context
        /// </remarks>
        protected void ChangeContextButton_Click(object sender, EventArgs e)
        {
            ContextItemCollection contextItemCollection =
                new ContextItemCollection();

            contextItemCollection.Add(new ContextItem("Patient.CO.PatientName", "LEDERER^JEFFREY^^^^"));
            contextItemCollection.Add(new ContextItem("Patient.CO.Sex", "M"));
            contextItemCollection.Add(new ContextItem("patient.id.mrn.empi", "556147"));
            contextItemCollection.Add(new ContextItem("patient.id.mrn.ssn", "172385585"));
            contextItemCollection.Add(new ContextItem("Patient.CO.DateTimeOfBirth", "194704170000"));
            contextItemCollection.Add(new ContextItem("encounter.id.visitnumber.finnumeric", "275908426"));
            contextItemCollection.Add(new ContextItem("patient.id.mrn.epic", "841110407"));
            CCOWUtility.ChangeContext(this, contextItemCollection);
        }

        protected void ContextODS_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["context"] = wcic;
        }
        #endregion
    }
}
