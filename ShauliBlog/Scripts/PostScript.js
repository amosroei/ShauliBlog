function ShowCommentBtnClick(idSelector) {

    $('#CommentsDiv_' + idSelector.split('_')[1]).removeClass('collapse');

    if ($('#' + idSelector).hasClass("HideBackground")) {
        $('#CommentsDiv_' + idSelector.split('_')[1]).show();
        $('#' + idSelector).removeClass("HideBackground");
        $('#' + idSelector).addClass("ShowBackground");
    }
    else {
        $('#CommentsDiv_' + idSelector.split('_')[1]).hide();
        $('#' + idSelector).removeClass("ShowBackground");
        $('#' + idSelector).addClass("HideBackground");
    }
}

function DeleteComment(commentId) {
    var CommentId = { id: commentId };

    $.ajax({
        type: "POST",
        url: "/Comment/Delete/",
        data: CommentId,
        success: function (data) {
            $('#Comment_' + commentId).addClass('collapse');
        },
        traditional: true,
        error: function (xhr, ajaxOptions, thrownError) {
            alert(thrownError);
        }
    });
}

function NewComment(postId, userName, userId) {

    var div = $('#NewComment_' + postId);              

    var text = div.find('.CommentText').val();

    if (text == null ||
        text.trim() == "") {
        alert("Post Comment: Missng fields")
    }
    else {
        var newComment = {            
            PostId: postId,
            //UserId: userId,
            //CommentAuthor: commentAuthor,
            AccountId: userId,        
            CommentText: text,            
        };

        $.ajax({
            type: "POST",
            url: "/Comment/Create/",
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(newComment),
            traditional: true,
            success: NewCommentSuccess,
            error: NewCommentError
        });
    }
}

function NewCommentSuccess(data) {
    var date = new Date(data.Date);
    //date.setSeconds(0, 0);
    var formattedDateString = date.toLocaleString('en-GB');
    var newCommentElement = "<p id='Comment_" + data.id + "'><b>" + formattedDateString + " - ";
    newCommentElement += data.Account.UserName;
    newCommentElement += "</b> <br>" + data.CommentText +
        "<a onclick='DeleteComment(" + data.id + ")'>" +
        "<img src='../../images/x.png' class='postHeaderImg'/>" +
        "</a>  </p>";

    $('#CommentsDiv_' + data.PostId + ' .CommentsList').prepend(newCommentElement);

    var div = $('#NewComment_' + data.PostId);
    var nameInput = div.find('.CommentAuthor');
    if (nameInput.length != 0) {
        nameInput.val("");
    }
    div.find('.CommentText').val("");
}

function NewCommentError(xhr, ajaxOptions, thrownError) {
    alert(thrownError);
}
