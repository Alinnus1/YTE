$(document).ready(function () {
    $.ajax({
        method: 'GET',
        url: "/Gender/Show",
        data: 'json',
    
    }).done(function (data) {
        // console.log(data);
        var row = "";
        $("#gender-dropdown").empty();
        $.each(data,function(i,v){
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#gender-dropdown").html(row);
    });
    
});

