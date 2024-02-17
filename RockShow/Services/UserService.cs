using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB.DataProvider;
using RockShow.Domain.LookUps;
using RockShow.Domain.Users;
using RockShow.Interfaces;
using RockShow.Requests.Users;
using RockShow.Security;


namespace RockShow.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDatabaseProcCommands _data = null;
        private IEmailService _emailService;
        private ILookUpService _lookUpService;
        private string _connectionString = null;

        public UserService(IConfiguration configuration,
            IAuthenticationService<int> authService, IDatabaseProcCommands data,
            ILookUpService lookUpService, IEmailService emailService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _authenticationService = authService;
            _data = data;
            _lookUpService = lookUpService;
            _emailService = emailService;
        }

        public async Task<bool> LogInAsync(LogInAddRequest model)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(model.Email, model.Password);


            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }
            return isSuccessful;
        }

        public int Create(UserAddRequest model)
        {
            int userId = 0;
            string procName = "dbo.Users_Insert";
            string password = model.Password;
            string salt = BCrypt.BCryptHelper.GenerateSalt(12);
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

            TokenAddRequest newToken = new TokenAddRequest()
            {
                TokenId = Guid.NewGuid().ToString(),
                TokenType = 1
            };

            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
               inputParamMapper: delegate (SqlParameterCollection collection)
               {
                   collection.AddWithValue("@Email", model.Email);
                   collection.AddWithValue("@FirstName", model.FirstName);
                   collection.AddWithValue("@LastName", model.LastName);
                   collection.AddWithValue("@Mi", model.Mi);
                   collection.AddWithValue("@AvatarUrl", model.AvatarUrl);
                   collection.AddWithValue("@Password", hashedPassword);
                   collection.AddWithValue("@Token", newToken.TokenId);
                   collection.AddWithValue("@TokenType", newToken.TokenType);
                   collection.AddWithValue("@RoleId", model.RoleId);
                   SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int)
                   {
                       Direction = ParameterDirection.Output
                   };
                   collection.Add(idOut);
               }, returnParameters: delegate (SqlParameterCollection returnCollection) 
               {
                   object outPutId = returnCollection["@Id"].Value;
                   if (outPutId != DBNull.Value)
                   {
                       userId = Convert.ToInt32(outPutId);
                   }

               });
            _emailService.SendConfirm(model, newToken);

            return userId;
        }
        public UserBase ForgotPassTokenRequest(string email)
        {
            UserBase user = null;
            TokenAddRequest newToken = null;
            user = GetCurrentUserByEmail(email);

            if (user != null)
            {
                newToken = new TokenAddRequest()
                {
                    TokenId = Guid.NewGuid().ToString(),
                    TokenType = 2
                };

                InsertUserToken(user.Id, newToken);
                _emailService.SendPassReset(user, newToken);

            }
            return user;
        }

        public bool ChangePassword(PasswordUpdateRequest userPassUpdate)
        {
            TokenUpdateRequest tokenConfirm = ConfirmByToken(userPassUpdate.Token);
            bool result = false;
            string procName = "[dbo].[Users_UpdatePassword]";
            if (tokenConfirm != null)
            {
                if (tokenConfirm.UserId != 0 && tokenConfirm.TokenType == 2)
                {
                    string password = userPassUpdate.Password;
                    string salt = BCrypt.BCryptHelper.GenerateSalt();
                    string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

                    _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    collection.AddWithValue("@Id", tokenConfirm.UserId);
                    collection.AddWithValue("@Password", hashedPassword);
                }
                , returnParameters: null);

                    DeleteUserToken(userPassUpdate.Token);

                    result = true;
                }
            }
            return result;
        }
        public void UpdateAvatar(string avatarUrl, int id)
        {
            string procName = "[dbo].[Users_UpdateAvatar]";
            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
                    inputParamMapper: delegate (SqlParameterCollection collection)
                    {
                        collection.AddWithValue("@Id", id);
                        collection.AddWithValue("@AvatarUrl", avatarUrl);
                    }, returnParameters: null);

            return;
        }

        public List<User> SelectAll()
        {
            List<User> userList = null;
            string procName = "[dbo].[Users_SelectAll]";
            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
                 inputParamMapper: null,  // stored procedure doesn't require parameters, this can be null
                 singleRecordMapper: (IDataReader reader, short set) =>
                 {
                     User user = null;
                     int index = 0;
                     user = FullUserMapper(reader, out index);

                     if (userList == null)
                     {
                         userList = new List<User>();
                     }

                     userList.Add(user);

                 }
                , returnParameters: null);

            return userList;
        }
        public InitialUser SelectById(int id)
        {
            InitialUser user = null;
            string procName = "[dbo].[Users_SelectById]";
            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
                 inputParamMapper: delegate (SqlParameterCollection collection)  // stored procedure doesn't require parameters, this can be null

                 {
                     collection.AddWithValue("@Id", id);
                 }
                , singleRecordMapper: (IDataReader reader, short set) =>
                 {
                     int index = 0;
                     user = InitialUserMapper(reader, ref index);
                 }
                , returnParameters: null);

            return user;
        }
        public void InsertUserToken(int userId, TokenAddRequest tokenReq)
        {
            string procName = "[dbo].[UserTokens_Insert]";
            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
                 inputParamMapper: delegate (SqlParameterCollection collection)
                 {
                     collection.AddWithValue("@Id", userId);
                     collection.AddWithValue("@TokenType", tokenReq.TokenType);
                     collection.AddWithValue("@Token", tokenReq.TokenId);
                 }
                , returnParameters: null
                );

            return;
        }
        public bool ConfirmAccount(string token)
        {
            TokenUpdateRequest tokenConfirm = ConfirmByToken(token);

            if (tokenConfirm.UserId != 0 && tokenConfirm.TokenType == 1)
            {
                int statusId = 1;

                UpdateIsConfirmed(tokenConfirm.UserId);
                UpdateUserStatus(tokenConfirm.UserId, statusId);
                DeleteUserToken(tokenConfirm.TokenId);

                return true;
            }
            return false;
        }
        private UserBase GetCurrentUserByEmail(string email)
        {
            UserBase user = null;
            string procName = "[dbo].[Users_SelectByEmail]";
            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
               inputParamMapper: delegate (SqlParameterCollection collection)
               {
                   collection.AddWithValue("@Email", email);
               }, singleRecordMapper: delegate (IDataReader reader, short set)
                   {
                       int index = 0;
                       user = new UserBase();
                       user.Id = reader.GetInt32(index++);
                       user.Email = reader.GetString(index++);
                       user.Role = reader.GetString(index++);
                       user.TenantId = "Oracle Illusions Tenant";
                       user.AvatarUrl = reader.GetString(index++);
                   }, returnParameters: null);


            return user;
        }
        public InitialUser InitialUserMapper(IDataReader reader, ref int index)
        {
            InitialUser user = new InitialUser();

            user.Id = reader.GetInt32(index++);
            user.FirstName = reader.GetString(index++);
            user.LastName = reader.GetString(index++);
            user.Mi = reader.GetString(index++);
            user.AvatarUrl = reader.GetString(index++);
            user.Email = reader.GetString(index++);
            user.Role = reader.GetString(index++);

            return user;
        }
        public User FullUserMapper(IDataReader reader, out int index)
        {
            User user = new User() { Status = new LookUp() };
            index = 0;

            user.Id = reader.GetInt32(index++);
            user.FirstName = reader.GetString(index++);
            user.LastName = reader.GetString(index++);
            user.Mi = reader.GetString(index++);
            user.AvatarUrl = reader.GetString(index++);
            user.Email = reader.GetString(index++);
            user.IsConfirmed = reader.GetBoolean(index++);
            user.Status = _lookUpService.MapSingleLookUp(reader, ref index);
            user.DateCreated = reader.GetDateTime(index++);
            user.DateModified = reader.GetDateTime(index++);
            user.Role = _lookUpService.MapSingleLookUp(reader, ref index);

            return user;
        }
        private void DeleteUserToken(string tokenId)
        {
            string procName = "[dbo].[UserTokens_Delete_ByToken]";
            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
                inputParamMapper: delegate (SqlParameterCollection collection)
                {
                    collection.AddWithValue("@Token", tokenId);
                }
                , returnParameters: null
                );

            return;
        }
            private TokenUpdateRequest ConfirmByToken(string tokenIncoming)
            {
           
                string procName = "[dbo].[UserTokens_Select_ByToken]";

                TokenUpdateRequest tokenUpdateReq = null;

            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
            inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Token", tokenIncoming);
            },
            singleRecordMapper: (IDataReader reader, short set) =>
            {
                tokenUpdateReq = new TokenUpdateRequest();
                int index = 1;

                tokenUpdateReq.UserId = reader.GetInt32(index++);
                tokenUpdateReq.TokenType = reader.GetInt32(index++);
                tokenUpdateReq.TokenId = tokenIncoming;
            },
            returnParameters: null
            );

            return tokenUpdateReq;
            }
        public void UpdateUserStatus(int id, int statusId)
        {
            string procName = "[dbo].[Users_UpdateStatus]";
            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
               inputParamMapper: delegate (SqlParameterCollection collection)
               {
                   collection.AddWithValue("@Id", id);
                   collection.AddWithValue("@StatusId", statusId);
               }
                , returnParameters: null);

            return;
        }
        private void UpdateIsConfirmed(int userId)
        {
            string procName = "[dbo].[Users_Confirm]";
            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
              inputParamMapper: delegate (SqlParameterCollection collection)
            {
                collection.AddWithValue("@Id", userId);
            }
            , returnParameters: null);

            return;
        }
        public void UpdateUserInfo(BaseUserProfileUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Users_Update]";
            _data.ExecuteNonQuery(_connectionString, procName, CommandType.StoredProcedure,
              inputParamMapper: delegate (SqlParameterCollection collection)
              {
                  collection.AddWithValue("@Id", userId);
                  collection.AddWithValue("@FirstName", model.FirstName);
                  collection.AddWithValue("@LastName", model.LastName);
                  collection.AddWithValue("@Email", model.Email);
              });
        }
        private IUserAuthData Get(string email, string password)
        {
            string passwordFromDb = "";
            bool isConfirmed = false;
            UserBase user = null;
            string procName = "[dbo].[Users_SelectPass_ByEmail]";
            _data.ExecuteReader(_connectionString, procName, CommandType.StoredProcedure,
               inputParamMapper: delegate (SqlParameterCollection collection)
               {
                   collection.AddWithValue("@Email", email);
               }
                , singleRecordMapper: delegate (IDataReader reader, short set)
                  {
                      passwordFromDb = reader.GetString(0);
                      isConfirmed = reader.GetBoolean(1);
                  }
                , returnParameters: null);

            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);

            if (isValidCredentials && isConfirmed)
            {
                user = GetCurrentUserByEmail(email);
            }
            else if (isValidCredentials && !isConfirmed)
            {
                throw new Exception("Email address not confirmed! Please check your email and try again.");
            }

            return user;
        }
    }
}

