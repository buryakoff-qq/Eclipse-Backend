using Eclipse.Core.DTOs.UserDTOs;

namespace Eclipse.Core.Interfaces.IUser
{
    public interface IUserService
    {
        public Task<RegistrationResult> Register(UserRegistration userRegistration);
        public Task<LoginResult> Login(UserLogin userLogin);
    }
}
