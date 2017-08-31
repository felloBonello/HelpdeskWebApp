using MongoDB.Driver.Linq;
using MongoDB.Bson;
using MongoDB.Kennedy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HelpdeskDAL
{
    public class ProblemDAO
    {
        public string Create(Problem prob)
        {
            string newid = "";

            try
            {
                DbContext ctx = new DbContext();
                ctx.Save(prob, "problems");
                newid = prob._id.ToString();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "Create");
            }

            return newid;
        }

        public Problem GetByID(String probID)
        {
            Problem retProb = null;
            ObjectId probid = new ObjectId(probID);

            try
            {
                DbContext _ctx;
                _ctx = new DbContext();
                Problem prob = _ctx.Problems.FirstOrDefault(p => p._id == probid);
                retProb = (Problem)prob;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "GetByID");
            }
            return retProb;
        }

        public List<Problem> GetAll()
        {
            List<Problem> allProbs = new List<Problem>();

            try
            {
                DbContext ctx = new DbContext();
                allProbs = ctx.Problems.ToList();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex,"ProblemDAO", "GetAll");
            }

            return allProbs;
        }

        public int Update(Problem prob)
        {
            int updateOK = -1;
            try
            {
                DbContext ctx = new DbContext();
                ctx.Save<Problem>(prob, "problems");
                updateOK = 1;
            }
            catch (MongoConcurrencyException ex)
            {
                updateOK = -2;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "Update");
            }

            return updateOK;
        }

        public bool Delete(string id)
        {
            bool deleteOk = false;
            ObjectId probid = new ObjectId(id);

            try
            {
                DbContext ctx = new DbContext();
                Problem prob = ctx.Problems.FirstOrDefault(p => p._id == probid);
                ctx.Delete<Problem>(prob, "problems");
                deleteOk = true;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "ProblemDAO", "Delete");
            }

            return deleteOk;
        }
    }
}
