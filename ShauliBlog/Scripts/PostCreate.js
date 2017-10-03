$(function () {
    var VideoCheck = $("#VideoCheck");
    var ImageCheck = $("#ImageCheck");
    var hiddenVideo = $("#hiddenVideo");
    var hiddenImage = $("#hiddenImage");
    hiddenVideo.hide();
    hiddenImage.hide();
    VideoCheck.change(function () {
        if (VideoCheck.is(':checked')) {
            hiddenVideo.show();
        } else {
            hiddenVideo.hide();
        }
    });
    ImageCheck.change(function () {
        if (ImageCheck.is(':checked')) {
            hiddenImage.show();
        } else {
            hiddenImage.hide();
        }
    });
});