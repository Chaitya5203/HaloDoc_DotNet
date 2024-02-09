namespace WebApplication2.Models
{
    public class Userdata
    {
        public string first_name { get; set; } = null!;
        public string? last_name { get; set; }
        public string email { get; set; } = null!;
        public string? phonenumber { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
        public DateTime Createddate { get; set; } = DateTime.Now;
    }
}