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
				Render(html, videos, "Ajánlott");
			}
			else
			{
				//Ajánlás
				videos = VideoQuery.GetRandomVideos(6);
				Render(html, videos, "Ajánlott");
			}

			//Legújabb videók
			videos = VideoQuery.GetLatestVideos(6);
			Render(html, videos, "Legújabb videók");

			//Legaktívabb feltöltő videói
			videos = VideoQuery.GetMostActiveUploaderVideos(6);
			Render(html, videos, "Legaktívabb feltöltő videói");

			//Legaktívabb kommentelő videói
			videos = VideoQuery.GetMostActiveCommenterVideos(6);
			Render(html, videos, "Legaktívabb kommentelő videói");

			ContentHTML.Text = html.ToString();
		}

		private void Render(StringBuilder html, List<Video> videoList, string title)
		{
			if (videoList == null || videoList.Count == 0)
				return;

			html.Append("<div class='panel' style='padding: 10px'><a class='panelText'>" + title + "</a><table style='width: 100%'>");
			{
				int rowCount = (int)Math.Ceiling((double)((float)videoList.Count / 6));

				for (int row = 0; row < rowCount; row++)
				{
					int offset = row * 6;

					html.Append("<tr>");
					{
						for (int x = offset; x < offset + 6; x++)
						{
							if (x >= videoList.Count)
							{
								html.Append("<td style='width: 16%'></td>");
							}
							else
							{
								html.Append("<td style='width: 16%'>");

								VideoHTMLRenderer.RenderThumbnail(videoList[x], html);

								html.Append("</td>");
							}
						}
					}
					html.Append("</tr>");
				}
			}
			html.Append("</table></div>");
		}
    }
}