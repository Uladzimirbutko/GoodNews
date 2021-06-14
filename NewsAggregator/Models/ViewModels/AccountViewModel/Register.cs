using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models.ViewModels.AccountViewModel
{
    public class Register
    {
        [Required(ErrorMessage = "Please fill email field")]
        [RegularExpression("\\D")] // Regex validation
        [DataType(DataType.EmailAddress)] // ___@{}.{}
        public string Email { get; set; }

        [Required(ErrorMessage = "Please fill password field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password does not match")]
        public string PasswordConfirmation { get; set; }
    }
}