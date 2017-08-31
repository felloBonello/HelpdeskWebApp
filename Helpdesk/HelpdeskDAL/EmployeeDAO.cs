///<summary>
/// @purpose: Holds information about each employee and allows weblayer to communicate with the database
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
using System.Text;
using System.Threading.Tasks;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        public string Create(Employee emp)
        {
            string newid = "";

            try
            {
                DbContext ctx = new DbContext();
                ctx.Save(emp, "employees");
                newid = emp._id.ToString();
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "Create");
            }

            return newid;
        }

        public Employee GetByID(string id)
        {
            Employee retEmp = null;
            ObjectId empid = new ObjectId(id);

            try
            {
                DbContext _ctx;
                _ctx = new DbContext();
                Employee emp = _ctx.Employees.FirstOrDefault(e => e._id == empid);
                retEmp = (Employee)emp;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "GetByID");
            }
            return retEmp;
        }

        public List<Employee> GetAll()
        {
            List<Employee> allEmps = new List<Employee>();

            try
            {
                DbContext ctx = new DbContext();
                allEmps = ctx.Employees.ToList();
            }
            catch(Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "GetAll");
            }

            return allEmps;
        }

        public int Update(Employee emp)
        {
            int updateOK = -1;
            try
            {
                DbContext ctx = new DbContext();
                ctx.Save<Employee>(emp, "employees");
                updateOK = 1;
            }
            catch (MongoConcurrencyException ex)
            {
                updateOK = -2;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "Update");
            }

                return updateOK;
        }

        public bool Delete(string id)
        {
            bool deleteOk = false;
            ObjectId empid = new ObjectId(id);

            try
            {
                DbContext ctx = new DbContext();
                Employee emp = ctx.Employees.FirstOrDefault(e => e._id == empid);
                ctx.Delete<Employee>(emp, "employees");
                deleteOk = true;
            }
            catch (Exception ex)
            {
                DALUtils.ErrorRoutine(ex, "EmployeeDAO", "Delete");
            }

            return deleteOk;
        }
    }
}
