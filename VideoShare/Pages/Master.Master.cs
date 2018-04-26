﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VideoShare.Data.Objects;

namespace VideoShare.Pages
{
    public partial class Master : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RenderUserLogin();
        }

        private void RenderUserLogin()
        {
            if (Session["User"] == null)
            {
                UserLogin.Text = "<div style='width: 100%; text-align:right'><a class='userBtn' href='Login.aspx'>Bejelentkezés/Regisztráió</a></div>";
            }
            else
            {
                WebsiteUser user = (WebsiteUser)Session["User"];

                UserLogin.Text = "<table style='width: 100%'><tr><td style='text-align: right'><a class='userBtn' href='Profile.aspx'>" + user.DisplayName + "</a></td><td><img src='Content/logout.png' style='cursor:pointer' onclick='logOut()' /></td></tr></table>";
            }
        }
    }
}