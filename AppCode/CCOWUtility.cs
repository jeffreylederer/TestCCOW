using System;
using System.Collections.Generic;
using com.sentillion.sdkweb.webcontextor;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CCOWTest
{

    /// <summary>
    /// Assorted CCOW functions
    /// </summary>
    public static class CCOWUtility
    {
        /// <summary>
        /// Resume context participating. Only works if state is suspended
        /// </summary>
        /// <param name="page">current page, needed to get session</param>
        public static void Resume(Page page)
        {

            String[] blob = new String[1];

            // if blob is empty, then not talking to context server
            if (page.Session["BLOB"] == null)
                return;
            blob[0] = (string)page.Session["BLOB"];

            // this should always work
            WebContextor webC = new WebContextor();
            

            // check if in correct state
            int state = 0;
            try
            {
                state = webC.GetState(blob);
            }
            catch
            {
                return;
            }

            // change state
            if (state == WebContextor.CsSuspended)
                webC.Resume(blob);
            page.Session["BLOB"] = blob[0];
        }

        /// <summary>
        /// Suspend context, only works if current state is participating
        /// </summary>
        /// <param name="page">current page, needed to get session</param>
        public static void Suspend(Page page)
        {
            String[] blob = new String[1];

            // if blob is empty, then not talking to context server
            if (page.Session["BLOB"] == null)
                return;
            blob[0] = (string)page.Session["BLOB"];

            // this should always work
            WebContextor webC = new WebContextor();
            

            // check if in correct state
            int state = 0;
            try
            {
                state = webC.GetState(blob);
            }
            catch
            {
                return;
            }

            // change state
            if (state == WebContextor.CsParticipating)
                webC.Suspend(blob);
            page.Session["BLOB"] = blob[0];
        }

         /// <summary>
        /// Changes context. Uses SessionHandler.EMPINumber to determine
        /// which patient context to change from.
        /// </summary>
        /// <param name="page">current page, needed to get session</param>
        public static void ChangeContext(Page page,ContextItemCollection wcic)
        {
            String[] blob = new String[1];
            bool[] noContinue = new bool[1];


            // if blob is empty, then not talking to context server
            if (page.Session["BLOB"] == null)
                return;

            blob[0] = (string)page.Session["BLOB"];

            // this should always work
            WebContextor webC = new WebContextor();

            // check if in correct state
            int state = 0;
            try
            {
                state = webC.GetState(blob);
            }
            catch
            {
                return;
            }
            if (state != WebContextor.CsParticipating)
            {
                page.Session["BLOB"] = blob[0];
                return;
            }


            try
            {
                // Start a context change transaction
                webC.StartContextChange(blob);



                // End of the context change transaction.  During this call the Context
                // Manager calls the mapping agent(s) (if any), surveys all participants, the
                // Contextor pops up the dialog (if needed), and finally the Context Manager
                // notifies all participants with a commit or cancel event.
                // This instigating app must act on the returned response value.
                webC.EndContextChange(blob, true, wcic, noContinue);

                // Context change is allowed to continue
                if (noContinue[0] == false)
                {
                    string[] coupon = new string[1];
                    page.Session["SLAVEAPPS"] = webC.CommitContextChange(blob, coupon);
                }
                else
                {
                    // When no continue array contains true value, it means that at least
                    // one application is busy or the Mapping Agent has invalidated the context change.
                    // The only possible choices are to break link or to cancel the change. 
                    webC.CancelContextChange(blob);
                }
            }
            catch
            {

            }
            finally
            {
                page.Session["BLOB"] = blob[0];
            }
        }

        /// <summary>
        /// End session with context server
        /// </summary>
        /// <param name="page">current page, needed to get session</param>
        public static void Stop(Page page)
        {
            String[] blob = new String[1];

            // if blob is empty, then not talking to context server
            if (page.Session["BLOB"] == null)
                return;
            blob[0] = (string)page.Session["BLOB"];

            // this should always work
            WebContextor webC = new WebContextor();
            
            // end session
            try
            {
                webC.Stop(blob);
            }
            catch
            {
            }
            page.Session["BLOB"] = null;
        }

        /// <summary>
        /// Add a listener applet to the page
        /// </summary>
        /// <param name="page">current page, needed to get session</param>
        /// <param name="blob">The blob is passed here so and the user will not have to save and
        /// reload from session object</param>
        public static void addNotifierApplet(Page page, string[] blob)
        {

            WebContextor webC = new WebContextor();
            int appletType = VergenceAppletsHtml.INTERNET_EXPLORER;
            VergenceAppletsHtml vergenceAppletsHtml = new VergenceAppletsHtml(appletType, "applets", "WebIEApplets.cab", "debug");

            // Determine if Web-based applications need to be notified
            string apps = (string)page.Session["SLAVEAPPS"];
            if (!string.IsNullOrEmpty(apps))
            {
                string contextCoupon = webC.GetContextCoupon(blob); // needed by web listener object
                page.Response.Write(vergenceAppletsHtml.notifierTag(apps, contextCoupon, "10"));
            }

        }
    }
}