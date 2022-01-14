using System.ComponentModel.DataAnnotations;

namespace Gabriel.Models
{
    public class LoginViewModel
    {
        
        [Required(ErrorMessage = "Enter a valid username adress")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Enter a valid password")]
        public string Password { get; set; }
    }
}
