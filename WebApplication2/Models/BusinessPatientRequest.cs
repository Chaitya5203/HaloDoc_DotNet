namespace WebApplication2.Models
{
    public class BusinessPatientRequest
    {
        public string first_name { get; set; } = null!;
        public string? last_name { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string business_name { get; set; } = string.Empty;
        public string? case_no { get; set; }
        public string? symptoms { get; set; }
        public string? p_first_name { get; set; }
        public string? p_last_name { get; set; }
        public DateTime? dob { get; set; } 
        public string? p_email { get; set; }
        public string? p_phone { get; set; }
        public string? p_street { get; set; }
        public string? p_city { get; set; }
        public string? p_state { get; set; }
        public string? p_zip_code { get; set; }
        public DateTime Createddate { get; set; } = DateTime.Now;
        public string password { get; set; } = null!;
        public string cpassword { get; set; } = null!;
    }
}
