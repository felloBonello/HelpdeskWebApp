///<summary>
/// @purpose: Dynamically create table for utils webpage and add functionality to all buttons and controls for callMobile webpage
/// @date: 2015-12-05
/// @author: Justin Bonello
/// @revisions: 6
///</summary>


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
            ddlProblem: {
                required: "Select Problem"
            },
            ddlEmployee: {
                required: "Select Employee"
            },
            ddlTechnician: {
                required: "Select Tech"
            },
            TextareaNotes: {
                required: "required 1-250 chars.", maxlength: "required 1-250 chars."
            }
        },
        errorPlacement: function (error, element) {
            if (element.attr("name") === "ddlEmployee" || element.attr("name") === "ddlProblem" || element.attr("name") === "ddlTechnician") {
                error.insertAfter($(element).parent());
            } else {
                error.insertAfter(element);
            }
        }
    });

    $("#lblstatus").text("Loading Calls...");
    getAll("Calls Loaded");

    $("#main").click(function (e) { //click on any row

        //   reset validation
        var validator = $('#CallModalForm').validate();
        validator.resetForm();

        if (!e) e = window.event;
        var callId = e.toElement.id.substr(0, e.toElement.id.length);

        if (callId == "main") {
            callId = e.target.id;  //clicked on a row somewhere else
        }

        if (callId.length === 24) {
            //$("#ButtonAction").prop("value", "Update");
            $("#ButtonAction").val("Update");
            //$("#ButtonAction").button("refresh");
            //$("#ButtonAction").prev('a').find('span.ui-btn-text').text("Update");
            //$("#ButtonDelete").show();
            $('#ButtonDelete').closest('.ui-btn').show();
            getById(callId);
        }
        else if (callId === "new")
        {
            loadEmptyModal();
        }
        else {        
            return false;    
        }
        //return false;  // arrow wasn't clicked
    });

    //Modal Buttons click - make sure to return false for click or REST calls get cancelled
    $("#ButtonDelete").click(function () {
        var deleteCall = confirm("really delete this call?");
        if (deleteCall) {
            _delete();
        }
        return false;
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
        return false;
    });
});

function loadEmptyModal() {
    $('#ButtonDelete').closest('.ui-btn').hide();
    //$("#ButtonAction").prev('a').find('span.ui-btn-text').text("Add");
    // $("#ButtonAction").prop("value", "Add");
    $("#ButtonAction").val("Add");
    $("#HiddenId").val("new");
    $("#lblstatus").text("");
    loadProblemDDL(-1);
    loadEmployeeDDL(-1);
    loadTechnicianDDL(-1);
    $("#lblDateOpened").text(formatDate());
    $("#lblDateClosed").text("");
    $("#CheckBoxClosed").prop("checked", false);
    $("#CheckBoxClosed").removeClass("ui-state-disabled");
    $("#TextareaNotes").val("");
    $('#ButtonAction').closest('.ui-btn').show();
    $("#TextareaNotes").attr('disabled', false);
    $("#ddlProblem").attr("disabled", false);
    $("#ddlEmployee").attr("disabled", false);
    $("#ddlTechnician").attr("disabled", false);
    $("#ButtonAction").button("refresh");
};

