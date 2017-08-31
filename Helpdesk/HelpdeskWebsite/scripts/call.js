
$(function () {
    $("#CallModalForm").validate({
        rules: {
            ddlProblem: { required: true },
            ddlEmployee: { required: true },
            ddlTechnician: { required: true },
            TextareaNotes: { maxlength: 250, required: true },
        },
        ignore: ".ignore, :hidden",
        errorElement: "div",
        wrapper: "div", //a wrapper around the error message
        messages: {
            ddlProblems: {
                required: "Select Problem"
            },
            ddlEmployee: {
                required: "Select Employee"
            },
            ddlTechnicians: {
                required: "Select Technician"
            },
            TextareaNotes: {
                required: "required 1-250 chars.", maxlength: "required 1-250 chars."
            }
        }
    });

    getAll("Calls Loaded");

    $("#main").click(function (e) { //click on any row

        //   reset validation
        var validator = $('#CallModalForm').validate();
        validator.resetForm();

        if (!e) e = window.event;
        var callId = e.target.parentNode.id;

        if (callId == "main") {
            callId = e.target.id;  //clicked on a row somewhere else
        }

        if (callId != "call") {
            getById(callId);
            $("#ButtonAction").prop("value", "Update");
            $("#ButtonDelete").show();
        }
        else {       
            $("#ButtonDelete").hide();
            $("#ButtonAction").prop("value", "add");
            $("#HiddenId").val("new");
            loadProblemDDL(-1);
            loadEmployeeDDL(-1);
            loadTechnicianDDL(-1);
            $("#lblDateOpened").text(formatDate());
            $("#lblDateClosed").text("");
            $("#CheckBoxClosed").prop("checked", false);
            $("#TextareaNotes").val("");
            $("#ButtonAction").show();
            $("#TextareaNotes").attr('disabled', false);
            $("#ddlProblem").attr("disabled", false);
            $("#ddlEmployee").attr("disabled", false);
            $("#ddlTechnician").attr("disabled", false);
        }
    })

    //Modal Buttons click - make sure to return false for click or REST calls get cancelled
    $("#ButtonDelete").click(function () {
        var deleteCall = confirm("really delete this call?");
        if (deleteCall) {
            _delete();
            return false;
        }
    });

    $("#ButtonAction").click(function () {
        if ($("#CallModalForm").valid()) {  
            if ($('#ButtonAction').val() === "Update") {
                update();
            }
            else {
                create();
            }         
        }
        else {
            $("#lblstatus").text("fix existing problems");
            $("#lblstatus").css({ "color": "red" });
        }
        return false;
    });

    $('#CheckBoxClosed').change(function () {
        if ($("#lblDateClosed").text() === "") {
            $("#lblDateClosed").text(formatDate());
        }
        else
            $("#lblDateClosed").text("");
    });

    $("#X").click(function () {
        $("#lblstatus").text(null);
    });
});

function copyInfoToModal(call) {
    if (call.OpenStatus) {
        $("#lblDateClosed").text("");
        $("#CheckBoxClosed").prop("checked", false);
        $("#ButtonAction").show();
        $("#TextareaNotes").attr('disabled', false);
        $("#ddlProblem").attr("disabled", false);
        $("#ddlEmployee").attr("disabled", false);
        $("#ddlTechnician").attr("disabled", false);
        $("#CheckBoxClosed").prop("disabled", false);
    }
    else {
        $("#lblDateClosed").text(formatDate(call.DateClosed));
        $("#ButtonAction").hide();
        $("#ddlProblem").attr("disabled", true);
        $("#ddlEmployee").attr("disabled", true);
        $("#ddlTechnician").attr("disabled", true);
        $("#TextareaNotes").attr('disabled', true);
        $("#CheckBoxClosed").prop("checked", true);
        $("#CheckBoxClosed").prop("disabled", true);
    }
    $("#HiddenId").val(call.Id);
    $("#HiddenEntity").val(call.Entity64);
    $("#lblDateOpened").text(formatDate(call.DateOpened));
    $("#TextareaNotes").val(call.Notes);
    loadEmployeeDDL(call.EmployeeId);
    loadProblemDDL(call.ProblemId);
    loadTechnicianDDL(call.TechId);
}

//load drop down
function loadProblemDDL(callProb) {
    $.ajax({
        type: "Get",
        url: "api/problems",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlProblem").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["Id"] + "\">" + this["Description"] + "</option>";
        });
        if (callProb == -1) {
            html += "<option value=\"\" disabled selected>-- select problem --</option>";
            $("#ddlProblem").append(html);
        }
        else {
            $("#ddlProblem").append(html);
            $("#ddlProblem").val(callProb);           
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("error");
    });
}

function loadEmployeeDDL(callEmp) {
    $.ajax({
        type: "Get",
        url: "api/employees",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlEmployee").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["Id"] + "\">" + this["Lastname"] + "</option>";
        });
        if (callEmp == -1) {
            html += "<option value=\"\" disabled selected>-- select employee --</option>";
            $("#ddlEmployee").append(html);
        }
        else {
            $("#ddlEmployee").append(html);
            $("#ddlEmployee").val(callEmp);          
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("error");
    });
}

