using System;
using System.Collections.Generic;
using VideoShare.Data;
using VideoShare.Data.Model;

namespace VideoShare
{
    public class Global : System.Web.HttpApplication
    {
        public static Database Database;
        public static bool DatabaseConnected;

        public static Dictionary<int, string> VideoCategories = new Dictionary<int, string>();

        protected void Application_Start(object sender, EventArgs e)
        {
            Database = new Database();
            DatabaseConnected = Database.Open();

            if (DatabaseConnected)
            {
                List<Category> cList = Database.SelectAll<Category>();

                foreach (Category c in cList)
                {
                    if (VideoCategories.ContainsKey(c.ID))
                        continue;

                    VideoCategories.Add(c.ID, c.Name);
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (Database != null)
            {
                Database.Close();
            }
        }
    }
}