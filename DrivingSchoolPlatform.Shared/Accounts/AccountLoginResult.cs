
namespace DrivingSchoolPlatform.Shared.Accounts
{
    public class AccountLoginResult
    {
        public AccountLoginResult(string token, string returnUrl)
        {
            Token = token;
            ReturnUrl = returnUrl;
        }

        public string Token { get; set; }
        public string ReturnUrl { get; set; }
    }
}
