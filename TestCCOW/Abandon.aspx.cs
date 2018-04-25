
namespace CCOWTest
{
    /// <summary>
    /// This page appends the session
    /// </summary>
    public partial class Abandon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            CCOWUtility.Stop(this);
        }
    }
}
