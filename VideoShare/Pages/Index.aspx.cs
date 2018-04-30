using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoShare.Data;
using VideoShare.Data.Model;
using VideoShare.Pages.Renderers;

namespace VideoShare.Pages
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
			StringBuilder html = new StringBuilder();

			List<Video> videos = null;

			if (Session["User"] != null)
			{
				//Ajánlás bejelentkezett felhasználónak
				videos = VideoQuery.GetRandomSimilarVideos((User)Session["User"], 6);
				VideoHTMLRenderer.RenderVideoList(html, videos, "Ajánlott");
			}
			else
			{
				//Ajánlás
				videos = VideoQuery.GetRandomVideos(6);
				VideoHTMLRenderer.RenderVideoList(html, videos, "Ajánlott");
			}

			//Legújabb videók
			videos = VideoQuery.GetLatestVideos(6);
			VideoHTMLRenderer.RenderVideoList(html, videos, "Legújabb videók");

			//Legaktívabb feltöltő videói
			videos = VideoQuery.GetMostActiveUploaderVideos(6);
			VideoHTMLRenderer.RenderVideoList(html, videos, "Legaktívabb feltöltő videói");

			//Legaktívabb kommentelő videói
			videos = VideoQuery.GetMostActiveCommenterVideos(6);
			VideoHTMLRenderer.RenderVideoList(html, videos, "Legaktívabb kommentelő videói");

			ContentHTML.Text = html.ToString();

			//Category selector
			StringBuilder catSelect = new StringBuilder();
			{
				foreach (KeyValuePair<int, string> pair in Global.VideoCategories)
				{
					catSelect.Append("<option value=\"" + pair.Key.ToString() + "\">" + pair.Value + "</option>");
				}
			}
			CategorySelect.Text = catSelect.ToString();
		}
    }
}