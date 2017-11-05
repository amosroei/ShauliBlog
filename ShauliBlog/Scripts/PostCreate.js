$(function () {
    var VideoCheck = $("#VideoCheck");
    var ImageCheck = $("#ImageCheck");
    var hiddenVideo = $("#hiddenVideo");
    var hiddenImage = $("#hiddenImage");

    // hides video and image input field
    hiddenVideo.hide();
    hiddenImage.hide();

    // on change, shows or hide the input field
    VideoCheck.change(function () {
        if (VideoCheck.is(':checked')) {
            hiddenVideo.show();
        } else {
            hiddenVideo.hide();
        }
    });

    // on change, shows or hide the input field
    ImageCheck.change(function () {
        if (ImageCheck.is(':checked')) {
            hiddenImage.show();
        } else {
            hiddenImage.hide();
        }
    });
});