using Identity.Services.Contracts;

namespace Identity.Services
{
    public class IdentityService : IIdentityService
    {
        public Task<string> Login(string userName, string password, string secret)
        {
            throw new NotImplementedException();
        }

        public Task<string> Register(string userName, string password, string confirmPassword)
        {
            throw new NotImplementedException();
        }
    }
}
