using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;
using System.Collections.Generic;
using MongoDB.Bson;
using HelpdeskViewModels;

namespace HelpdeskTests
{
    [TestClass]
    public class ProblemTest
    {
        
        [TestMethod]
        public void ProblemDAOUpdateShouldReturnTrue()
        {
            ProblemDAO dao = new ProblemDAO();
            //simulate user 1 getting an Problem
            Problem prob = dao.GetByID("563d30b53dd4dd34b4c05cc2"); 
            prob.Description = "Device Not Turned On";
            int rowsUpdated = dao.Update(prob);
            //user 1 makes update
            Assert.IsTrue(rowsUpdated == 1);
        }

        [TestMethod]
        public void ProblemDAOUpdateTwiceShouldReturnNegative()
        {
            ProblemDAO dao = new ProblemDAO();
            //simulate 2 users getting an Problem
            Problem prob = dao.GetByID("563d30b53dd4dd34b4c05cc2"); 
            Problem prob2 = dao.GetByID("563d30b53dd4dd34b4c05cc2");
            prob.Description = "Device Not Turned On";
            int rowsUpdated = dao.Update(prob);
            if (rowsUpdated == 1)
            {
                rowsUpdated = dao.Update(prob2);
            }
            Assert.IsTrue(rowsUpdated == -2);
        }

        [TestMethod]
        public void ProblemDAOGetAllShouldReturnList()
        {
            ProblemDAO dao = new ProblemDAO();
            List<Problem> probs = dao.GetAll();
            Assert.IsTrue(probs.Count > 0);
        }

        [TestMethod]
        public void ProblemDAOCreateAndDeleteShouldReturnTrue()
        {
            bool deleteOk = false;
            Problem prob = new Problem();
            ProblemDAO dao = new ProblemDAO();
            prob.Description = "Spilt Mountain Dew Code red all over";
            string newid = dao.Create(prob);
            if (newid.Length == 24) // new id's are a 24 byte hex string
            {
                deleteOk = dao.Delete(newid);
            }
            Assert.IsTrue(deleteOk);
        }
        

        /////////////////////////////////////////////////  View Model Tests /////////////////////////////////////
        [TestMethod]
        public void ProblemVMUpdateShouldReturnTrue()
        {
            ProblemViewModel vm = new ProblemViewModel();
            // simulate user 1 getting an Problem
            vm.GetByID("563d30b53dd4dd34b4c05cc2"); // problems' id
            vm.Description = "Device Not Plugged In";
            int rowsUpdated = vm.Update();
            //user 1 makes update
            Assert.IsTrue(rowsUpdated == 1);
        }

        [TestMethod]
        public void ProblemVMUpdateTwiceShouldReturnNegative2()
        {
            ProblemViewModel vm = new ProblemViewModel();
            ProblemViewModel vm2 = new ProblemViewModel();
            // simulate 2 users getting an Problem
            vm.GetByID("563d30b53dd4dd34b4c05cc2"); // problems' id
            vm2.GetByID("563d30b53dd4dd34b4c05cc2"); // problems' id
            vm.Description = "Device Not Plugged In";
            int rowsUpdated = vm.Update();
            if (rowsUpdated == 1)
            {
                rowsUpdated = vm2.Update();
            }
            Assert.IsTrue(rowsUpdated == -2);
        }

        [TestMethod]
        public void ProblemVMGetAllShouldReturnList()
        {
            ProblemViewModel vm = new ProblemViewModel();
            List<ProblemViewModel> probs = vm.GetAll();
            Assert.IsTrue(probs.Count > 0);
        }

        [TestMethod]
        public void ProblemVMCreateAndDeleteShouldReturnTrue()
        {
            bool deleteOk = false;
            ProblemViewModel vm = new ProblemViewModel();
            // and some hardcoded data
            vm.Description = "some Description";
            vm.Create();

            if (vm.Id.Length == 24) //new id's are a 24 byte hex string
            {
                deleteOk = vm.Delete();
            }
            Assert.IsTrue(deleteOk);
        }
    }
}
