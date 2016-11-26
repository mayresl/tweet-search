function pesquisa(key){
    var url = "http://localhost:50919/api/Twitter/GetTweets?key=" + key;
    $.ajax({
        url: url,
        type: 'GET',
        contentType: 'application/json; charset=utf-8',        
        success: function(response){
            console.log(response);
        },
        error: function(errors){
            console.log(errors);
        }
    });
}