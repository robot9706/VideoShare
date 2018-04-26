using VideoShare.Data.Model;

namespace VideoShare.Data.Objects
{
    public class WebsiteUser
    {
        private User _userData;

        public string DisplayName
        {
            get { return _userData.DisplayName; }
        }

        public WebsiteUser(User user)
        {
            _userData = user;
        }

        public static WebsiteUser LoginUser(string name, string pw)
        {
            User data = User.FindUser(name, pw);

            if (data == null)
                return null;

            return new WebsiteUser(data);
        }

        public static WebsiteUser Register(string name, string mail, string pw)
        {
            if (User.CheckName(name))
                return null;

            User newUser = User.Register(name, mail, pw);

            if (newUser == null)
                return null;

            return new WebsiteUser(newUser);
        }
    }
}