///<summary>
/// @purpose: Controller for the employee javascript file to comminicate with viewmodel
/// @date: 2015-10-28
/// @author: Justin Bonello
/// @revisions: 4
///</summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

//added for fileupload
using System.Web;
using System.IO;



using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class EmployeesController : ApiController
    {
        [Route("api/employees")]
        public IHttpActionResult Get()
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = emp.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        [Route("api/employees/{Id}")]
        public IHttpActionResult Get(string Id)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Id = Id;
                emp.GetByID(Id);
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        [Route("api/employees")]
        public IHttpActionResult Post(EmployeeViewModel emp)
        {
            try
            {
                emp.Create();
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return BadRequest("Create failed - " + ex.Message);
            }
        }

        [Route("api/employees")]
        public IHttpActionResult Put(EmployeeViewModel emp)
        {
            try
            {
                if (emp.Update() == 1)
                    return Ok("Employee " + emp.Lastname + " updated!");
                else if (emp.Update() == -1)
                    return Ok("Employee " + emp.Lastname + " not updated!");
                else
                    return Ok("Data is stale for " + emp.Lastname + " data not updated!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }

        [Route("api/employees/{Id}")]
        public IHttpActionResult Delete(string Id)
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Id = Id;
                emp.GetByID(Id);
                emp.Delete();
                return Ok("Employee deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }
    }
}