function loadTechnicianDDL(callTech) {
    $.ajax({
        type: "Get",
        url: "api/employees",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlTechnician").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["Id"] + "\">" + this["Lastname"] + "</option>";
        });
        if (callTech == -1) {
            html += "<option value=\"\" disabled selected>-- select technician --</option>";
            $("#ddlTechnician").append(html);
        }
        else {
            $("#ddlTechnician").append(html);
            $("#ddlTechnician").val(callTech);          
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("error");
    });
}

//build initial table
function buildTable(data) {
    jQuery.ajaxSetup({ async: false });

    $("#main").empty();
    var bg = false;
    calls = data; // copy to global var
    div = $("<div id=\"call\" data-toggle=\"modal\" data-target=\"#myModal\" class=\"row trWhite\">");
    div.html("<div class=\"col-lg-12 rowBlue\" style=\"text-align: center;\" id=\"id0\">...Click Here to add</div>");
    div.appendTo($("#main"));
    $.each(data, function (index, call) {
        var cls = "rowBlue";
        bg ? cls = "rowBlue" : cls = "rowLightBlue";
        bg = !bg;
        div = $("<div id=\"" + call.Id + "\" data-toggle=\"modal\" data-target=\"#myModal\" style=\"padding:0 0 0 0; width:105%; \"  class=\"row col-lg-12 " + cls + "\">");
        var callId = call.Id;
        var EmpName;
        var Problem;

        ajaxCall("Get", "api/problems/" + call.ProblemId, "")
            .then(function (data) {
            Problem = data.Description;
        })
            .fail(function (jqXHR, textStatus, errorThrown) {
            errorRoutine(jqXHR);
        });
        ajaxCall("Get", "api/employees/" + call.EmployeeId, "")
            .done(function (data) {
                EmpName = data.Lastname;
            })
            .fail(function (jqXHR, textStatus, errorThrown) {
            errorRoutine(jqXHR);
            });

        div.html(
                        "<div class=\"col-xs-4\" style=\"text-indent: 2em;\" id=\"date" + callId + "\">" + formatDate(call.DateOpened) + "</div>" +
                        "<div class=\"col-xs-3\" id=\"employeeName" + callId + "\">" + EmpName + "</div>" +
                        "<div class=\"col-xs-5\" id=\"Problem" + callId + "\">" + Problem + "</div>"
                        );
        div.appendTo($("#main"));
    }); //each

    jQuery.ajaxSetup({ async: true });
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

    ajaxCall("Get", "api/calls", "")
    .done(function (data) {
        buildTable(data);
        if (msg == "")
            $("#LabelStatus").text("Calls Loaded");
        else
            $("#LabelStatus").text(msg);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function getById(callId) {
    ajaxCall("Get", "api/calls/" + callId, "")
    .done(function (data) {
        copyInfoToModal(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function create() {
    call = new Object();
    call.EmployeeId = $("#ddlEmployee").val();
    call.ProblemId = $("#ddlProblem").val();
    call.TechId = $("#ddlTechnician").val();
    call.DateOpened = $("#lblDateOpened").text();
    call.DateClosed = $("#lblDateClosed").text();
    call.OpenStatus = !($("#CheckBoxClosed").is(":checked"));
    call.Notes = $("#TextareaNotes").val();
    ajaxCall("Post", "api/calls/", call)
    .done(function (data) {
        getAll("Call added");
        $("#myModal").modal("toggle");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function update() {
    call = new Object();
    call.EmployeeId = $("#ddlEmployee").val();
    call.ProblemId = $("#ddlProblem").val();
    call.TechId = $("#ddlTechnician").val();
    call.DateClosed = $("#lblDateClosed").text();
    call.OpenStatus = !($("#CheckBoxClosed").is(":checked"));
    call.Notes = $("#TextareaNotes").val();
    call.Id = $("#HiddenId").val();
    call.Entity64 = $("#HiddenEntity").val();

    ajaxCall("Put", "api/calls/", call)
    .done(function (data) {
        getAll("");
        copyInfoToModal(call);
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
    ajaxCall("Delete", "api/calls/" + $("#HiddenId").val(), "")
    .done(function (data) {
        getAll(data);
        $("#myModal").modal("toggle");
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        $("#myModal").modal("toggle");
        errorRoutine(jqXHR)
    });
}

function formatDate(date) {
    var d;
    if (date === undefined) {
        d = new Date();
    }
    else {
        var d = new Date(Date.parse(date));
    }

    var _day = d.getDate();
    var _month = d.getMonth() + 1;
    var _year = d.getFullYear();
    var _hour = d.getHours();
    var _min = d.getMinutes();
    if (_min < 10) { _min = "0" + _min; }
    return _year + "-" + _month + "-" + _day + " " + _hour + ":" + _min;
}