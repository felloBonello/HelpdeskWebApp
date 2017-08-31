///<summary>
/// @purpose: Controller for the call javascript file to comminicate with call viewmodel
/// @author: Justin Bonello
/// @revisions: 8
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
    public class CallsController : ApiController
    {
        [Route("api/calls")]
        public IHttpActionResult Get()
        {
            try
            {
                CallViewModel call = new CallViewModel();
                List<CallViewModel> allCalls = call.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        [Route("api/calls/{Id}")]
        public IHttpActionResult Get(string Id)
        {
            try
            {
                CallViewModel call = new CallViewModel();
                call.Id = Id;
                call.GetByID(Id);
                return Ok(call);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        [Route("api/calls")]
        public IHttpActionResult Post(CallViewModel call)
        {
            try
            {
                call.Create();
                return Ok(call);
            }
            catch (Exception ex)
            {
                return BadRequest("Create failed - " + ex.Message);
            }
        }

        [Route("api/calls")]
        public IHttpActionResult Put(CallViewModel call)
        {
            try
            {
                if (call.Update() == 1)
                    return Ok("Call updated!");
                else if (call.Update() == -1)
                    return Ok("Call not updated!");
                else
                    return Ok("Data is stale, data not updated!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }

        [Route("api/calls/{Id}")]
        public IHttpActionResult Delete(string Id)
        {
            try
            {
                CallViewModel call = new CallViewModel();
                call.Id = Id;
                call.GetByID(Id);
                call.Delete();
                return Ok("Call deleted!");
            }
            catch (Exception ex)
            {
                return BadRequest("Update failed - " + ex.Message);
            }
        }
    }
}