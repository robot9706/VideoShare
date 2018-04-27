using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoShare.Data.Model;
using VideoShare.Pages.Renderers;

namespace VideoShare.Pages
{
    public partial class Watch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["v"] != null)
            {
                int id = 0;
                if (!Int32.TryParse(Request.Params["v"], out id))
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    Video video = Video.FindVideo(id);
                    if (video == null)
                    {
                        Response.Redirect("Index.aspx");
                    }
                    else
                    {
                        RenderPage(video);
                    }
                }
            }
            else
            {
                Response.Redirect("Index.aspx");
            }
        }

        private void RenderPage(Video video)
        {
            bool loggedIn = (Session["User"] != null);

            StringBuilder latestVideo = new StringBuilder();
            VideoHTMLRenderer.RenderBigView(video, latestVideo, loggedIn);
            VideoView.Text = latestVideo.ToString();

            StringBuilder commentBuilder = new StringBuilder();

            if (loggedIn)
            {
                commentBuilder.Append("<tr><td><div class='panel' style='width: 100%'><textarea id='commentText' placeholder='Megjegyzés' style='border: none; outline: none; background-color: transparent; width: 100%; resize: none;' rows='3'></textarea><div id='commentButton' style='width: 15%; background-color: #323232; text-align: center; cursor: pointer; border-radius: 5px' onclick='doComment(" + video.ID.ToString() + ")'><a style='color: #A0A0A0;'>Küldés</a></div></div></td></tr>");
            }

            List<VideoComment> comments = VideoComment.GetComments(video.ID);

            commentBuilder.Append("<tr><td><div class='panel' style='width: 100%; margin-top: 10px;'><a>Megjegyzések: " + comments.Count.ToString() + "</a></div></td></tr>");

            foreach (VideoComment c in comments)
            {
                User commenter = Data.Model.User.GetUserByID(c.UserID);

                commentBuilder.Append("<tr><td><div class='panel' style='width: 100%'><p>" + c.Comment + "</p><a>" + commenter.DisplayName + " - " + c.Date.ToString("yyyy-MM-dd HH:mm:ss") + "</a></div></td></tr>");
            }

            CommentBox.Text = commentBuilder.ToString();
        }
    }
}