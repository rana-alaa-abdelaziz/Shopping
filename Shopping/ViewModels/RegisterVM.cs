using System.ComponentModel.DataAnnotations;

namespace Shopping.ViewModels
{
    public class RegisterVM
    {
        [Required, EmailAddress, StringLength(100, MinimumLength = 5)]
        public string Email { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
            ErrorMessage = "Password must include a special character (e.g. @, #, $).")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}