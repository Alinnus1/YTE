$(document).ready(function(){
    var id = window.location.href.toString().split('/')[5];
    $("#list").attr('href', "/ArtReview/ListReviewsOfArt/"+id);
})