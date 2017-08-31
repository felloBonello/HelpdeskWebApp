QUnit.test("Helpdesk Tests", function (assert) {
    assert.async(9)  // 9 Tests

    var firstEmployee = {};
    var secondEmployee = {};

    //Get All Employees
    ajaxCall("Get", "api/employees", "")
    .then(function (data) {
        firstEmployee = data[1];
        secondEmployee = data[2];
        var numOfEmployees = data.length - 1;
        ok(numOfEmployees > 0, "Found " + numOfEmployees + " Employees"); //assert #1
    })
    .then(function (data) {

    //Create new employee object
    emp = new Object();
    emp.Title = "Mr.";
    emp.Firstname = "Test";
    emp.Lastname = "Case";
    emp.Phoneno = "(123)123-4567";
    emp.Email = "Test@gmail.com";
    emp.DepartmentId = "563d30b43dd4dd34b4c05cb2";

    //add employee to database
    ajaxCall("Post", "api/employees/", emp)
    .then(function (data) {
        ok(data.Firstname === "Test", "Created employee: Test Case"); //assert # 2
        // Retrieve employee for delete
        return(ajaxCall("Get", "api/employees", ""));
    })
    .then(function (data) {
        var Id = data[data.length - 1].Id;
        ok(data[data.length - 1].Firstname === "Test", "Employee " + Id + " retrieved for delete"); //assert # 5
        ajaxCall("Delete", "api/employees/" + Id, "")
        .then(function (msg) {
            //check to see if keyword 'deleted' is returned indicating success
            var x = msg.indexOf("deleted!");
            notEqual(x, -1, "Employee " + Id + " was deleted"); // assert # 6
        });
    });

    // Retrieve Smartypants for update
    ajaxCall("Get", "api/employees/" + firstEmployee.Id, "")
    .then(function (data) {
        data.Phoneno = "(555)555-5554";
        ok(data.Id.length === 24, "Employee " + data.Id + " retrieved for update") // assert # 3
        //Update employee Smartypants
        ajaxCall("Put", "api/employees", data)
        .then(function (msg) {
            //check to see if keyword 'not' is returned indicating failure
            var x = msg.indexOf("not");
            equal(x, -1, "Employee " + data.Id + "  was updated"); // assert # 4
        });
    });
    
    // Retrieve Andmirrors for concurency test update
    ajaxCall("Get", "api/employees/" + secondEmployee.Id, "")
    .then(function (data) {
        data.Phoneno = "(555)555-5552";
        ok(data.Id.length === 24, "Employee " + data.Id + " retrieved for update (concurency test)") // assert # 7
        //Update employee Smartypants
        ajaxCall("Put", "api/employees", data)
        .then(function (msg) {
            //check to see if keyword 'not' is returned indicating failure
            var x = msg.indexOf("not");
            equal(x, -1, "First update for employee " + data.Id + "  complete (concurency test)"); // assert # 8

            data.Phoneno = "(555)555-5554";
            ajaxCall("Put", "api/employees", data)
            .then(function (msg2) {
                var x = msg2.indexOf("stale");
                notEqual(x, -1, "Second update for employee " + data.Id + "  was stale (concurency test)"); // assert # 9
            });
        });
    });
    });
});