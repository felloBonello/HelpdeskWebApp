using MongoDB.Driver.Linq;
using System.Linq;
using System;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Kennedy;

namespace HelpdeskDAL
{
    public class DepartmentDAO
    {
        public string Create(Department dep)
        {
            string newid = "";

            try
            {
                DbContext ctx = new DbContext();
                ctx.Save(dep, "departments");
                newid = dep._id.ToString();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "Create");
            }

            return newid;
        }

        public Department GetByID(String depID)
        {
            Department retDep = null;
            ObjectId _id = new ObjectId(depID);

            try
            {
                DbContext _ctx;
                _ctx = new DbContext();
                Department dep = _ctx.Departments.FirstOrDefault(d => d._id == _id);
                retDep = (Department)dep;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "GetByID");
            }
            return retDep;
        }

        public List<Department> GetAll()
        {
            List<Department> allDeps = new List<Department>();

            try
            {
                DbContext ctx = new DbContext();
                allDeps = ctx.Departments.ToList();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "GetAll");
            }

            return allDeps;
        }

        public int Update(Department dep)
        {
            int updateOK = -1;
            try
            {
                DbContext ctx = new DbContext();
                ctx.Save<Department>(dep, "departments");
                updateOK = 1;
            }
            catch (MongoConcurrencyException ex)
            {
                updateOK = -2;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "Update");
            }

                return updateOK;
        }

        public bool Delete(string id)
        {
            bool deleteOk = false;
            ObjectId depid = new ObjectId(id);

            try
            {
                DbContext ctx = new DbContext();
                Department dep = ctx.Departments.FirstOrDefault(d => d._id == depid);
                ctx.Delete<Department>(dep, "departments");
                deleteOk = true;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "DepartmentDAO", "Delete");
            }

            return deleteOk;
        }
    }
}
