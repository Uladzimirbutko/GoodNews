using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models.ViewModels.AccountViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Email введён некоррентно.")]
        // Regex validation
        [DataType(DataType.EmailAddress)] // ___@{}.{}
        public string Email { get; set; }

        [Required(ErrorMessage = "Пароль введён некорректно.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string PasswordConfirmation { get; set; }

        [DataType(DataType.Date)]
        public int Age { get; set; }
        public string FullName { get; set; }

    }
}