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
                    RenderUser(data, (Session["User"] != null && ((User)Session["User"]).ID == data.ID));
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
            //Profile data
            Profile_UserName.Text = user.DisplayName;
            Profile_JoinDate.Text = user.RegistrationDate.ToString("yyyy-MM-dd");
            Profile_Desc.Text = user.Info;

            Tuple<int, int> videosViews = user.GetVideosAndViews();
            Profile_VideoCount.Text = videosViews.Item1.ToString();
            Profile_ViewCount.Text = videosViews.Item2.ToString();

            //Video list
            List<Video> videos = user.GetVideosDateOrdered();

            StringBuilder html = new StringBuilder();

            if (videos.Count > 0)
            {
                StringBuilder latestVideo = new StringBuilder();
                VideoHTMLRenderer.RenderBigView(videos[0], latestVideo, Session["User"] as User, false);
                LatestVideoBox.Text = latestVideo.ToString();

                html.Append("<div class='profilePanel' style='padding: 10px'><a class='profileText'>Feltöltések:</a><table style='width: 100%'>");
                {
                    int rowCount = (int)Math.Ceiling((double)((float)videos.Count / 6));

                    for (int row = 0; row < rowCount; row++)
                    {
                        int offset = row * 6;

                        html.Append("<tr>");
                        {
                            for (int x = offset; x < offset + 6; x++)
                            {
                                if (x >= videos.Count)
                                {
                                    html.Append("<td style='width: 16%'></td>");
                                }
                                else
                                {
                                    html.Append("<td style='width: 16%'>");

                                    VideoHTMLRenderer.RenderThumbnail(videos[x], html, isOwner);

                                    html.Append("</td>");
                                }
                            }
                        }
                        html.Append("</tr>");
                    }
                }
                html.Append("</table></div>");
            }
            else
            {
                LatestVideoBox.Text = "<video width='800' height='480' controls='controls'><source='' type='video/mp4' /></ video>";
            }

            //Playlists
            List<Playlist> lists = user.GetPlaylists();

            foreach (Playlist list in lists)
            {
                List<Video> listVideos = list.GetVideos();

				string deleteButtonHtml = (isOwner ? "<div style='background-color:#323232; float:right; padding: 2px; cursor: pointer' onclick='deleteList(" + list.ID.ToString() + ")'><a>Törlés</a></div>" : string.Empty);

				html.Append("<div class='profilePanel' style='padding: 10px'><a class='profileText'>" + list.Title + "</a><a class='profileTextSmall' style='margin-left: 10px'>Létrehozva: " + list.CreationDate.ToString("yyyy-MM-dd HH:mm:ss") + ", Videók: " + listVideos.Count.ToString() + "</a>" + deleteButtonHtml + "<table style='width: 100%'>");
                {
                    int rowCount = (int)Math.Ceiling((double)((float)listVideos.Count / 6));

                    for (int row = 0; row < rowCount; row++)
                    {
                        int offset = row * 6;

                        html.Append("<tr>");
                        {
                            for (int x = offset; x < offset + 6; x++)
                            {
                                if (x >= listVideos.Count)
                                {
                                    html.Append("<td style='width: 16%'></td>");
                                }
                                else
                                {
                                    html.Append("<td style='width: 16%'>");

                                    VideoHTMLRenderer.RenderThumbnail(listVideos[x], html, isOwner, list);

                                    html.Append("</td>");
                                }
                            }
                        }
                        html.Append("</tr>");
                    }
                }
                html.Append("</table></div>");
            }

            //Output the rendered text
            VideoList.Text = html.ToString();

            //Upload popup
            StringBuilder categoryList = new StringBuilder();

            List<Category> allCategory = Global.Database.SelectAll<Category>();

            categoryList.Append("<div id='catList'>");
            foreach (Category cat in allCategory)
            {
                categoryList.Append("<input id='" + cat.ID.ToString() + "' type='checkbox'><a>" + cat.Name + "</a></input>");
            }
            categoryList.Append("</div>");

            CategoryList.Text = categoryList.ToString();

            //Header script
            if (isOwner)
            {
				string descEditAllowed = "false";
				if (isOwner)
					descEditAllowed = "true";

                ExtraScript.Text = "<script>_isDescEditable=" + descEditAllowed + ";var ll=window.onload;window.onload=function(){document.getElementById('userPanel').style.removeProperty('visibility');if(ll!=null)ll();}</script>";
            }
        }
    }
}