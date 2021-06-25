using System.ComponentModel.DataAnnotations;

namespace NewsAggregator.Models.ViewModels.AccountViewModel
{
    public class RegisterViewModel
    {
        
        [Required(ErrorMessage = "Введите Email.")]
        [EmailAddress(ErrorMessage = "Поле заполнено некорректно")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Введите пароль.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$" , ErrorMessage = "Пароль введён некорректно. Должны присутсвовать цифры, буквы лытинского алфавита(строчные и заглавные) длина от 8 до 15 символов.")  ]
        public string Password { get; set; }


        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают.")]
        public string PasswordConfirmation { get; set; }


        [Required(ErrorMessage = "Введите ваш возраст")]
        [Range(18, 99,ErrorMessage = "Для пользования ресурсом возраст должен составлять от 18 до 99 лет")]
        public int Age { get; set; }


        [Required(ErrorMessage = "Введите Ваше полное имя")]
        public string FullName { get; set;}

    }
}