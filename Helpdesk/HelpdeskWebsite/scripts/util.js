///<summary>
/// @purpose: Dynamically create table for utils webpage and add functionality to all buttons and controls
/// @date: 2015-10-28
/// @author: Justin Bonello
/// @revisions: 3
///</summary>

$(function () {
    $("#ButtonAction").click(function () {
        $("#lblstatus").text("Reloading database...");
        loadCollections();
    })
});

function loadCollections() {
    ajaxCall("Post", "api/utils", "")
    .done(function (data) {
        $("#lblstatus").text(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function ajaxCall(type, url, data) {
    return $.ajax({ //return the promise that '$.ajax' returns
        type: type,
        url: url,
        data: JSON.stringify(data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        processData: true
    });
}

function errorRoutine(jqXHR) { //common error routine
    if (jqXHR.responseJSON == null) {
        $("#lblstatus").text(jqXHR.responseText);
    }
    else {
        $("#lblstatus").text(jqXHR.responseJSON.Message);
    }
}