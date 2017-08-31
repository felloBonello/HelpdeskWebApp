using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class UtilController : ApiController
    {
        [Route("api/utils")]
        public IHttpActionResult Post()
        {
            try
            {
                ViewModelUtils utils = new ViewModelUtils();
                if (utils.LoadCollections())
                    return Ok("Collections have been reloading succesfully!");
                else
                    return Ok("Helpdesk failed to reload collections.");
            }
            catch (Exception ex)
            {
                return BadRequest("Create failed - " + ex.Message);
            }
        }
    }
}
