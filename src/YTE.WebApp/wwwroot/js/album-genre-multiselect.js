$(document).ready(function () {
    $.ajax({
        method: 'GET',
        url: "/Genre/ShowAlbumGenres",
        data: 'json',

    }).done(function (data) {
        var id = window.location.href.toString().split('/')[5];
        var row = "";
        var select = $("#Genre");
        select.empty();
        $.each(data,function(i,v){
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        select.html(row);

        $.ajax({
            method: 'GET',
            url: "/Genre/ShowSpecificAlbumGenres/"+ id,
            data: 'json'
        }).done(function(data){

            var selectedOptions = [];
            $.each(data,function(i,v){
                selectedOptions.push(v.value);
            });
            $("#Genre > option").each(function() {
                if(selectedOptions.includes(this.value)){
                    $(this).attr('selected', 'selected');
                }
            });
        });
    });

});
