
$(function () {
    $("#DepartmentModalForm").validate({
        rules: {
            TextboxName: { required: true }
        },
        ignore: ".ignore, :hidden",
        errorElement: "div",
        wrapper: "div", //a wrapper around the error message
        messages: {
            TextboxName: {
                required: "Enter Department"
            }
        }
    });

    getAll("Departments Loaded");


    $("#main").click(function (e) { //click on any row
        var validator = $('#DepartmentModalForm').validate();
        validator.resetForm();

        if (!e) e = window.event;
        var depId = e.target.parentNode.id;

        if (depId == "main") {
            depId = e.target.id;  //clicked on a row somewhere else
        }

        if (depId != "department") {
            getById(depId);
            $("#ButtonAction").prop("value", "Update");
            $("#ButtonDelete").show();
        }
        else {
            $("#BottonDelete").hide();
            $("#ButtonAction").prop("value", "add");
            $("#HiddenId").val("new");
            $("#TextboxName").val("");
            $("#ButtonUpdate").prop("value", "Add");
            $("#ButtonDelete").hide("");
        }
    })

    //Modal Buttons click - make sure to return false for click or REST calls get cancelled
    $("#ButtonDelete").click(function () {
        var deleteEmp = confirm("really delete this department?");
        if (deleteEmp) {
            _delete();
            return false;
        }
    });

    $("#ButtonAction").click(function () {
        if ($("#DepartmentModalForm").valid()) {
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

function copyInfoToModal(dep) {
    $("#TextboxName").val(dep.DepartmentName);
    $("#HiddenId").val(dep.Id);
    $("#HiddenEntity").val(dep.Entity64);
}

//build initial table
function buildTable(data) {
    $("#main").empty();
    var bg = false;
    departments = data; // copy to global var
    div = $("<div id=\"department\" data-toggle=\"modal\" data-target=\"#myModal\" class=\"row trWhite\">");
    div.html("<div class=\"col-lg-12 rowBlue\" id=\"id0\">...Click Here to add</div>");
    div.appendTo($("#main"));
    $.each(data, function (index, dep) {
        var cls = "rowBlue";
        bg ? cls = "rowBlue" : cls = "rowLightBlue";
        bg = !bg;
        div = $("<div id=\"" + dep.Id + "\" data-toggle=\"modal\" data-target=\"#myModal\" style=\"padding:0 0 0 0; width:110%;\" class=\"row col-lg-12 " + cls + "\">");
        var depId = dep.Id;
        div.html(
                        "<div class=\"col-xs-12\" id=\"departmentname" + depId + "\">" + dep.DepartmentName + "</div>"
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

    ajaxCall("Get", "api/departments", "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Departments Loaded");
        else
            $("#LabelStatus").text(msg);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function getById(depId) {
    ajaxCall("Get", "api/departments/" + depId, "")
    .done(function (data) {
        copyInfoToModal(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function create() {
        dep = new Object();
        dep.DepartmentName = $("#TextboxName").val();

        ajaxCall("Post", "api/departments/", dep)
        .done(function (data) {
            getAll(data);
            $("#myModal").modal("toggle");
        })
        .fail(function (jqXHR, textStatus, errorThrown) {
            errorRoutine(jqXHR);
        });
}

function update() {
    dep = new Object();
    dep.DepartmentName = $("#TextboxName").val();
    dep.Id = $("#HiddenId").val();
    dep.Entity64 = $("#HiddenEntity").val();

    ajaxCall("Put", "api/departments/", dep)
    .done(function (data) {
        getAll("");
        copyInfoToModal(dep);
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
    ajaxCall("Delete", "api/departments/" + $("#HiddenId").val(), "")
    .done(function (data) {
        getAll(data);
        $("#myModal").modal("toggle");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        $("#myModal").modal("toggle");
        errorRoutine(jqXHR)
    });
}