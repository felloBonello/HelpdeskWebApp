///<summary>
/// @purpose: Controller for the problem javascript file to comminicate with viewmodel
/// @date: 2015-10-28
/// @author: Justin Bonello
/// @revisions: 5
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
    public class ProblemController : ApiController
    {
        [Route("api/problems")]
        public IHttpActionResult Get()
        {
            try
            {
                ProblemViewModel prob = new ProblemViewModel();
                List<ProblemViewModel> allProblems = prob.GetAll();
                return Ok(allProblems);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }


        [Route("api/problems/{Id}")]
        public IHttpActionResult Get(string Id)
        {
            try
            {
                ProblemViewModel prob = new ProblemViewModel();
                prob.Id = Id;
                prob.GetByID(Id);
                return Ok(prob);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        [Route("api/problems")]
        public IHttpActionResult Post(ProblemViewModel prob)
        {
            try
            {
                prob.Create();
                return Ok("Problem " + prob.Description + " added");
            }
            catch (Exception ex)
            {
                return BadRequest("Create failed - " + ex.Message);
            }
        }

        [Route("api/problems")]
        public IHttpActionResult Put(ProblemViewModel prob)
        {
            try
            {
                if (prob.Update() == 1)
                    return Ok("Problem " + prob.Description + " updated!");
                else if (prob.Update() == -1)
                    return Ok("Problem " + prob.Description + " not updated!");
                else
                    return Ok("Data is stale for " + prob.Description + " data not updated!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }

        [Route("api/problems/{Id}")]
        public IHttpActionResult Delete(string Id)
        {
            try
            {
                ProblemViewModel prob = new ProblemViewModel();
                prob.Id = Id;
                prob.GetByID(Id);
                prob.Delete();
                return Ok("Problem deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }
    }
}