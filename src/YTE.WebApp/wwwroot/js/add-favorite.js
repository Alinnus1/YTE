function addToFavorite(id){
    var data = {id: id };
    $.ajax({
        type: "POST",
        url: '/FavoriteList/Add/',
        data: data,
        success: function(resultData) {
            // take the result data and update the div
            location.reload();
        }
        
      });
}

function removeFromFavorite(id) {
    var data = { id: id };
    $.ajax({
        type: "POST",
        url: '/FavoriteList/Remove/',
        data: data,
        success: function (resultData) {
            // take the result data and update the div
            location.reload();
        }

    });
}

function addToWatchList(id) {
    var data = { id: id };
    $.ajax({
        type: "POST",
        url: '/WatchList/Add/',
        data: data,
        success: function (resultData) {
            // take the result data and update the div
            location.reload();
        }

    });
}

function removeFromWatchList(id) {
    var data = { id: id };
    $.ajax({
        type: "POST",
        url: '/WatchList/Remove/',
        data: data,
        success: function (resultData) {
            // take the result data and update the div
            location.reload();
        }

    });
}