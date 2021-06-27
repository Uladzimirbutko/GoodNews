using System.ComponentModel.DataAnnotations;

namespace WebApplication.Request
{
    public class RegisterRequest
    {
        
        [RegularExpression(@".+\@.+\..+", ErrorMessage = "Поле заполнено некорректно")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Пароль введён некорректно.Должны присутсвовать цифры, буквы лытинского алфавита(строчные и заглавные) длина от 8 до 15 символов.")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password mismatch.")]
        public string PasswordConfirmation { get; set; }

        public string FullName { get; set; }

        [Range(18, 99, ErrorMessage = "Для пользования ресурсом возраст должен составлять от 18 до 99 лет")]
        public int Age { get; set; }
    }
}