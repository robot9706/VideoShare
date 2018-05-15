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
                case "upload":
                    UploadVideo(req);
                    break;
                case "view":
                    OnVideoView(req);
                    break;
                case "newlist":
                    CreatePlaylist(req);
                    break;
                case "dellist":
                    DeletePlaylist(req);
                    break;
                case "delvid":
                    DeleteVideo(req);
                    break;
                case "delvidfromlist":
                    DeleteVideoFromList(req);
                    break;
                case "addvidtolist":
                    AddVideoToList(req);
                    break;
				case "catview":
					GenerateCategoryViewHTML(req);
					break;
				case "dateview":
					GenerateDateViewHTML(req);
					break;
				case "udesc":
					UpdateUserDescription(req);
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

        private void UploadVideo(HttpRequest request)
        {
            if (request.Params["t"] == null || request.Params["d"] == null || request.Params["c"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            if (Session["User"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            string[] catList = request.Params["c"].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if(catList.Length == 0)
            {
                Response.Write("Hiba!");
                return;
            }

            string title = HttpUtility.UrlDecode(request.Params["t"]);
            string desc = HttpUtility.UrlDecode(request.Params["d"]);

            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(desc))
            {
                Response.Write("Hiba!");
                return;
            }

            Video video = new Data.Model.Video();
            video.Title = title;
            video.Description = desc;
            video.Likes = 0;
            video.Dislikes = 0;
            video.Length = 50;
            video.UploadTime = DateTime.Now;
            video.Uploader = ((User)Session["User"]).ID;

            if (Global.Database.Insert<Video>(video))
            {
                int videoID = Global.Database.GetSequenceCurrentID<Video>();

                foreach (string ct in catList)
                {
                    int cid;
                    if(!Int32.TryParse(ct, out cid))
                    {
                        continue;
                    }

                    VideoCategory newCat = new Data.Model.VideoCategory();
                    newCat.VideoID = videoID;
                    newCat.CategoryID = cid;

                    Global.Database.Insert<VideoCategory>(newCat);
                }

                Response.Write("ok");
            }
            else
            {
                Response.Write("Hiba!");
            }
        }

        private void OnVideoView(HttpRequest request)
        {
            if (request.Params["v"] == null)
            {
                return; //Nincs hiba
            }

            int id = 0;
            if (!Int32.TryParse(request.Params["v"], out id))
            {
                return;
            }

            Video v = Video.FindVideo(id);
            if (v == null)
            {
                return;
            }

            Data.Model.View.AddView(v);

			if (Session["User"] != null) //If a user is logged in
			{
				User user = (User)Session["User"];

				UserView.UpdateUserViews(user, v);
			}
        }

        private void CreatePlaylist(HttpRequest req)
        {
            if (req.Params["t"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            if (Session["User"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            if (string.IsNullOrEmpty(req.Params["t"]))
            {
                Response.Write("Hiba!");
                return;
            }

            Playlist pl = new Playlist();
            pl.Title = req.Params["t"];
            pl.Creator = ((User)Session["User"]).ID;
            pl.CreationDate = DateTime.Now;

            if (Global.Database.Insert<Playlist>(pl))
            {
                Response.Write("ok");
            }
            else
            {
                Response.Write("Hiba!");
            }
        }

        private void DeletePlaylist(HttpRequest req)
        {
            if (req.Params["l"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int id;
            if (!Int32.TryParse(req.Params["l"], out id))
            {
                Response.Write("Hiba!");
                return;
            }

            Playlist list = Playlist.Find(id);
            if (list == null)
            {
                Response.Write("Hiba!");
                return;
            }

            list.Delete();

            Response.Write("ok");
        }

        private void DeleteVideo(HttpRequest req)
        {
            if (req.Params["v"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int id;
            if (!Int32.TryParse(req.Params["v"], out id))
            {
                Response.Write("Hiba!");
                return;
            }

            Video video = Video.FindVideo(id);
            if (video == null)
            {
                Response.Write("Hiba!");
                return;
            }

            video.Delete();

            Response.Write("ok");
        }

        private void DeleteVideoFromList(HttpRequest req)
        {
            if (req.Params["v"] == null || req.Params["l"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int vid;
            if (!Int32.TryParse(req.Params["v"], out vid))
            {
                Response.Write("Hiba!");
                return;
            }

            Video video = Video.FindVideo(vid);
            if (video == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int lid;
            if (!Int32.TryParse(req.Params["l"], out lid))
            {
                Response.Write("Hiba!");
                return;
            }

            Playlist list = Playlist.Find(lid);
            if (list == null)
            {
                Response.Write("Hiba!");
                return;
            }

            list.DeleteFromList(video);

            Response.Write("ok");
        }

        private void AddVideoToList(HttpRequest req)
        {
            if (req.Params["v"] == null || req.Params["l"] == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int vid;
            if (!Int32.TryParse(req.Params["v"], out vid))
            {
                Response.Write("Hiba!");
                return;
            }

            Video video = Video.FindVideo(vid);
            if (video == null)
            {
                Response.Write("Hiba!");
                return;
            }

            int lid;
            if (!Int32.TryParse(req.Params["l"], out lid))
            {
                Response.Write("Hiba!");
                return;
            }

            Playlist list = Playlist.Find(lid);
            if (list == null)
            {
                Response.Write("Hiba!");
                return;
            }

            PlaylistContent content = new Data.Model.PlaylistContent();
            content.PlaylistID = list.ID;
            content.VideoID = video.ID;

            if (Global.Database.Insert<PlaylistContent>(content))
            {
                Response.Write("ok");
            }
            else
            {
                Response.Write("Hiba!");
            }
        }

		private void GenerateCategoryViewHTML(HttpRequest req)
		{
			if (req.Params["c"] == null || req.Params["t"] == null)
			{
				Response.Write("Hiba!");
				return;
			}

			string cat = req.Params["c"];
			string filterText = req.Params["t"];

			int? categoryID = null;
			int parse;
			if (cat != "all" && Int32.TryParse(cat, out parse))
			{
				categoryID = parse;
			}

			VideoQuery.CategoryFilterType filter = VideoQuery.CategoryFilterType.Time;
			Enum.TryParse<VideoQuery.CategoryFilterType>(filterText, out filter);

			List<Video> videos = VideoQuery.GetFilteredCategoryVideos(categoryID, filter, 6);

			StringBuilder html = new StringBuilder();
			VideoHTMLRenderer.RenderVideoListTable(html, videos);

			Response.Clear();

			Response.StatusCode = 200;
			Response.Write(html.ToString());
		}

		private void GenerateDateViewHTML(HttpRequest req)
		{
			if (req.Params["t"] == null)
			{
				Response.Write("Hiba!");
				return;
			}

			string filterText = req.Params["t"];

			VideoQuery.DateFilterType filter = VideoQuery.DateFilterType.Day;
			Enum.TryParse<VideoQuery.DateFilterType>(filterText, out filter);

			List<Video> videos = VideoQuery.GetDatedCategoryVideos(filter, 6);

			DateTime today = DateTime.Now.Date;
			List<string> lookupNames = new List<string>(6);
			switch (filter)
			{
				case VideoQuery.DateFilterType.Day:
					lookupNames.Add("Ma");
					lookupNames.Add("Tegnap");
					lookupNames.Add(today.AddDays(-2).ToString("yyyy-MM-dd"));
					lookupNames.Add(today.AddDays(-3).ToString("yyyy-MM-dd"));
					lookupNames.Add(today.AddDays(-4).ToString("yyyy-MM-dd"));
					lookupNames.Add(today.AddDays(-5).ToString("yyyy-MM-dd"));
					break;
				case VideoQuery.DateFilterType.Week:
					for (int x = 0; x < 6; x++)
					{
						lookupNames.Add(today.AddDays(-7 * x).ToString("yyyy-MM-dd"));
					}
					break;
				case VideoQuery.DateFilterType.Month:
					for (int x = 0; x < 6; x++)
					{
						lookupNames.Add(today.AddMonths(-x).ToString("yyyy-MM-dd"));
					}
					break;
			}

			StringBuilder html = new StringBuilder();
			VideoHTMLRenderer.RenderVideoListTable(html, videos, new Func<int, string>((int index) =>
			{
				return lookupNames[index];
			}));

			Response.Clear();

			Response.StatusCode = 200;
			Response.Write(html.ToString());
		}

		private void UpdateUserDescription(HttpRequest request)
		{
			if (request.Params["d"] == null)
			{
				Response.Write("Hiba!");
				return;
			}

			if (Session["User"] == null)
			{
				Response.Write("Hiba!");
				return;
			}

			string desc = HttpUtility.UrlDecode(request.Params["d"]);

			User user = (User)Session["User"];
			user.Info = desc;

			if (Global.Database.Update<User>(user))
			{
				Response.Write("ok");
			}
			else
			{
				Response.Write("Hiba az adatok frissítése közben!");
			}
		}
	}
}