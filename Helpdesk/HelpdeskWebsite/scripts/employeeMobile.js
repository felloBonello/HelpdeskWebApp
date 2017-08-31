///<summary>
/// @purpose: Dynamically create table for utils webpage and add functionality to all buttons and controls for employeeMobile webpage
/// @date: 2015-12-05
/// @author: Justin Bonello
/// @revisions: 3
///</summary>

$(function () {
    $("#EmployeeModalForm").validate({
        rules: {
            TextBoxTitle: { maxlength: 4, required: true, validTitle: true },
            TextBoxFirstname: { maxlength: 25, required: true },
            TextBoxLastname: { maxlength: 25, required: true },
            TextBoxEmail: { maxlength: 40, required: true, email: true },
            TextBoxPhone: { maxlength: 15, required: true },
            ddlDepts: { required: true }
        },
        ignore: ".ignore, :hidden",
        errorElement: "div",
        wrapper: "div", //a wrapper around the error message
        messages: {
            TextBoxTitle: {
                required: "required 1-4 chars.", maxlength: "required 1-4 chars.", validTitle: "Mr. Ms. Mrs. or Dr."
            },
            TextBoxFirstname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxLastname: {
                required: "required 1-25 chars.", maxlength: "required 1-25 chars."
            },
            TextBoxPhone: {
                required: "required 1-15 chars.", maxlength: "required 1-15 chars."
            },
            TextBoxEmail: {
                required: "required 1-40 chars.", maxlength: "required 1-40 chars.", email: "need valid email format"
            },
            ddlDepts: {
                required: "required department"
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
    $.validator.addMethod("validTitle", function (value, element) { // custom rule
        return this.optional(element) || (value == "Mr." || value == "Ms." || value == "Mrs." || value == "Dr.");
    }, "");

    getAll('');

    //   Main display click
    $('#main').click(function (e) {  // click on any row

        //   reset validation
        var validator = $('#EmployeeModalForm').validate();
        validator.resetForm();

        var empId = e.toElement.id.substr(3, e.toElement.id.length);

        if (empId.length === 24) {
            getById(empId);
        }
        else
            return false;  // arrow wasn't clicked
    });

    $("#ButtonDelete").click(function () {
        var deleteEmp = confirm("really delete this employee?");
        if (deleteEmp) {
            _delete();
        }
        return false;
    });

    $("#ButtonAction").click(function () {
        if ($("#EmployeeModalForm").valid()) {
            update();
            return false;
        }
    });

});  // main jquery function

// build initial table
function buildTable(data) {
    $('#main').empty();
    var bg = false;
    employees = data; // copy to global var
    li = $("<li data-role=\"list-divider\" id=\"emphead\" role=\"heading\">" +
              "<fieldset class=\"ui-grid-c\"style=\"padding:3%;background-color: #03a9f4;color:white;\">" +
              "   <div class=\"ui-block-a\" style=\"width:15%;\">&nbsp;</div>" +
              "   <div class=\"ui-block-b\" style=\"width:20%;\">Title</div>" +
              "   <div class=\"ui-block-c\" style=\"width:20%;text-align:center;\">First</div>" +
              "   <div class=\"ui-block-d\" style=\"width:30%;text-align:center;\">Last</div>" +
              "</fieldset>" +
           "</li>");
    li.appendTo($('#main'));

    $.each(employees, function (index, emp) {
        var empId = emp.Id;
        li = $("<li id=\"" + empId + "\" class=\"ui-li-divider ui-bar-inherit\" style=\"padding:2%; background-color: #40c4ff;\">" +
                      "<fieldset>" +
                      "             <div class=\"ui-block-a\" style=\"width:15%;\">" +
                      "                         <a href=\"#empmobilemodal\" data-transition=\"flip\" class=\"ui-btn-icon-right ui-icon-carat-r\" id=\"emp" + empId + "\">" +
                      "                                 <img src =\"data:image/png;base64," + emp.StaffPicture64 + "\"  style=\"max-width:25px; max-height:25px;\" />" +
                      "                         </a>" +
                      "              </div>" +
                      "   <div class=\"ui-block-b\" style=\"width:20%;\">" + emp.Title + "</div>" +
                      "   <div class=\"ui-block-c\" style=\"width:20%;\">" + emp.Firstname + "</div>" +
                      "   <div class=\"ui-block-d\" style=\"width:20%;\">" + emp.Lastname + "</div>" +  
                      "</fieldset>" +
                   "</li>");
        li.appendTo($('#main'));
    }); // each
}

//  copy Employee info to modal
function copyInfoToModal(emp) {
    $('#TextBoxTitle').val(emp.Title);
    $('#TextBoxFirstname').val(emp.Firstname);
    $('#TextBoxLastname').val(emp.Lastname);
    $('#TextBoxPhone').val(emp.Phoneno);
    $('#TextBoxEmail').val(emp.Email);
    $("#HiddenId").val(emp.Id);
    $("#HiddenImg").val(emp.StaffPicture64);
    $("#HiddenEntity").val(emp.Entity64);
    $("#lblstatus").text("");
    if (emp.IsTech) {
        $("#CheckBoxIsTech").prop("checked", true);
    }
    else {
        $("#CheckBoxIsTech").prop("checked", false);
    }
    loadDepartmentDDL(emp.DepartmentId);
}

//load drop down
function loadDepartmentDDL(empdep) {
    $.ajax({
        type: "Get",
        url: "api/departments",
        contentType: "application/json; charset=utf-8"
    })
    .done(function (data) {
        html = "";
        $("#ddlDepts").empty();
        $.each(data, function () {
            html += "<option value=\"" + this["Id"] + "\"  data-mini=\"true\">" + this["DepartmentName"] + "</option>";
        });
        $("#ddlDepts").append(html);
        $("#ddlDepts").val(empdep);
        $('#ddlDepts').selectmenu('refresh');
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        alert("error");
    });
}

//   ajax calls
function getAll(msg) {
    $('#LabelStatus').text("Employees Loading...");

    ajaxCall('Get', 'api/employees', '')
    .done(function (data) {
        buildTable(data);
        if (msg == '')
            $('#LabelStatus').text('Employees Loaded');
        else
            $('#LabelStatus').text(msg + ' - ' + 'Employees Loaded');
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function getById(empId) {
    ajaxCall('Get', 'api/employees/' + empId, '')
    .done(function (data) {
        copyInfoToModal(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });
}

function update() {
    emp = new Object();
    emp.Title = $("#TextBoxTitle").val();
    emp.Firstname = $("#TextBoxFirstname").val();
    emp.Lastname = $("#TextBoxLastname").val();
    emp.Phoneno = $("#TextBoxPhone").val();
    emp.Email = $("#TextBoxEmail").val();
    emp.DepartmentId = $("#ddlDepts").val();
    emp.Id = $("#HiddenId").val();
    emp.IsTech = $("#CheckBoxIsTech").is(":checked");
    emp.StaffPicture64 = $("#HiddenImg").val();
    emp.Entity64 = $("#HiddenEntity").val();

    ajaxCall("Put", "api/employees/", emp)
    .done(function (data) {
        getAll(data);
        copyInfoToModal(emp);
        $("#lblstatus").text(data);
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        errorRoutine(jqXHR);
    });

}

function _delete() {
    ajaxCall("Delete", "api/employees/" + $("#HiddenId").val(), "")
    .done(function (data) {
        getAll(data);
        window.location.href = '#mobilepage';
    })
    .fail(function (jqXHR, textStatus, errorThrown) {
        $("#myModal").modal("toggle");
        errorRoutine(jqXHR)
    });
}

// ajax Call - returns promise
function ajaxCall(type, url, data) {
    return $.ajax({ // return the promise that `$.ajax` returns
        type: type,
        url: url,
        data: JSON.stringify(data),
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        processData: true,
    });
}

// commmon error
function errorRoutine(jqXHR) {
    if (jqXHR.responseJSON == null) {
        $('#LabelStatus').text(jqXHR.responseText);
    }
    else {
        $('#LabelStatus').text(jqXHR.responseJSON.Message);
    }
}