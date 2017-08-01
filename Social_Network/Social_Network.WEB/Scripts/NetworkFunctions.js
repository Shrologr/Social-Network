function UsersAjaxCall(divid) {
    $.ajax({
        type: "POST",
        url: '/User/UserList/',
        success: function (data) {
            $(divid).html(data)
        }
    })
}

function PostsAjaxCall(divid, userid) {
    $.ajax({
        type: "POST",
        url: '/User/PostList/',
        data: { id: userid },
        success: function (data) {
            $(divid).html(data)
        }
    })
}

function RemovePost(divid, userid, postId) {
    $.ajax({
        type: "POST",
        url: '/User/RemovePost/',
        data: { id: postId },
        success: function (data) {
            PostsAjaxCall(divid, userid)
        }
    })
}

function LikePost(divid, postId) {
    $.ajax({
        type: "POST",
        url: '/User/LikePost/',
        data: { id: postId },
        success: function (data) {
            $(divid).html("♥ "+data["likes"])
        }
    })
}

function CreatePostAjaxCall(divid, userid, textid, imageid) {
    var formdata = new FormData();
    var files = $(imageid).get(0).files;
    if (files.length > 0) {
        formdata.append("image", files[0]);
    }
    formdata.append("text", $(textid).val());
    formdata.append("id", userid);
    $.ajax({
        type: "POST",
        url: '/User/PostCreate/',
        dataType: 'json',
        data: formdata,
        contentType: false,
        processData: false,
        success: function (data) {
            PostsAjaxCall(divid, userid)
        }
    })
}

function SetIntervalOnLoad(divid, userid) {
    PostsAjaxCall(divid, userid)
    setInterval(function () { PostsAjaxCall(divid, userid) }, 5000)
}