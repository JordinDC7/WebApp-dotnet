using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RockShow.Controllers;
using RockShow.Domain.Users;
using RockShow.Interfaces;
using RockShow.Requests.Users;
using RockShow.Responses;
using RockShow.Security;
using RockShow.Security.Configs;
using RockShow.Services;


using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserApiController : BaseApiController
    {
        IUserService _userService = null;
        IAuthenticationService<int> _authService = null;
        ILookUpService _lookUpService = null;
        IEmailService _emailService = null;
        IOptions<SecurityConfig> _options;

        public UserApiController(IUserService userService
            , IAuthenticationService<int> authenticationService
            , ILookUpService lookUpService
            , IEmailService emailService
            , ILogger<UserApiController> logger
            , IOptions<SecurityConfig> options) : base(logger)
        {
            _userService = userService;
            _authService = authenticationService;
            _lookUpService = lookUpService;
            _emailService = emailService;
            _options = options;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<SuccessResponse>> LogInAsync(LogInAddRequest request)
        {
            ObjectResult result = null;

            try
            {
                bool successResponse = _userService.LogInAsync(request).Result;

                if (successResponse)
                {
                    ItemResponse<object> response = new ItemResponse<object>() { Item = successResponse };
                    result = Ok200(response);
                }
                else
                {
                    ErrorResponse response = new ErrorResponse("Login Failed! Please check credentials and try again.");
                    result = StatusCode(404, response);
                }
            }
            catch (System.Exception e)
            {
                ErrorResponse response = new ErrorResponse(e.Message);
                base.Logger.LogError(e.ToString());
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet("logout")]
        public async Task<ActionResult<SuccessResponse>> LogoutAsync()
        {
            await _authService.LogOutAsync();
            SuccessResponse response = new SuccessResponse();
            return Ok200(response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult<ItemResponse<int>> CreateUser(UserAddRequest request)
        {
            ObjectResult result = null;

            try
            {
                int newId = _userService.Create(request);

                if (newId != 0)
                {
                    ItemResponse<int> response = new ItemResponse<int>() { Item = newId };
                    result = Created201(response);
                }
                else
                {
                    ErrorResponse response = new ErrorResponse("404 Error Occurred");
                    result = StatusCode(404, response);
                }
            }
            catch (System.Exception ex)
            {
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpGet]
        public ActionResult<ItemResponse<User>> SelectAll()
        {

            BaseResponse response = null;
            int code = 200;

            try
            {
                List<User> list = _userService.SelectAll();

                if (list == null)
                {
                    code = 404;
                    response = new ErrorResponse("App resource not found.");
                }
                else
                {
                    response = new ItemsResponse<User> { Items = list };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpGet("current")]
        public ActionResult<ItemResponse<InitialUser>> GetCurrentUser()
        {
            InitialUser user = null;
            BaseResponse response = null;
            int code = 0;

            try
            {
                var test = _authService.IsLoggedIn();
                int id = _authService.GetCurrentUserId();
                user = _userService.SelectById(id);

                if (user != null)
                {
                    code = 200;
                    response = new ItemResponse<InitialUser>() { Item = user };
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("No user found.");
                }
            }
            catch (System.Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpPut("confirm")]
        [AllowAnonymous]
        public ActionResult ConfirmAccount(string tokenId)
        {
            int code = 0;
            BaseResponse response = null;
            bool result = false;
            try
            {
                result = _userService.ConfirmAccount(tokenId);

                if (result)
                {
                    code = 200;
                    response = new SuccessResponse();
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found: Please try again, or request a new confirmation link.");
                }
            }
            catch (System.Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }

        [HttpPost("forgot")]
        [AllowAnonymous]
        public ActionResult PassResetReq(string email)
        {
            UserBase user = null;
            int code = 0;
            BaseResponse response = null;

            try
            {
                user = _userService.ForgotPassTokenRequest(email);

                if (user != null)
                {
                    code = 200;
                    response = new SuccessResponse();
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("Email not found.");
                }
            }
            catch (System.Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpPut("changepassword")]
        [AllowAnonymous]
        public ActionResult ChangePasswordRequest(PasswordUpdateRequest userModel)
        {
            int code = 0;
            BaseResponse response = null;
            bool result;

            try
            {
                result = _userService.ChangePassword(userModel);

                if (result)
                {
                    code = 200;
                    response = new SuccessResponse();
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("Resource not found: Please try again, or request a new reset link.");
                }
            }
            catch (System.Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }

        [HttpPut("profileupdate")]
        public ActionResult<SuccessResponse> UpdateUserInfo(BaseUserProfileUpdateRequest model)
        {
            int code = 0;
            BaseResponse response = null;

            try
            {
                int userId = _authService.GetCurrentUserId();

                _userService.UpdateUserInfo(model, userId);
                response = new SuccessResponse();
            }
            catch (System.Exception ex)
            {

                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }
            return StatusCode(code, response);
        }



        [HttpPut("admin/dashboard/userStatus/deactivate/{id:int}")]
        public ActionResult<SuccessResponse> DeactivateUser(int id)
        {
            int code = 200;
            BaseResponse response = null;
            int statusCode = 2;

            try
            {
                _userService.UpdateUserStatus(id, statusCode);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }

            return StatusCode(code, response);
        }

        [HttpPut("update")]
        public ActionResult UserUpdateWizard(string avatarUrl)
        {
            int userId = _authService.GetCurrentUserId();
            int code = 0;
            BaseResponse response = null;

            try
            {
                if (userId != 0)
                {
                    _userService.UpdateAvatar(avatarUrl, userId);
                    code = 200;
                    response = new SuccessResponse();
                }
                else
                {
                    code = 404;
                    response = new ErrorResponse("404: User not found");
                }
            }
            catch (System.Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(code, response);
        }
    }
}
