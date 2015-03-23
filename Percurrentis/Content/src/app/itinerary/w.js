function getNext() {
    // get single item from some JSON api

    /*$.getJSON("http://api.icndb.com/jokes/random", function (data) {
        $("#putithere").html("");
        $.each(data, function (key, val) {
            if (val.joke != undefined) {
                $("#putithere").html(val.joke);
            }
        });
    });*/

    $.ajax({
        url: "http://clientsfromhell.net/post/1682478108/oh-you-mean-the-web-you-said-the-internet-and",
        dataType: 'text',
        success: function (data) {
            console.log(data);
            /*var elements = $("<div>").html(data)[0].getElementsByTagName("ul")[0].getElementsByTagName("li");
            for (var i = 0; i < elements.length; i++) {
                var theText = elements[i].firstChild.nodeValue;
                // Do something here
            }*/
        }
    });
}