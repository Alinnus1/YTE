$(document).ready(function(){

    var dropdown = $('#art-type-dropdown')
    dropdown.change(function(){
        var divul = $('#specific-fields');
        var val = dropdown.val();
        if(val == 1){
            $('form').attr('action',"/ArtObject/Create");
            divul.empty();
        }else if (val == 4){
            $.ajax({
               method: 'GET',
               url: "/Film/Attributes",
               data: 'json',
            }).done(function (data){
                $('form').attr('action',"/Film/Create");
                var row = '';
                
                $.each(data,function(i,v){
                    var type = 'text';
                    var defaultVal = '';
                    if(v === 'Length'){
                        defaultVal = '00:00:00';
                    }
                    row += '<label class="control-label" for="'+ v + '"> '+v+ '</label>';
                    
                    row+= '<input class="form-control" type="'+type +'" id="'+v+ '"name="'+v+ '" value="'+defaultVal+'">';

                    row+= '<span asp-validation-for="'+v+'"></span>';
               });
               divul.html(row);
               $('#Length').change(function(){
                debugger;
                var inputField = document.querySelector('#Length');

                var isValid = /^([01][0-9]|2[0-3]):([0-5][0-9]):([0-5][0-9])$/.test(inputField.value);
        
                if (isValid) {
                inputField.style.backgroundColor = '';
                
                
                } else {
                inputField.value = '00:00:00'
                    
                }
            })
           })
        }else if (val ==5){
            $.ajax({
                method: 'GET',
                url: "/Manga/Attributes",
                data: 'json',
            }).done(function (data){
                $('form').attr('action',"/Manga/Create");
                var row = '';

                $.each(data,function(i,v){
                    var type = 'number';
                    var defaultVal = '';
                    if(v === 'IsFinished'){
                        type = 'checkbox';
                    }
                    row += '<label class="control-label" for="'+ v + '"> '+v+ '</label>';
                    
                    row+= '<input class="form-control" type="'+type +'" id="'+v+ '"name="'+v+ '" value="'+defaultVal+'">';

                    row+= '<span asp-validation-for="'+v+'"></span>';
                });

                divul.html(row);
                $('#NoChapters').attr('min','1');
                $('#NoChapters').attr('max','10000');
                $('#NoVolumes').attr('min','1');
                $('#NoVolumes').attr('max','10000');

            })
        }else if (val ==8){
            $.ajax({
                method:'GET',
                url:"/VideoGame/Attributes",
                data: 'json',
            }).done(function (data){
                $('form').attr('action',"/VideoGame/Create");
                var row= '';

                $.each(data,function(i,v){
                    var type = 'text';
                    if(v === 'Multiplayer'){
                        type = 'checkbox';
                    }
                    row += '<label class="control-label" for="'+ v + '"> '+v+ '</label>';
                    
                    row+= '<input class="form-control" type="'+type +'" id="'+v+ '"name="'+v+ '" value="">';

                    row+= '<span asp-validation-for="'+v+'"></span>';
                });
                divul.html(row);
                
            })
        }
    });

    
})