using System;

namespace NewsAggregator.Models.ViewModels.User
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public int Age { get; set; }

    }
}