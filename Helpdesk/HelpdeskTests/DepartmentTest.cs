using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;
using System.Collections.Generic;
using MongoDB.Bson;
using HelpdeskViewModels;

namespace HelpdeskTests
{
    [TestClass]
    public class DepartmentTest
    {
        
        [TestMethod]
        public void CreateCollectionsShouldReturnTrue()
        {
            DALUtils util = new DALUtils();
            Assert.IsTrue(util.LoadCollections());
        }
        

        [TestMethod]
        public void DepartmentDAOUpdateShouldReturnTrue()
        {
            DepartmentDAO dao = new DepartmentDAO();
            //simulate user 1 getting an employee
            Department dep = dao.GetByID("564107b33dd4ed255425f4c5"); //administrations' id
            dep.DepartmentName = "Administration";
            int rowsUpdated = dao.Update(dep);
            //user 1 makes update
            Assert.IsTrue(rowsUpdated == 1);
        }

        [TestMethod]
        public void DepartmentDAOUpdateTwiceShouldReturnNegative()
        {
            DepartmentDAO dao = new DepartmentDAO();
            //simulate 2 users getting an employee
            Department dep = dao.GetByID("564107b33dd4ed255425f4c5"); //administrations' id
            Department dep2 = dao.GetByID("564107b33dd4ed255425f4c5"); //administrations' id
            dep.DepartmentName = "Administration";
            int rowsUpdated = dao.Update(dep);
            if (rowsUpdated == 1)
            {
                rowsUpdated = dao.Update(dep2);
            }
            Assert.IsTrue(rowsUpdated == -2);
        }

        [TestMethod]
        public void DepartmentDAOGetAllShouldReturnList()
        {
            DepartmentDAO dao = new DepartmentDAO();
            List<Department> deps = dao.GetAll();
            Assert.IsTrue(deps.Count > 0);
        }

        [TestMethod]
        public void DepartmentDAOCreateAndDeleteShouldReturnTrue()
        {
            bool deleteOk = false;
            Department dep = new Department();
            DepartmentDAO dao = new DepartmentDAO();
            dep.DepartmentName = "Human Resources";  // and some hardcoded data

            string newid = dao.Create(dep);
            if (newid.Length == 24) // new id's are a 24 byte hex string
            {
                deleteOk = dao.Delete(newid);
            }
            Assert.IsTrue(deleteOk);
        }
        
        /////////////////////////////////////////////////  View Model Tests /////////////////////////////////////
        [TestMethod]
        public void DepartmentVMUpdateShouldReturnTrue()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            // simulate user 1 getting an Department
            vm.GetByID("564107b33dd4ed255425f4c5"); // admins' id
            vm.DepartmentName = "Administration";
            int rowsUpdated = vm.Update();
            //user 1 makes update
            Assert.IsTrue(rowsUpdated == 1);
        }

        [TestMethod]
        public void DepartmentVMUpdateTwiceShouldReturnNegative2()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            DepartmentViewModel vm2 = new DepartmentViewModel();
            // simulate 2 users getting an Department
            vm.GetByID("564107b33dd4ed255425f4c5"); // admins' id
            vm2.GetByID("564107b33dd4ed255425f4c5"); // admins' id
            vm.DepartmentName = "Administration";
            int rowsUpdated = vm.Update();
            if (rowsUpdated == 1)
            {
                rowsUpdated = vm2.Update();
            }
            Assert.IsTrue(rowsUpdated == -2);
        }

        [TestMethod]
        public void DepartmentVMGetAllShouldReturnList()
        {
            DepartmentViewModel vm = new DepartmentViewModel();
            List<DepartmentViewModel> deps = vm.GetAll();
            Assert.IsTrue(deps.Count > 0);
        }

        [TestMethod]
        public void DepartmentVMCreateAndDeleteShouldReturnTrue()
        {
            bool deleteOk = false;
            DepartmentViewModel vm = new DepartmentViewModel();
            // and some hardcoded data
            vm.DepartmentName = "someName";
            vm.Create();

            if (vm.Id.Length == 24) //new id's are a 24 byte hex string
            {
                deleteOk = vm.Delete();
            }
            Assert.IsTrue(deleteOk);
        }
    }
}