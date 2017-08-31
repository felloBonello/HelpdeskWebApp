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
    public class DepartmentController : ApiController
    {
        [Route("api/departments")]
        public IHttpActionResult Get()
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                List<DepartmentViewModel> allDepartments = dep.GetAll();
                return Ok(allDepartments);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }


        [Route("api/departments/{Id}")]
        public IHttpActionResult Get(string Id)
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                dep.Id = Id;
                dep.GetByID(Id);
                return Ok(dep);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        [Route("api/departments")]
        public IHttpActionResult Post(DepartmentViewModel dep)
        {
            try
            {
                dep.Create();
                return Ok("Department " + dep.DepartmentName + " added");
            }
            catch (Exception ex)
            {
                return BadRequest("Create failed - " + ex.Message);
            }
        }

        [Route("api/departments")]
        public IHttpActionResult Put(DepartmentViewModel dep)
        {
            try
            {
                if (dep.Update() == 1)
                    return Ok("Department " + dep.DepartmentName + " updated!");
                else if (dep.Update() == -1)
                    return Ok("Department " + dep.DepartmentName + " not updated!");
                else
                    return Ok("Data is stale for " + dep.DepartmentName + " data not updated!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }

        [Route("api/departments/{Id}")]
        public IHttpActionResult Delete(string Id)
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                dep.Id = Id;
                dep.GetByID(Id);
                dep.Delete();
                return Ok("Department deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }
    }
}
