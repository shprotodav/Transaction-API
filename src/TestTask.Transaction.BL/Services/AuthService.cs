using System;
using System.Net.Http;
using System.Threading.Tasks;
using TestTask.Transaction.Common.Exceptions;
using TestTask.Transaction.Common.Extensions;

namespace TestTask.Transaction.BL.Services
{
    public interface IAuthService
    {
        Task ValidateToken(string token);
    }

    public class AuthService : IAuthService
    {
        private readonly HttpClient _client;

        public AuthService(HttpClient client)
        {
            _client = client;
        }

        public async Task ValidateToken(string token)
        {
            try
            {
                await _client.GetData("account/validatetoken", $"?token={token}");
            }
            catch
            {
                throw new BusinessLogicException("Invalid access token");
            }
        }
    }
}
