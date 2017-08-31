///<summary>
/// @purpose: Dynamically create table for problem webpage and add functionality to all buttons and controls
/// @date: 2015-10-28
/// @author: Justin Bonello
/// @revisions: 3
///</summary>


$(function () {
    $("#ProblemModalForm").validate({
        rules: {
            TextboxDesc: { required: true }
        },
        ignore: ".ignore, :hidden",
        errorElement: "div",
        wrapper: "div", //a wrapper around the error message
        messages: {
            TextboxDesc: {
                required: "Enter Problem"
            }
        }
    });

    getAll("Problems Loaded");


    $("#main").click(function (e) { //click on any row
        var validator = $('#ProblemModalForm').validate();
        validator.resetForm();

        if (!e) e = window.event;
        var probId = e.target.parentNode.id;

        if (probId == "main") {
            probId = e.target.id;  //clicked on a row somewhere else
        }

        if (probId != "problem") {
            getById(probId);
            $("#ButtonAction").prop("value", "Update");
            $("#ButtonDelete").show();
        }
        else {
            $("#BottonDelete").hide();
            $("#ButtonAction").prop("value", "add");
            $("#HiddenId").val("new");
            $("#TextboxDesc").val("");
            $("#ButtonUpdate").prop("value", "Add");
            $("#ButtonDelete").hide("");
        }
    })

    //Modal Buttons click - make sure to return false for click or REST calls get cancelled
    $("#ButtonDelete").click(function () {
        var deleteEmp = confirm("really delete this problem?");
        if (deleteEmp) {
            _delete();
            return false;
        }
    });

    $("#ButtonAction").click(function () {
        if ($("#ProblemModalForm").valid()) {
            if ($("#ButtonAction").val() === "Update") {
                update();
            }
            else {
                create();
            }
        }
        return false;
    })

    $("#X").click(function () {
        $("#lblstatus").text(null);
    })
});

function copyInfoToModal(prob) {
    $("#TextboxDesc").val(prob.Description);
    $("#HiddenId").val(prob.Id);
    $("#HiddenEntity").val(prob.Entity64);
}

//build initial table
function buildTable(data) {
    $("#main").empty();
    var bg = false;
    problems = data; // copy to global var
    div = $("<div id=\"problem\" data-toggle=\"modal\" data-target=\"#myModal\" class=\"row trWhite\">");
    div.html("<div class=\"col-lg-12 rowBlue\" id=\"id0\">...Click Here to add</div>");
    div.appendTo($("#main"));
    $.each(data, function (index, prob) {
        var cls = "rowBlue";
        bg ? cls = "rowBlue" : cls = "rowLightBlue";
        bg = !bg;
        div = $("<div id=\"" + prob.Id + "\" data-toggle=\"modal\" data-target=\"#myModal\" style=\"padding:0 0 0 0; width:110%;\" class=\"row col-lg-12 " + cls + "\">");
        var probId = prob.Id;
        div.html(
                        "<div class=\"col-xs-12\" id=\"problemdesc" + probId + "\">" + prob.Description + "</div>"
                        );
        div.appendTo($("#main"));
    }); //each
} //build table

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

function getAll(msg) {
    $("LabelStatus").text("Loading...");

    ajaxCall("Get", "api/problems" + "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Problems Loaded");
        else
            $("#LabelStatus").text(msg);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function getById(probId) {
    ajaxCall("Get", "api/problems/" + probId, "")
    .done(function (data) {
        copyInfoToModal(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function create() {
    prob = new Object();
    prob.Description = $("#TextboxDesc").val();

    ajaxCall("Post", "api/problems/", prob)
    .done(function (data) {
        getAll(data);
        $("#myModal").modal("toggle");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function update() {
    prob = new Object();
    prob.Description = $("#TextboxDesc").val();
    prob.Id = $("#HiddenId").val();
    prob.Entity64 = $("#HiddenEntity").val();

    ajaxCall("Put", "api/problems/", prob)
    .done(function (data) {
        getAll("");
        copyInfoToModal(prob);
        $("#lblstatus").text(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
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

function _delete() {
    ajaxCall("Delete", "api/problems/" + $("#HiddenId").val(), "")
    .done(function (data) {
        getAll(data);
        $("#myModal").modal("toggle");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        $("#myModal").modal("toggle");
        errorRoutine(jqXHR)
    });
}