using System.ComponentModel.DataAnnotations;

namespace LearnWithMentorDTO
{
    public class UserLoginDTO
    {
        public UserLoginDTO(string email, string password)
        {
            Email = email;
            Password = password;
            Role = role;
        }

        [Required]
        [RegularExpression(ValidationRules.EMAIL_REGEX,
            ErrorMessage = "Email not valid")]
        public string Email { set; get; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { set; get; }
    }
}