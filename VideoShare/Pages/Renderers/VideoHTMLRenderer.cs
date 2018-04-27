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
        public static void RenderThumbnail(Video video, StringBuilder builder)
        {
            int views = video.GetViews();
            User uploader = video.GetUploader();

            builder.Append("<div style='width: 185px; margin-left:auto; margin-right: auto'><table style='width: 100%'>");

            builder.Append("<tr>");
            builder.Append("<td colspan='2' style='text-align: center'><a href='Watch.aspx?v=" + video.ID.ToString() + "'><img style='cursor: pointer' src=\"" + video.GetThumbnailLink() + "\" /></a></td>");
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

        public static void RenderBigView(Video video, StringBuilder builder, bool loggedIn)
        {
            int views = video.GetViews();
            User uploader = video.GetUploader();
            string categoryList = video.GetCategoriesListed();

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

            builder.Append("</table>");

            builder.Append("</div>");
        }
    }
}