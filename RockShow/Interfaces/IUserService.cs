using RockShow.Domain.Users;
using RockShow.Requests.Users;
using RockShow.Services;
using System.Data;

namespace RockShow.Interfaces
{
    public interface IUserService
    {
        bool ChangePassword(PasswordUpdateRequest userPassUpdate);
        bool ConfirmAccount(string token);
        int Create(UserAddRequest model);
        UserBase ForgotPassTokenRequest(string email);
        User FullUserMapper(IDataReader reader, out int index);
        InitialUser InitialUserMapper(IDataReader reader, ref int index);
        void InsertUserToken(int userId, TokenAddRequest tokenReq);
        Task<bool> LogInAsync(LogInAddRequest model);
        List<User> SelectAll();
        InitialUser SelectById(int id);
        void UpdateAvatar(string avatarUrl, int id);
        void UpdateUserInfo(BaseUserProfileUpdateRequest model, int userId);
        void UpdateUserStatus(int id, int statusId);
    }
}