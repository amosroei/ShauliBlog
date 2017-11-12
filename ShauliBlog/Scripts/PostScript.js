// deletes post by the given id
function DeletePost(postId) {
    var PostId = { postId: postId };

    // checks if the desired post exists
    $.ajax({
        type: "GET",
        url: "/Post/CheckEntityExist/",
        data: PostId,
        success: function (isExist) {
            // if not, notifies the user
            if (isExist == "False") {
                alert('הפוסט שביקשת למחוק לא נמצא, מתבצע רענון של הנתונים...');                
                location.reload();

            }
            // if exists, deletes the post
            else {
                $.ajax({
                    type: "POST",
                    url: "/Post/Delete/",
                    data: PostId,
                    success: function (data) {
                        location.reload();                       
                    },
                    traditional: true,
                    error: function (xhr, ajaxOptions, thrownError) {
                        alert(thrownError);
                    }
                });
            }            
        },
        traditional: true,
        error: function (xhr, ajaxOptions, thrownError) {
            //alert(thrownError);
        }
    });
}


// opens the comments section
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

// delets the comment
function DeleteComment(commentId) {
    var CommentId = { id: commentId };

    $.ajax({
        type: "POST",
        url: "/Comment/Delete/",
        data: CommentId,
        success: function (response) {
            if (response.success) {
                $('#Comment_' + commentId).addClass('collapse');
            }
        },
        traditional: true      
    });
}

// creates a new comment
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

// if comment created successfully, updates the html accordingly
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


// Checks for Apriori suggestions on space pressed inside comment text
$(document).ready(function () {
    // select all textarea controls
    var textareaControls = $("[name='textareaField']");

    for (var i = 0; i < textareaControls.length; i++) {

        //current textarea element
        var elem = textareaControls[i];

        // add keypress listener to current element
        elem.addEventListener("keypress", function (value) {

            // saves this is variable
            var currControl = this;

            var controlIndex = currControl.id.split("_").pop();

            // checks if current key is space
            if (value.which == 32) {
                var txtComment = $("#text_" + controlIndex).val();

                // checks if comment text is not empty
                if (txtComment.length != 0) {
                    $.post("/AprioriAlgorithm/checkForAprioriSuggestions", { comment: txtComment }, function (data) {

                        // sets the matching apriorisuggestions control to the returned value
                        $('#AprioriSuggestions_' + controlIndex).text(data.comment);
                    });
                };
            }
        });
    }
});
