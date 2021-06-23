using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models.ViewModels.AccountViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Пожалуйста введите коррентный Email")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

    }
}