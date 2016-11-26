function pesquisa(key){
    var url = 'https://api.twitter.com/1.1/search/tweets.json?q=';
    url += key;
    var win = window.open(url, '_blank');
    win.focus();
    //alert(key);
}