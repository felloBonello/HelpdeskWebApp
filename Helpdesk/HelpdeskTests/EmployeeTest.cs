using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;
using System.Collections.Generic;
using MongoDB.Bson;
using HelpdeskViewModels;

namespace HelpdeskTests
{
    [TestClass]
    public class EmployeeTest
    {
        
        [TestMethod]
        public void EmployeeDAOUpdateShouldReturnTrue()
        {
            EmployeeDAO dao = new EmployeeDAO();
            //simulate user 1 getting an employee
            Employee emp = dao.GetByID("564107b33dd4ed255425f4ca"); //smartypants' id
            emp.Phoneno = "555-555-5551";
            int rowsUpdated = dao.Update(emp);
            //user 1 makes update
            Assert.IsTrue(rowsUpdated == 1);
        }

        [TestMethod]
        public void EmployeeDAOUpdateTwiceShouldReturnNegative()
        {
            EmployeeDAO dao = new EmployeeDAO();
            //simulate 2 users getting an employee
            Employee emp = dao.GetByID("564107b33dd4ed255425f4ca"); //smartypants' id
            Employee emp2 = dao.GetByID("564107b33dd4ed255425f4ca"); //smartypants' id
            emp.Phoneno = "555-555-5551";
            int rowsUpdated = dao.Update(emp);
            if (rowsUpdated == 1)
            {
                rowsUpdated = dao.Update(emp2);
            }
            Assert.IsTrue(rowsUpdated == -2);
        }

        [TestMethod]
        public void EmployeeDAOGetAllShouldReturnList()
        {
            EmployeeDAO dao = new EmployeeDAO();
            List<Employee> emps = dao.GetAll();
            Assert.IsTrue(emps.Count > 0);
        }

        [TestMethod]
        public void EmployeeDAOCreateAndDeleteShouldReturnTrue()
        {
            bool deleteOk = false;
            Employee emp = new Employee();
            EmployeeDAO dao = new EmployeeDAO();
            // use and existing department id here
            emp.DepartmentId = new ObjectId("564107b33dd4ed255425f4c5");
            // and some hardcoded data
            emp.Email = "somemp@here.com";
            emp.Firstname = "Some";
            emp.Lastname = "Employee";
            emp.Phoneno = "(555)555-5555";
            emp.Title = "Mr. ";
            string newid = dao.Create(emp);
            if (newid.Length == 24) // new id's are a 24 byte hex string
            {
                deleteOk = dao.Delete(newid);
            }
            Assert.IsTrue(deleteOk);
        }
        
        /////////////////////////////////////////////////  View Model Tests /////////////////////////////////////
        [TestMethod]
        public void EmployeeVMUpdateShouldReturnTrue()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            // simulate user 1 getting an employee
            vm.GetByID("564107b33dd4ed255425f4ca"); // smartypants' id
            vm.Phoneno = "555-555-5551";
            int rowsUpdated = vm.Update();
            //user 1 makes update
            Assert.IsTrue(rowsUpdated == 1);
        }

        [TestMethod]
        public void EmployeeVMUpdateTwiceShouldReturnNegative2()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            EmployeeViewModel vm2 = new EmployeeViewModel();
            // simulate 2 users getting an employee
            vm.GetByID("564107b33dd4ed255425f4ca"); // smartypants' id
            vm2.GetByID("564107b33dd4ed255425f4ca"); // smartypants' id
            vm.Phoneno = "555-555-5551";
            int rowsUpdated = vm.Update();
            if(rowsUpdated == 1)
            {
                rowsUpdated = vm2.Update();
            }
            Assert.IsTrue(rowsUpdated == -2);
        }

        [TestMethod]
        public void EmployeeVMGetAllShouldReturnList()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            List<EmployeeViewModel> emps = vm.GetAll();
            Assert.IsTrue(emps.Count > 0);
        }

        [TestMethod]
        public void EmployeeVMCreateAndDeleteShouldReturnTrue()
        {
            bool deleteOk = false;
            EmployeeViewModel vm = new EmployeeViewModel();
            //use an existing departmentid here
            vm.DepartmentId = "564107b33dd4ed255425f4c5";
            // and some hardcoded data
            vm.Email = "someemp@here.com";
            vm.Firstname = "Some";
            vm.Lastname = "Employee";
            vm.Phoneno = "(555)555-5555";
            vm.Title = "Mr. ";
            vm.Create();
            if(vm.Id.Length == 24) //new id's are a 24 byte hex string
            {
                deleteOk = vm.Delete();
            }
            Assert.IsTrue(deleteOk);
        }
    }
}
