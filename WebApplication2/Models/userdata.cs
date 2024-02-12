using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Userdata
    {
        [Required(ErrorMessage = "First Name is required.")]
        [MinLength(5, ErrorMessage = "The First Name must be atleast 5 characters")]
        [MaxLength(15, ErrorMessage = "The First Name cannot be more than 15 characters")]
        public string first_name { get; set; } = null!;
        [Required(ErrorMessage = "Last Name is required.")]
        public string? last_name { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Email is incorrect")]
        public string email { get; set; } = null!;
        [Required(ErrorMessage = "Phone Number is required.")]
        public string? phonenumber { get; set; }
        [Required(ErrorMessage = "Phone Number is required.")]
        public string? street { get; set; }
        [Required(ErrorMessage = "dob is required.")]
        public string? dob { get; set; }
        [Required(ErrorMessage = "room Number is required.")]
        public string? room { get; set; }
        [Required(ErrorMessage = "city name is required.")]
        public string? city { get; set; }
        [Required(ErrorMessage = "state name is required.")]
        public string? state { get; set; }
        [Required(ErrorMessage = "zipcode is required.")]
        public string? zipcode { get; set; }
        [Required(ErrorMessage = "create date is required.")]
        public DateTime Createddate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "password is required.")]
        public string password { get; set; } = null!;
        [Required(ErrorMessage = "Confirm Password is required.")]
        public string cpassword { get; set; } = null!;
    }
}