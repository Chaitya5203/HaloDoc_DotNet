using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class login
    {
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string Usarname { get; set; } = null!;
        [Required(ErrorMessage = "Please Enter Password")]
        public string? Passwordhash { get; set; }
    }
}
