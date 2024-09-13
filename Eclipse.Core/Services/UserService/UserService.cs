using Eclipse.Core.DTOs.UserDTOs;
using Eclipse.Core.Interfaces.IPassword;
using Eclipse.Core.Interfaces.IToken;
using Eclipse.Core.Interfaces.IUser;
using Eclipse.Core.Models;
using FluentValidation;

namespace Eclipse.Core.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IValidator<UserRegistration> _userRegistrationValidator;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IValidator<UserRegistration> userRegistrationValidator, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _userRegistrationValidator = userRegistrationValidator;
            _tokenService = tokenService;
        }
        public async Task<RegistrationResult> Register(UserRegistration userRegistration)
        {
            var registrationResult = new RegistrationResult();
            var validationResult = await _userRegistrationValidator.ValidateAsync(userRegistration);
            if (!validationResult.IsValid)
            {
                registrationResult.Result = false;
                registrationResult.Response = "Invalid input";
                return registrationResult;
            }
            var existingUser = await _userRepository.GetUserByUsername(userRegistration.Username);
            if (existingUser != null)
            {
                registrationResult.Result = false;
                registrationResult.Response = "Username already exists";
                return registrationResult;
            }
            existingUser = await _userRepository.GetUserByEmail(userRegistration.Email);
            if (existingUser != null)
            {
                registrationResult.Result = false;
                registrationResult.Response = "Email already exists";
                return registrationResult;
            }
            var user = new User
            {
                Username = userRegistration.Username,
                FirstName = userRegistration.FirstName,
                LastName = userRegistration.LastName,
                Email = userRegistration.Email,
                IsActive = true,
                PasswordHash = _passwordHasher.HashPassword(userRegistration.Password)
            };
            try
            {

                await _userRepository.CreateUser(user);
                registrationResult.Result = true;
                registrationResult.Response = "User registered successfully";
            }
            catch (Exception ex)
            {
                registrationResult.Result = false;
                registrationResult.Response = ex.Message;
            }
            return registrationResult;
        }
        public async Task<LoginResult> Login(UserLogin userLogin)
        {
            var loginResult = new LoginResult();
            var user = await _userRepository.GetUserByUsername(userLogin.Username);
            if (user == null)
            {
                loginResult.Result = false;
                loginResult.Response = "Invalid username";
                return loginResult;
            }
            if (!_passwordHasher.VerifyPassword(userLogin.Password, user.PasswordHash))
            {
                loginResult.Result = false;
                loginResult.Response = "Invalid password";
                return loginResult;
            }
            loginResult.Result = true;
            loginResult.Response = "Login successful";
            loginResult.Token = _tokenService.GenerateToken(user);
            return loginResult;
        }
    }
}
