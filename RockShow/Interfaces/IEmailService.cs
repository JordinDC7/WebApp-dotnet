using RockShow.Requests.Email;
using RockShow.Requests.Users;
using RockShow.Services;

namespace RockShow.Interfaces
{
    public interface IEmailService
    {
        Task ContactUs(ContactUsRequest model);
        Task SendConfirm(UserAddRequest userModel, TokenAddRequest token);
        Task SendPassReset(UserBase userModel, TokenAddRequest token);
    }
}