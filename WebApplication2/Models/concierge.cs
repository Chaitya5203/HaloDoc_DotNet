namespace WebApplication2.Models
{
    public class ConciergePatientRequest
    {
        public string cname { get; set; } = null!;
        public string cstreet { get; set; } = null!;
        public string ccity { get; set; } = null!;
        public string cstate { get; set; } = null!;
        public string czip { get; set; } = null!;
        public int Regionid { get; set; } = 1;
        public DateTime Createddate { get; set; } = DateTime.Now;
        public string first_name { get; set; } = null!;
        public string? last_name { get; set; }
        public string pemail { get; set; } = null!;
        public string? Phonenumber { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
