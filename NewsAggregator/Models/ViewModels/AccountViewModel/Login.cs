using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models.ViewModels.AccountViewModel
{
    public class Login
    {
        [Required(ErrorMessage = "Please fill email field")]
        [DataType(DataType.EmailAddress)]

        public string Email { get; set; }

        [Required(ErrorMessage = "Please fill password field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}