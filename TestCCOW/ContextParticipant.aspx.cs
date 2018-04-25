using System;

namespace PIMS
{
	// Required to get this to work properly.
	// 1) Participant Url in Contextor.Run call must point to this page on the server.
	// 2) The page must have anonymous access.
	// 3) The 'accept' response in ContextChangesPending must include a blank 'reason'
	//		parameter.

	/// <summary>
	/// Summary description for ContextParticipant.
	/// </summary>
	public partial class ContextParticipant : System.Web.UI.Page
	{
		string incomingMethod;
		
		protected void Page_Load(object sender, System.EventArgs e)
		{
			incomingMethod = Request.QueryString["method"];

			switch (incomingMethod)
			{
				case "ContextChangesPending":
					ContextChangesPending();
					break;
				case "ContextChangesAccepted":
					ContextChangesAccepted();
					break;
				case "ContextChangesCanceled":
					ContextChangesCanceled();
					break;
				case "CommonContextTerminated":
					CommonContextTerminated();
					break;
				case "Ping":
					break;
				default:
					break;
			}

		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion

		private void ContextChangesPending()
		{
			Response.Write("decision=accept&reason=");
		}

		private void ContextChangesAccepted()
		{
		}

		private void ContextChangesCanceled()
		{
		}

		private void CommonContextTerminated()
		{
			
		}
	}
}
