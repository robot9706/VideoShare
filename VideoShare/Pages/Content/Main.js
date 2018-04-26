window.onload = function ()
{
    document.getElementById('searchTextInput').addEventListener('keyup', function (e) {
        e.preventDefault();

        if (e.keyCode == 13) //Enter
        {
            doSearch();
        }
    });

    document.getElementById('searchButtonInput').addEventListener('click', function () {
        doSearch();
    });
}

function doSearch()
{
    var txt = document.getElementById('searchTextInput').value;

    if (txt != "" && txt.length > 0) {
        document.location = "Search.aspx?s=" + encodeURI(txt);
    }
}

function logOut()
{
    var request = new XMLHttpRequest();
    request.onload = function () {
        window.location.reload();
    }

    request.open("POST", "Services.aspx?f=logout", true);
    request.send(null);
}

function login(username, pwHash, loginOk, loginError)
{
    var request = new XMLHttpRequest();
    request.onload = function () {
        if (this.readyState == 4) {
            if (this.responseText == "ok") {
                loginOk();
            } else {
                loginError(this.responseText);
            }
        }
    }

    var url = encodeURI("Services.aspx?f=login&u=" + username + "&p=" + pwHash);

    request.open("POST", url, true);
    request.send(null);
}

function register(username, pwHash, email, regOk, regError) {
    var request = new XMLHttpRequest();
    request.onload = function () {
        if (this.readyState == 4) {
            if (this.responseText == "ok") {
                regOk();
            } else {
                regError(this.responseText);
            }
        }
    }

    var url = encodeURI("Services.aspx?f=register&u=" + username + "&p=" + pwHash + "&m=" + email);

    request.open("POST", url, true);
    request.send(null);
}