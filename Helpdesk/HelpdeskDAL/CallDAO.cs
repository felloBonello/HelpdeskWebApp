///<summary>
/// @purpose: Holds information about each employee and allows viewmodel to communicate with the database
///                         adds functions that allow for CRUD
/// @date: 2015-10-28
/// @author: Justin Bonello
/// @revisions: 4
///</summary>

using MongoDB.Bson;
using MongoDB.Kennedy;
using System;
using System.Collections.Generic;
using System.Linq;


namespace HelpdeskDAL
{
    public class CallDAO
    {
        public string Create(Call call)
        {
            string newid = "";

            try
            {
                DbContext ctx = new DbContext();
                ctx.Save(call, "calls");
                newid = call._id.ToString();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "Create");
            }

            return newid;
        }

        public Call GetByID(string id)
        {
            Call retCall = null;
            ObjectId callid = new ObjectId(id);

            try
            {
                DbContext _ctx;
                _ctx = new DbContext();
                Call call = _ctx.Calls.FirstOrDefault(c => c._id == callid);
                retCall = (Call)call;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "GetByID");
            }
            return retCall;
        }

        public List<Call> GetAll()
        {
            List<Call> allCalls = new List<Call>();

            try
            {
                DbContext ctx = new DbContext();
                allCalls = ctx.Calls.ToList();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "GetAll");
            }

            return allCalls;
        }

        public int Update(Call call)
        {
            int updateOK = -1;
            try
            {
                DbContext ctx = new DbContext();
                ctx.Save<Call>(call, "calls");
                updateOK = 1;
            }
            catch (MongoConcurrencyException ex)
            {
                updateOK = -2;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "Update");
            }

            return updateOK;
        }

        public bool Delete(string id)
        {
            bool deleteOk = false;
            ObjectId callid = new ObjectId(id);

            try
            {
                DbContext ctx = new DbContext();
                Call call = ctx.Calls.FirstOrDefault(c => c._id == callid);
                ctx.Delete<Call>(call, "calls");
                deleteOk = true;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "CallDAO", "Delete");
            }

            return deleteOk;
        }
    }
}