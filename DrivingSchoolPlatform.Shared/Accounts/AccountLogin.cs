
using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolPlatform.Shared.Accounts
{
    public class AccountLogin
    {
        public AccountLogin(string phoneNumber, string password, bool rememberMe, string returnUrl)
        {
            PhoneNumber = phoneNumber;
            Password = password;
            RememberMe = rememberMe;
            ReturnUrl = returnUrl;
        }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = true;

        public string ReturnUrl { get; set; } = "/";
    }
}
