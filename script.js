$(document).ready(function() {
    var win = $(window);

    win.scroll(function() {
        if ($(document).height() - win.height() == win.scrollTop()) {
            $('#loading').show();

            pesquisa($("#search_bar").val(),last);
        }
    });
});

function pesquisa(key, last_id = ""){
    var url = "http://localhost:50919/api/Twitter/GetTweets?key=" + key + "&count=30&last_id=" + last_id;
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',        
        success: function(response){
            renderizarTweets(response);
        },
        error: function(errors){            
            console.log(errors);
        }
    });
}

var last = "";
function renderizarTweets(json){
    var tweets = [];
    tweets = json.statuses;
    var next = json.search_metadata.next_results.split("=");
    last = next[1].slice(0, -2);
    console.log(next[1]);
    console.log(last);
    var element = "";
    for (var i = 0; i < tweets.length; i++){
        element = "<li>" + tweets[i].text + "</li>";
        $("#tweets").append(element);
    }
}

function verificarFim(){
    var win = $(window);
    if ($(document).height() - win.height() == win.scrollTop()){
        $('#loading').show();
        return true;
    } else {
        return false;
    }
}