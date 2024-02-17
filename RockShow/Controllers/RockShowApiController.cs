using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RockShow.Domain.Rocks;
using RockShow.Interfaces;
using RockShow.Requests.Ratings;
using RockShow.Responses;
using RockShow.Services;

namespace RockShow.Controllers
{

    [Route("api/RockShow")]
    [ApiController]
    public class RockShowApiController : BaseApiController
    {
        private IRockServiceNew _rockService = null;

        public RockShowApiController(IRockServiceNew service,
            ILogger<RockShowApiController> logger) : base(logger)
        {
            _rockService = service;
        }

        [HttpPost]
        public ActionResult<ItemsResponse<int>> Add(AddRatingModel model)
        {
            ObjectResult result = null;

            try
            {
                int id = _rockService.Add(model);
                ItemsResponse<int> response = new ItemsResponse<int>() { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                Logger.LogError(ex.ToString());
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet]
        public ActionResult<ItemsResponse<RockModel>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                List<RockModel> list = _rockService.GetAll();
                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application Resource not found.");
                }
                else
                {
                    response = new ItemsResponse<RockModel> { Items = list };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }
    }
    
}




