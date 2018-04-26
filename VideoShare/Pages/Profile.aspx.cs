using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoShare.Data.Model;

namespace VideoShare.Pages
{
    public partial class Profile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["p"] != null)
            {
                User data = Data.Model.User.GetUser(Request.Params["p"]);
                if (data == null)
                {
                    Response.Redirect("Index.aspx");
                }
                else
                {
                    RenderUser(data, false);
                }
            }
            else
            {
                if (Session["User"] == null)
                {
                    Response.Redirect("Login.aspx");
                }
                else
                {
                    RenderUser((User)Session["User"], true);
                }
            }
        }

        private void RenderUser(User user, bool isOwner)
        {
            Profile_UserName.Text = user.DisplayName;
            Profile_JoinDate.Text = user.RegistrationDate.ToString("yyyy-MM-dd");
            Profile_Desc.Text = user.Info;

            Tuple<int, int> videosViews = user.GetVideosAndViews();
            Profile_VideoCount.Text = videosViews.Item1.ToString();
            Profile_ViewCount.Text = videosViews.Item2.ToString();

            List<Video> videos = user.GetVideosDateOrdered();

            if (videos.Count > 0)
            {
                LatestVideoBox.Text = "<video width='800' height='480' controls='controls'><source='" + videos[0].GetContentLink() + "' type='video/mp4' /></video>";
                LatestVideoTitleBox.Text = "<a href='Video.aspx?v=" + videos[0].ID.ToString() + "' class='profileText'>" + videos[0].Title + "</a>";

                StringBuilder allVideosBuilder = new StringBuilder();

                allVideosBuilder.Append("<div class='profilePanel' style='padding: 10px'><a class='profileText'>Feltöltések:</a><table style='width: 100%'>");
                {
                    int rowCount = (int)Math.Ceiling((double)((float)videos.Count / 6));

                    for (int row = 0; row < rowCount; row++)
                    {
                        int offset = row * 6;

                        allVideosBuilder.Append("<tr>");
                        {
                            for (int x = offset; x < offset + 6; x++)
                            {
                                if (x >= videos.Count)
                                {
                                    allVideosBuilder.Append("<td style='width: 16%'></td>");
                                }
                                else
                                {
                                    allVideosBuilder.Append("<td style='width: 16%'><a href='Video.aspx?v=" + videos[x].ID.ToString() + "'><img style='cursor: pointer' src=\"" + videos[x].GetThumbnailLink() + "\" /></a><a href='Video.aspx?v=" + videos[x].ID.ToString() + "'>" + videos[x].Title + "</a></td>");
                                }
                            }
                        }
                        allVideosBuilder.Append("</tr>");
                    }
                }
                allVideosBuilder.Append("</table></div>");

                VideoList.Text = allVideosBuilder.ToString();
            }
            else
            {
                LatestVideoBox.Text = "<video width='800' height='480' controls='controls'><source='' type='video/mp4' /></ video>";
            }
        }
    }
}