using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace cheque_data_provider_API
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserRepository _userRepository;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock, IUserRepository userRepository) :
           base(options, logger, encoder, clock)
        {
            _userRepository = userRepository;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorizationHeader.Substring("Basic ".Length).Trim();
                var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialsAsEncodedString.Split(':');

                if (await _userRepository.Authenticate(credentials[0], credentials[1]))
                {
                    var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }
            }

            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\".com\"");

            return await Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
    public interface IUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public interface IUserRepository
    {
        Task<bool> Authenticate(string username, string password);
    }

    public class User : IUser
    {
        private string _username;
        private string _password;

        public User()
        {
            _username = String.Empty;
            _password = String.Empty;
        }

        public int Id { get; set; }
        public string Username { get => _username; set => _username = value; }
        public string Password { get => _password; set => _password = value; }
    }
    public class UserRepository : IUserRepository
    {
        private List<User> _users = new List<User>
        {
            new User
            {
                Id = 1, Username = "test", Password = "123@abc"
            }
        };

        public async Task<bool> Authenticate(string username, string password)
        {
            if (await Task.FromResult(_users.SingleOrDefault(x => x.Username == username && x.Password == password)) != null)
            {
                return true;
            }

            return false;
        }
    }
}
