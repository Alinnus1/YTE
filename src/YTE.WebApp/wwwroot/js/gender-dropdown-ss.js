
$(document).ready(function () {
    $.ajax({
        method: 'GET',
        url: "/Gender/Show",
        data: 'json',
    
    }).done(function (data) {

        var id = $("#Id.lol[type='hidden']").val();
        var row = "";
        $("#gender-dropdown").empty();
        $.each(data,function(i,v){
            row += "<option value=" + v.value + ">" + v.text + "</option>";
        });
        $("#gender-dropdown").html(row);
        
        $.ajax({
            method: 'GET',
            url: "/Gender/Specific/"+id,
            data: 'json'
        }).done(function(data){
            var selectedOption  = [];
            $.each(data,function(i,v){
                selectedOption.push(v.value);
            });
            $("#gender-dropdown > option").each(function(){
                if(selectedOption.includes(this.value)){
                    $(this).attr('selected','selected');
                }
            });
        });
    });
    
});

