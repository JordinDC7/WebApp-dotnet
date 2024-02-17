using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using RockShow.Responses;
using RockShow.Interfaces;

namespace RockShow.Controllers
{
    
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected ILogger Logger { get; set; }
        public BaseApiController(ILogger logger)
        {
            logger.LogInformation($"Controller Firing {this.GetType().Name} ");
            Logger = logger;
        }
        protected CreatedResult Created201(IItemResponse respone)
        {
            string url = Request.Path + "/" + respone.Item.ToString();

            return base.Created(url, respone);
        }
        protected OkObjectResult Ok200(BaseResponse respone)
        {

            return base.Ok(respone);
        }

        protected CreatedResult Created201(ItemsResponse<int> response)
        {
            string url = Request.Path + "/" + response.Item.ToString();

            return base.Created(url, response);
        }

        protected NotFoundObjectResult NotFound404(BaseResponse respone)
        {
            return base.NotFound(respone);
        }

        protected ObjectResult CustomResponse(HttpStatusCode code, BaseResponse response)
        {
            return StatusCode((int)code, response);
        }
    }
}