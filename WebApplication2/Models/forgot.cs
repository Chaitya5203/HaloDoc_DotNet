using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class forgot
    {
        [Required(ErrorMessage = "Please Enter First Name.")]
        public string email { get; set; } = null!;
    }
}
