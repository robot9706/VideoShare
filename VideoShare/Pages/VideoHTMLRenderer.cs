using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using VideoShare.Data.Model;

namespace VideoShare.Pages.Renderers
{
    public class VideoHTMLRenderer
    {
        public static void RenderThumbnail(Video video, StringBuilder builder, bool editable = false, Playlist list = null, string topRow = null)
        {
            int views = video.GetViews();
            User uploader = video.GetUploader();

            string deleteFunction = ((list != null) ? "deleteVideoFromList(" + video.ID.ToString() + ", " + list.ID.ToString() + ")" : "deleteVideo(" + video.ID.ToString() + ")");
            string deleteButton = (editable ? "<a style='position: absolute;left:10px;top:5px;cursor:pointer' onclick='" + deleteFunction + "'>X</a>" : string.Empty);

            builder.Append("<div style='width: 185px; margin-left:auto; margin-right: auto'><table style='width: 100%'>");

			if (!string.IsNullOrEmpty(topRow))
			{
				builder.Append("<tr><td colspan='2' style='text-align:center'>");
				builder.Append("<a>" + topRow + "</a>");
				builder.Append("</td></tr>");
			}

            builder.Append("<tr>");
            builder.Append("<td colspan='2' style='text-align: center'><a href='Watch.aspx?v=" + video.ID.ToString() + "'><div style='position: relative'><img style='cursor: pointer' src=\"" + video.GetThumbnailLink() + "\" />" + deleteButton + "</div></td>");
            builder.Append("</tr>");

            builder.Append("<tr>");
            builder.Append("<td style='width: 80%; text-align: left'><a href='Watch.aspx?v=" + video.ID.ToString() + "'>" + video.Title + "</a></td>");
            builder.Append("<td style='width: 20%; text-align: right'><a>" + TimeSpan.FromSeconds(video.Length).ToString("%m\\:ss") + "</a></td>");
            builder.Append("</tr>");

            builder.Append("<tr>");
            builder.Append("<td style='width: 80%; text-align: left'><a href='Profile.aspx?p=" + uploader.Username.ToString() + "'>" + uploader.DisplayName + "</a></td>");
            builder.Append("<td style='width: 20%; text-align: right'><a>" + views .ToString() + "</a></td>");
            builder.Append("</tr>");

            builder.Append("</table></div>");
        }

        public static void RenderBigView(Video video, StringBuilder builder, User loggedInUser, bool functionsAllowed = true)
        {
            int views = video.GetViews();
            User uploader = video.GetUploader();
            string categoryList = video.GetCategoriesListed();

            bool loggedIn = (loggedInUser != null);
            bool isOwner = (loggedInUser != null && loggedInUser.ID == uploader.ID);

            string upvoteHref = (loggedIn ? "href='javascript:doVote(\"upvote\", " + video.ID.ToString() + ")'" : string.Empty);
            string downvoteHref = (loggedIn ? "href='javascript:doVote(\"downvote\", " + video.ID.ToString() + ")'" : string.Empty);

            builder.Append("<div style='width: 800px; margin-left: auto; margin-right: auto; padding-bottom: 10px'>");

            builder.Append("<div style='background-color: #323232'>");
            builder.Append("<video width='800' height='480' controls='controls'><source='" + video.GetContentLink() + "' type='video/mp4' /></video>");
            builder.Append("</div>");

            builder.Append("<table style='width: 100%'>");

            builder.Append("<tr>");
            builder.Append("<td style='width: 80%'><a class='profileText'>" + video.Title + "</a></td>");
            builder.Append("<td style='width: 20%; text-align: right'><a>" + TimeSpan.FromSeconds(video.Length).ToString("%m\\:ss") + "</a></td>");
            builder.Append("</tr>");

            builder.Append("<tr>");
            builder.Append("<td style='width: 80%'><a href='Profile.aspx?p=" + uploader.Username + "' class='profileText'>" + uploader.DisplayName + "</a></td>");
            builder.Append("<td style='width: 20%; text-align: right; vertical-align: center'><a style='padding-left: 10px'>" + views.ToString() + " megtekintés</a></td>");
            builder.Append("</tr>");

            builder.Append("<tr>");
            builder.Append("<td style='width: 80%'><a>Feltöltés: " + video.UploadTime.ToString("yyyy-MM-dd HH:mm:ss") + "</a></td>");
            builder.Append("<td style='width: 20%; text-align: right; vertical-align: center'><a " + upvoteHref + "><img src='Content/upvote.png' /></a><a>" + video.Likes.ToString() + "</a><a " + downvoteHref + "><img src='Content/downvote.png' /></a><a>" + video.Dislikes.ToString() + "</a></td>");
            builder.Append("</tr>");

            if (!string.IsNullOrEmpty(video.Description))
            {
                builder.Append("<tr>");
                builder.Append("<td colspan='2' style='margin-top: 10px'><p>Leírás: " + video.Description + "</p></td>");
                builder.Append("</tr>");
            }

            if (!string.IsNullOrEmpty(categoryList))
            {
                builder.Append("<tr>");
                builder.Append("<td colspan='2' style='margin-top: 10px'><p>Kategória: " + categoryList + "</p></td>");
                builder.Append("</tr>");
            }

            if (functionsAllowed && (isOwner || loggedIn))
            {
                builder.Append("<tr><td colspan='2' style='margin-top: 10px'>");
                if (isOwner)
                {
                    builder.Append("<div onclick='deleteVideo(" + video.ID.ToString() + ")' style='float: left; padding: 10px; background-color: #131313; cursor:pointer; border-radius: 5px'><a>Videó törlése</a></div>");
                }
                if (loggedIn)
                {
                    builder.Append("<div onclick='document.getElementById(\"listAdd\").style.removeProperty(\"visibility\")' style='float: left; margin-left: 2px; padding: 10px; background-color: #131313; cursor:pointer; border-radius: 5px'><a>Hozzáadás listához</a></div>");
                }
                builder.Append("</td></tr>");
            }

            builder.Append("</table>");

            builder.Append("</div>");
        }

		public static void RenderVideoList(StringBuilder html, List<Video> videoList, string title)
		{
			if (videoList == null || videoList.Count == 0)
				return;

			html.Append("<div class='panel' style='padding: 10px'><a class='panelText'>" + title + "</a>");
			{
				RenderVideoListTable(html, videoList);
			}
			html.Append("</div>");
		}

		public static void RenderVideoListTable(StringBuilder html, List<Video> videoList, Func<int, string> TopRowText = null)
		{
			html.Append("<table style='width: 100%'>");
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

								string topRow = null;
								if(TopRowText != null)
								{
									topRow = TopRowText(x);
								}

								RenderThumbnail(videoList[x], html, false, null, topRow);

								html.Append("</td>");
							}
						}
					}
					html.Append("</tr>");
				}
			}
			html.Append("</table>");
		}
	}
}