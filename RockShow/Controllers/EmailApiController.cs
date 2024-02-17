using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RockShow.Domain.AppSettings;
using RockShow.Interfaces;
using RockShow.Requests.Email;
using RockShow.Responses;

namespace RockShow.Controllers
{
    [Route("api/emails")]
    [ApiController]
    public class EmailApiController : BaseApiController
    {
        IEmailService _service = null;
        private AppKeys _appKeys;

        public EmailApiController(IEmailService service, IOptions<AppKeys> appKeys, ILogger<BaseApiController> logger) : base(logger)
        {
            _service = service;
            _appKeys = appKeys.Value;
        }


        [AllowAnonymous]
        [HttpPost("contact")]
        public ActionResult<SuccessResponse> SendContactEmail(ContactUsRequest model)
        {
            int code = 201;
            BaseResponse response = null;
            try
            {
                _service.ContactUs(model);
                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                response = new ErrorResponse(ex.Message);
                code = 500;
            }


            return StatusCode(code, response);
        }



    }
}

