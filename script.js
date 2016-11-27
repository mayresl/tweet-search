$(document).ready(function () {
    var win = $(window);

    win.scroll(function () {
        if ($(document).height() - win.height() == win.scrollTop() && $("#search_bar").val()) {
            $('#loading').show();

            pesquisa($("#search_bar").val(), last);
        }
    });

    $("#go").click(function () {
        $('#loading').hide();
        $("#status").hide();
        $("#tweets").html('');
    });

    $("#search_bar").keyup(function () {
        if (!this.value) {
            $('#loading').hide();
            $("#status").hide();
            $("#tweets").html('');
        }
    });
});

function pesquisa(key, last_id = "") {
    key = key.replace(new RegExp('@', 'g'), "%40");
    key = key.replace(new RegExp('#', 'g'), "%23");
    key = key.replace(new RegExp(' ', 'g'), "+");
    var url = "http://localhost:50919/api/Twitter/GetTweets?key=" + key + "&count=30&last_id=" + last_id;
    console.log(url);
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',
        success: function (response) {
            renderizarTweets(response);
        },
        error: function (errors) {
            trataErro(errors);
        }
    });
}

var last = "";

function renderizarTweets(json) {
    if (json.statuses) {
        if (!((json.search_metadata.next_results) || (json.search_metadata.refresh_url))) {
            trataErro(json);
        } else {
            var tweets = json.statuses;
            if (tweets.length > 0) {
                if (json.search_metadata.next_results) {
                    var next = json.search_metadata.next_results.split("=");
                } else {
                    var next = json.search_metadata.refresh_url.split("=");
                }
                last = next[1].slice(0, -2);
                var element = "";
                for (var i = 0; i < tweets.length; i++) {
                    element = "<a href='https://twitter.com/statuses/" + tweets[i].id_str + "' class='list-group-item' target='_blank'>";
                    element += "<div class='media'>";
                    element += "<div class='media-left'>";
                    element += "<img class='media-object' src='" + tweets[i].user.profile_image_url_https + "'/>";
                    element += "</div>";
                    element += "<div class='media-body'>";
                    element += "<h4 class='media-heading'>" + tweets[i].user.name + " <small>@" + tweets[i].user.screen_name + "</small></h4>";
                    element += tweets[i].text;
                    element += "</div>";
                    element += "</div>";
                    element += "</a>";
                    $("#tweets").append(element);
                }
                $('#loading').hide();
            } else {
                $("#status").text('Parece que não existem tweets sobre isso :(');
                $("#status").show();
            }
        }
    } else {
        trataErro(json);
    }
}

function trataErro(json) {
    console.info("Retorno JSON: ", json);
    $("#status").show();
    $("#status").text('Ops!... Ocorreu um erro.');
}