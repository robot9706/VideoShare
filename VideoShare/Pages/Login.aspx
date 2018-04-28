<%@ Page Title="" Language="C#" MasterPageFile="~/Pages/Master.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VideoShare.Pages.Login" %>
<asp:Content ID="HeadContent" ContentPlaceHolderID="head" runat="server">
    <script src="Content/md5.js"></script>
    <script>
        var ll = window.onload;
        window.onload = function()
        {
            if (ll != null) ll();

            document.getElementById('loginBtn').addEventListener('click', function () {
                var uname = document.getElementById('username').value;
                var upw = document.getElementById('password').value;

                if (uname == "" || uname.length == 0) {
                    document.getElementById("errTxt").innerHTML = 'Kérem adja meg a felhasználó nevét!';
                    return;
                }
                if (upw == "" || upw.length == 0) {
                    document.getElementById("errTxt").innerHTML = 'Kérem adja meg a jelszavát!';
                    return;
                }

                upw = md5(upw);

                document.getElementById("errTxt").innerHTML = '';
                login(uname, upw, callback_LoginOk, callback_LoginError);
            });

            document.getElementById('regUserBtn').addEventListener('click', function () {
                var uname = document.getElementById('regUsername').value;
                var uemail = document.getElementById('regEmail').value;
                var upw1 = document.getElementById('regPassword').value;
                var upw2 = document.getElementById('regPassword2').value;

                var errTxt = document.getElementById('regErrTxt');
                errTxt.innerHTML = '';

                if (uname == "" || uname.length == 0) {
                    errTxt.innerHTML = 'Kérem adja meg a felhasználó nevét!';
                    return;
                }
                if (uemail == "" || uemail.length == 0) {
                    errTxt.innerHTML = 'Kérem adja meg email címét!';
                    return;
                }
                if (upw1 == "" || upw1.length == 0) {
                    errTxt.innerHTML = 'Kérem adja meg a jelszavát!';
                    return;
                }
                if (upw2 == "" || upw2.length == 0) {
                    errTxt.innerHTML = 'Kérem adja meg a jelszavát!';
                    return;
                }

                if (upw1 != upw2) {
                    errTxt.innerHTML = 'A jelszavak nem egyeznek!';
                    return;
                }

                upw1 = md5(upw1);

                register(uname, upw1, uemail, callback_RegOk, callback_RegError);
            });

            document.getElementById('regBtn').addEventListener('click', function () {
                document.getElementById('loginPanel').style.visibility = "hidden";
                document.getElementById('regPanel').style.removeProperty("visibility");
            });

            document.getElementById('loginBtn2').addEventListener('click', function () {
                document.getElementById('loginPanel').style.removeProperty("visibility");
                document.getElementById('regPanel').style.visibility = "hidden";
            });
        }

        function callback_LoginOk()
        {
            window.location.href = "Index.aspx";
        }

        function callback_LoginError(err)
        {
            document.getElementById("errTxt").innerHTML = err;
        }
        
        function callback_RegOk()
        {
            window.location.href = "Index.aspx";
        }

        function callback_RegError(err)
        {
            document.getElementById("regErrTxt").innerHTML = err;
        }
    </script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="body" runat="server">
    <div style="margin-top: 100px;">

        <div id="loginPanel" style="background-color: #434343; border-radius: 5px; position: absolute; margin-left: auto; margin-right: auto; width: 400px; left: 0; right: 0">
            <table style="width: 100%; padding: 10px">
                <tr>
                    <td style="text-align: center"><a style="font-size: large">Bejelentkezés</a></td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px">
                        <div style="background-color: #131313; padding: 10px; border-radius: 5px">
                            <input id="username" type="text" class="hiddenInput" style="width: 100%" placeholder="Felhasználónév" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px;">
                        <div style="background-color: #131313; padding: 10px; border-radius: 5px">
                            <input id="password" type="password" class="hiddenInput" style="width: 100%" placeholder="Jelszó" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px;">
                        <div style="text-align: center">
                            <a id="errTxt" style="font-size: large; color: red;"></a>
                        </div>
                        <div id="loginBtn" style="padding: 10px; background-color: #131313; cursor:pointer; width: 50%; margin-left: auto; margin-right: auto; border-radius: 5px">
                            <a>Bejelentkezés</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px;">
                        <div id="regBtn" style="padding: 10px; background-color: #131313; cursor:pointer; width: 50%; margin-left: auto; margin-right: auto; border-radius: 5px">
                            <a>Új felhasználó</a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="regPanel" style="background-color: #434343; border-radius: 5px; visibility: hidden; position: absolute; margin-left: auto; margin-right: auto; width: 400px; left: 0; right: 0">
            <table style="width: 100%; padding: 10px;">
                <tr>
                    <td style="text-align: center"><a style="font-size: large">Regisztráció</a></td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px">
                        <div style="background-color: #131313; padding: 10px; border-radius: 5px">
                            <input id="regUsername" type="text" class="hiddenInput" style="width: 100%" placeholder="Felhasználónév" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px">
                        <div style="background-color: #131313; padding: 10px; border-radius: 5px">
                            <input id="regEmail" type="email" class="hiddenInput" style="width: 100%" placeholder="Email" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px;">
                        <div style="background-color: #131313; padding: 10px; border-radius: 5px">
                            <input id="regPassword" type="password" class="hiddenInput" style="width: 100%" placeholder="Jelszó" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px;">
                        <div style="background-color: #131313; padding: 10px; border-radius: 5px">
                            <input id="regPassword2" type="password" class="hiddenInput" style="width: 100%" placeholder="Jelszó újra" required="required" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px;">
                        <div style="text-align: center">
                            <a id="regErrTxt" style="font-size: large; color: red;"></a>
                        </div>
                        <div id="regUserBtn" style="padding: 10px; background-color: #131313; cursor:pointer; width: 50%; margin-left: auto; margin-right: auto; border-radius: 5px">
                            <a>Regisztráció</a>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 10px;">
                        <div id="loginBtn2" style="padding: 10px; background-color: #131313; cursor:pointer; width: 50%; margin-left: auto; margin-right: auto; border-radius: 5px">
                            <a>Már van fiókom</a>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
