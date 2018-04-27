using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoShare.Data.Model;

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
                case "upvote":
                    DoVote(req, true);
                    break;
                case "downvote":
                    DoVote(req, false);
                    break;
                case "comment":
                    DoComment(req);
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
                Response.Write("Hiba!");
                return;
            }

            string userName = request.Params["u"];
            string pwHash = request.Params["p"];

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(pwHash))
            {
                Response.Write("Hibás adatok!");
                return;
            }

            User user = Data.Model.User.FindUser(userName, pwHash);

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
                Response.Write("Hiba!");
                return;
            }

            string userName = request.Params["u"];
            string email = request.Params["m"];
            string pwHash = request.Params["p"];

            if (Data.Model.User.GetUser(userName) != null)
            {
                Response.Write("A felhasználó név már létezik!");

                return;
            }

            User user = Data.Model.User.Register(userName, email, pwHash);

            if (user != null)
            {
                Session["User"] = user;

                Response.Write("ok");
            }
            else
            {
                Response.Write("Hiba!");
            }
        }

        private void DoVote(HttpRequest request, bool up)
        {
            if (request.Params["v"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int id = 0;
            if (!Int32.TryParse(request.Params["v"], out id))
            {
                Response.Write("Hiba!");
                return;
            }

            Video v = Video.FindVideo(id);
            if (v == null)
            {
                Response.Write("Hiba!");
                return;
            }

            bool ok = false;
            if (up)
            {
                ok = v.Upvote();
            }
            else
            {
                ok = v.Downvote();
            }

            if (ok)
            {
                Response.Write("ok");
            }
            else
            {
                Response.Write("Hiba!");
            }
        }

        private void DoComment(HttpRequest request)
        {
            if (request.Params["v"] == null || request.Params["c"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            if (Session["User"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int id = 0;
            if (!Int32.TryParse(request.Params["v"], out id))
            {
                Response.Write("Hiba!");
                return;
            }

            Video v = Video.FindVideo(id);
            if (v == null)
            {
                Response.Write("Hiba!");
                return;
            }

            string comment = request.Params["c"];
            if (string.IsNullOrEmpty(comment))
            {
                Response.Write("Hiba!");
                return;
            }

            VideoComment newComment = new Data.Model.VideoComment();
            newComment.Comment = comment;
            newComment.Date = DateTime.Now;
            newComment.VideoID = id;
            newComment.UserID = ((User)Session["User"]).ID;

            if (Global.Database.Insert<VideoComment>(newComment))
            {
                Response.Write("ok");
            }
            else
            {
                Response.Write("Hiba!");
            }
        }
    }
}