using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;
using System.Collections.Generic;
using MongoDB.Bson;
using HelpdeskViewModels;
using System.Diagnostics;

namespace HelpdeskTests
{
    [TestClass]
    public class CallTest
    {
        /*
        [TestMethod]
        public void CreateCollectionsShouldReturnTrue()
        {
            DALUtils util = new DALUtils();
            Assert.IsTrue(util.LoadCollections());
        }
        */

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestMethod]
        public void CallDAOComprehensiveTestsReturnTrue()
        {
            CallDAO dao = new CallDAO();
            Call call = new Call();
            call.DateOpened = DateTime.Now;
            call.DateClosed = null;
            call.OpenStatus = true;
            call.EmployeeId = new MongoDB.Bson.ObjectId("56464e723dd4df30e88b8b8c"); //Bigshot
            call.TechId = new MongoDB.Bson.ObjectId("56464e723dd4df30e88b8b92"); //Burner
            call.ProblemId = new MongoDB.Bson.ObjectId("56464e723dd4df30e88b8b99"); //memory
            call.Notes = "Bigshot has bad RAM, Burner to fix it";
            string newId = dao.Create(call);
            this.testContextInstance.WriteLine("New Call Id == " + newId);
            call = dao.GetByID(newId);
            this.testContextInstance.WriteLine("Call retrieved");
            call.Notes = call.Notes + "\nOrdered new RAM";

            if (dao.Update(call) == 1)
                this.testContextInstance.WriteLine("Call was updated");
            else
                Trace.WriteLine("Call was not updated");

            if (dao.Delete(newId))
                this.testContextInstance.WriteLine("Call was deleted ");
            else
                this.testContextInstance.WriteLine("Call was not deleted ");
            call = dao.GetByID(newId);
            Assert.IsNull(call);

        }

        [TestMethod]
        [ExpectedException(typeof(MongoDB.Driver.MongoException), "No Id exists")]
        public void CallViewModelComprehensiveTestsReturnTrue()
        {
            CallViewModel vm = new CallViewModel();
            vm.DateOpened = DateTime.Now;
            vm.OpenStatus = true;
            vm.EmployeeId = "56464e723dd4df30e88b8b8c"; // Bigshot
            vm.TechId = "56464e723dd4df30e88b8b92"; //Burner
            vm.ProblemId = "56464e723dd4df30e88b8b99";
            vm.Notes = "Bigshot has bad RAM, Burner to fix it";
            vm.Create();
            this.testContextInstance.WriteLine("New Call Id == " + vm.Id);
            vm.GetByID(vm.Id);
            this.testContextInstance.WriteLine("Call retrieved");
            vm.Notes = vm.Notes + "\nOrdered new Ram";

            if (vm.Update() == 1)
                this.testContextInstance.WriteLine("Call was updated " + vm.Notes);
            else
                Trace.WriteLine("Call was not updated");

            if (vm.Delete())
                this.testContextInstance.WriteLine("Call was deleted");
            else
                this.testContextInstance.WriteLine("Call was not deleted");

            vm.Update(); //should throw MongoException see attribute
        }
    }
}
