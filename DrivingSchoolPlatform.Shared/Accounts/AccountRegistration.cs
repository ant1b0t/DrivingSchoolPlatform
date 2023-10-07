
using System.ComponentModel.DataAnnotations;

namespace DrivingSchoolPlatform.Shared.Accounts
{
    public class AccountRegistration
    {
        public AccountRegistration(string phoneNumber, string fullName, string password, string passwordConfirm, string role)
        {
            PhoneNumber = phoneNumber;
            FullName = fullName;
            Password = password;
            PasswordConfirm = passwordConfirm;
            Role = role;
        }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