function copyInfoToModal(call) {
    if (call.OpenStatus) {
        $("#lblDateClosed").text("");
        $("#CheckBoxClosed").prop("checked", false);
        $('#ButtonAction').closest('.ui-btn').show();
        $("#TextareaNotes").attr('disabled', false);
        $("#ddlProblem").attr("disabled", false);
        $("#ddlEmployee").attr("disabled", false);
        $("#ddlTechnician").attr("disabled", false);
        $("#CheckBoxClosed").removeClass("ui-state-disabled");
    }
    else {
        $("#lblDateClosed").text(formatDate(call.DateClosed));
        $('#ButtonAction').closest('.ui-btn').hide();
        $("#ddlProblem").attr("disabled", true);
        $("#ddlEmployee").attr("disabled", true);
        $("#ddlTechnician").attr("disabled", true);
        $("#TextareaNotes").attr('disabled', true);
        $("#CheckBoxClosed").prop("checked", true);
        $("#CheckBoxClosed").addClass("ui-state-disabled");
    }
    $("#lblstatus").text("");
    $("#HiddenId").val(call.Id);
    $("#HiddenEntity").val(call.Entity64);
    $("#lblDateOpened").text(formatDate(call.DateOpened));
    $("#TextareaNotes").val(call.Notes);
    loadEmployeeDDL(call.EmployeeId);
    loadProblemDDL(call.ProblemId);
    loadTechnicianDDL(call.TechId);
    $("#ButtonAction").button("refresh");
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
            html += "<option value=\"" + this["Id"] + "\"   data-mini=\"true\">" + this["Description"] + "</option>";
        });
        if (callProb == -1) {
            html += "<option value=\"\" data-mini=\"true\" disabled selected>select problem</option>";
            $("#ddlProblem").append(html);
            $('#ddlProblem').selectmenu('refresh');
        }
        else {
            $("#ddlProblem").append(html);
            $("#ddlProblem").val(callProb);
            $('#ddlProblem').selectmenu('refresh');
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
            html += "<option value=\"" + this["Id"] + "\" data-mini=\"true\">" + this["Lastname"] + "</option>";
        });
        if (callEmp == -1) {
            html += "<option value=\"\" data-mini=\"true\" disabled selected>select employee</option>";
            $("#ddlEmployee").append(html);
            $('#ddlEmployee').selectmenu('refresh');
        }
        else {
            $("#ddlEmployee").append(html);
            $("#ddlEmployee").val(callEmp);
            $('#ddlEmployee').selectmenu('refresh');
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
            html += "<option value=\"" + this["Id"] + "\" data-mini=\"true\">" + this["Lastname"] + "</option>";
        });
        if (callTech == -1) {
            html += "<option value=\"\" data-mini=\"true\" disabled selected>select tech</option>";
            $("#ddlTechnician").append(html);
            $('#ddlTechnician').selectmenu('refresh');
        }
        else {
            $("#ddlTechnician").append(html);
            $("#ddlTechnician").val(callTech);
            $('#ddlTechnician').selectmenu('refresh');
        }
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("error");
    });
}

//build initial table
function buildTable(data) {
    jQuery.ajaxSetup({ async: false });

    $('#main').empty();
    var bg = false;
    employees = data; // copy to global var
    li = $("<li data-role=\"list-divider\" id=\"emphead\" role=\"heading\">" +
              "<fieldset class=\"ui-grid-b\"style=\"padding:3%;background-color: #03a9f4;color:white;\">" +
              "   <div class=\"ui-block-a\" style=\"width:27%; font-size:12px; text-align:center;\">Date</div>" +
              "   <div class=\"ui-block-b\" style=\"width:24%;font-size:12px; text-align:center;\">Lastname</div>" +
              "   <div class=\"ui-block-c\" style=\"width:34%;font-size:12px; text-align:center; \">Problem</div>" +
              "</fieldset>" +
           "</li>");
    li.appendTo($('#main'));

    li = $("<li id=\"new\" class=\"ui-li-divider ui-bar-inherit\" style=\"background-color: #40c4ff;\">" +
            "<a href=\"#callmobilemodal\" data-transition=\"flip\" id=\"new\" style=\"text-decoration:none; font-size:11px; color:black;\">" +
            "...Click here to add" +
            "</a>" +
            "</li>"
        );
    li.appendTo($('#main'));


    $.each(data, function (index, call) {
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


        li = $("<li id=\"" + callId + "\" class=\"ui-li-divider ui-bar-inherit\" style=\"padding:2%; background-color: #40c4ff;\">" +
                      "<fieldset>" +
                      "<a href=\"#callmobilemodal\" data-transition=\"flip\" class=\"ui-btn-icon-right ui-icon-carat-r\" style=\"color:black;\" id=\""+ callId + "\">" +
                      "   <div class=\"ui-block-a\" style=\"width:30%; font-size:11px;\">" + formatDate(call.DateOpened) + "</div>" +
                      "   <div class=\"ui-block-b\" style=\"width:24%; word-wrap: break-word; overflow: hidden; text-overflow: ellipsis; font-size:11px;\">" + EmpName + "</div>" +
                      "   <div class=\"ui-block-c\" style=\"width:34%; word-wrap: break-word; overflow: hidden; text-overflow: ellipsis; font-size:11px;\">" + Problem + "</div>" +
                      "</a>" +
                      "</fieldset>" +
                   "</li>");
        li.appendTo($('#main'));
    }); // each
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
        window.location.href = '#mobilepage';
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
    call.DateOpened = $("#lblDateOpened").text();
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
        window.location.href = '#mobilepage';
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