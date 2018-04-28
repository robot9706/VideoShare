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
	public partial class Search : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (Request.Params["s"] == null)
			{
				Response.Redirect("Index.aspx");
				return;
			}

			string query = Request.Params["s"];
			List<Video> list = VideoQuery.Search(query);

			if (list == null)
			{
				Response.Redirect("Index.aspx");
				return;
			}

			StringBuilder html = new StringBuilder();

			Render(html, list, "Keresés: \"" + query + "\"");

			ContentHTML.Text = html.ToString();
		}

		private void Render(StringBuilder html, List<Video> videoList, string title)
		{
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