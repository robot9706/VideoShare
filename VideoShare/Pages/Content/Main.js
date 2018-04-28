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

function register(username, pwHash, email, regOk, regError)
{
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

var voted = false;

function doVote(mode, video)
{
    if (voted)
        return;

    voted = true;

    var request = new XMLHttpRequest();

    var url = encodeURI("Services.aspx?f=" + mode + "&v=" + video);

    request.open("POST", url, true);
    request.send(null);
}

function doComment(vid)
{
    var c = document.getElementById('commentText').value;
    if (c == "" || c.length == 0)
        return;

    var request = new XMLHttpRequest();
    request.onload = function () {
        if (this.readyState == 4) {
            if (this.responseText == "ok") {
                window.location.reload();
            } else {
                alert(this.responseText);
            }
        }
    }

    var url = encodeURI("Services.aspx?f=comment&v=" + vid + "&c=" + c);

    request.open("POST", url, true);
    request.send(null);
}

function onVideoWatched(video)
{
    var request = new XMLHttpRequest();
    var url = encodeURI("Services.aspx?f=view&v=" + video);

    request.open("POST", url, true);
    request.send(null);
}

function deleteVideo(id) {
    var url = "Services.aspx?f=delvid&v=" + id;

    url = encodeURI(url);

    var request = new XMLHttpRequest();
    request.onload = function () {
        if (this.readyState == 4) {
            if (this.responseText == "ok") {
                window.location.reload();
            } else {
                alert(this.responseText);
            }
        }
    }

    request.open("POST", url, true);
    request.send(null);
}

function deleteVideoFromList(video, list) {
    var url = "Services.aspx?f=delvidfromlist&v=" + video + "&l=" + list;

    url = encodeURI(url);

    var request = new XMLHttpRequest();
    request.onload = function () {
        if (this.readyState == 4) {
            if (this.responseText == "ok") {
                window.location.reload();
            } else {
                alert(this.responseText);
            }
        }
    }

    request.open("POST", url, true);
    request.send(null);
}

function addVideoToList(video, list)
{
    var url = "Services.aspx?f=addvidtolist&v=" + video + "&l=" + list;

    url = encodeURI(url);

    var request = new XMLHttpRequest();
    request.onload = function () {
        if (this.readyState == 4) {
            if (this.responseText == "ok") {
                window.location.reload();
            } else {
                alert(this.responseText);
            }
        }
    }

    request.open("POST", url, true);
    request.send(null);
}