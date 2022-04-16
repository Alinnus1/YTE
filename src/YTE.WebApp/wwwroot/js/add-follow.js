function addToFollow(id){
    var data = {id: id };
    $.ajax({
        type: "POST",
        url: '/FollowList/Add/',
        data: data,
        success: function(resultData) {
            location.reload();
        }
        
      });
}

function removeFromFollow(id) {
    var data = { id: id };
    $.ajax({
        type: "POST",
        url: '/FollowList/Remove/',
        data: data,
        success: function (resultData) {
            // take the result data and update the div
            location.reload();
        }

    });
}

