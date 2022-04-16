$(document).ready(function () {
    $.ajax({
        method: 'GET',
        url: "/Genre/Show",
        data: 'json',
    
    }).done(function (data) {
        // console.log(data);
        var row = "";
        $("#genre-dropdown").empty();
        $.each(data,function(i,v){
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#genre-dropdown").html(row);
    });
    
});

