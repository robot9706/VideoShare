using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoShare.Data.Objects;

namespace VideoShare.Pages
{
    public partial class Services : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpRequest req = Request;

            Response.Clear();
            Response.StatusCode = 200;

            if (req.Params["f"] == null)
            {
                Response.Write("Unknown function!");

                return;
            }

            switch (req.Params["f"])
            {
                case "logout":
                    Session["User"] = null;
                    break;
                case "login":
                    Login(req);
                    break;
                case "register":
                    Register(req);
                    break;

                default:
                    Response.Write("Unknown function!");
                    return;
            }
        }

        private void Login(HttpRequest request)
        {
            if (request.Params["u"] == null || request.Params["p"] == null)
            {
                Response.StatusCode = 500;
            }

            string userName = request.Params["u"];
            string pwHash = request.Params["p"];

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwHash))
            {
                Response.Write("Hibás adatok!");
                return;
            }

            WebsiteUser user = WebsiteUser.LoginUser(userName, pwHash);

            if (user == null)
            {
                Response.Write("Hibás adatok!");
                return;
            }

            Session["User"] = user;
            Response.Write("ok");
        }

        private void Register(HttpRequest request)
        {
            if (request.Params["u"] == null || request.Params["p"] == null || request.Params["m"] == null)
            {
                Response.StatusCode = 500;
            }

            string userName = request.Params["u"];
            string email = request.Params["m"];
            string pwHash = request.Params["p"];

            WebsiteUser user = WebsiteUser.Register(userName, email, pwHash);

            if (user != null)
            {
                Session["User"] = user;

                Response.Write("ok");
            }
            else
            {
                Response.Write("A felhasználó név már létezik!");
            }
        }
    }
}