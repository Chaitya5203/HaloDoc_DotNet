namespace WebApplication2.Models
{
    public class FamilyFriendPatientRequest
    {
        public string? f_first_name { get; set; }
        public string? f_last_name { get; set; }
        public string? f_phone_number { get; set; }
        public string? f_email { get; set; }
        public DateTime Createddate { get; set; }= DateTime.Now;
        public string p_first_name { get; set; } = null!;
        public string? p_last_name { get; set; }
        public string p_email { get; set; } = null!;
        public string? p_phonenumber { get; set; }
        public string? p_street { get; set; }
        public string? p_city { get; set; }
        public string? p_state { get; set; }
        public string? p_zip { get; set; }
        public string password { get; set; } = null!;
        public string cpassword { get; set; } = null!;
    }
}
