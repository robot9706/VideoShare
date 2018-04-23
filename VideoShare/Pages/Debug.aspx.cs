using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VideoShare.Pages
{
    public partial class Debug : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblDbCon.InnerHtml = (Global.DatabaseConnected ? "Ok" : "Failed");
        }
    }
}