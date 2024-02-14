using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Data;

namespace WebApplication2.Models
{
    public class profile    
    {
        public Request Request { get; set; }
        public int Requestid { get; set; }
        public int Requesttypeid { get; set; }
        public int? Userid { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Phonenumber { get; set; }
        public string? Email { get; set; }
        public short Status { get; set; }
        public int? Physicianid { get; set; }
        public string? Confirmationnumber { get; set; }
        public DateTime Createddate { get; set; }
        public BitArray? Isdeleted { get; set; }
        public DateTime? Modifieddate { get; set; }
        public string? Declinedby { get; set; }
        public BitArray Isurgentemailsent { get; set; } = null!;
        public DateTime? Lastwellnessdate { get; set; }
        public BitArray? Ismobile { get; set; }
        public short? Calltype { get; set; }
        public BitArray? Completedbyphysician { get; set; }
        public DateTime? Lastreservationdate { get; set; }
        public DateTime? Accepteddate { get; set; }
        public string? Relationname { get; set; }
        public string? Casenumber { get; set; }
        public string? Ip { get; set; }
        public string? Casetag { get; set; }
        public string? Casetagphysician { get; set; }
        public string? Patientaccountid { get; set; }
        public int? Createduserid { get; set; }
        public virtual ICollection<Emaillog> Emaillogs { get; set; } = new List<Emaillog>();
        public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();
        public virtual ICollection<Requestbusiness> Requestbusinesses { get; set; } = new List<Requestbusiness>();
        public virtual ICollection<Requestclient> Requestclients { get; set; } = new List<Requestclient>();
        public virtual ICollection<Requestclosed> Requestcloseds { get; set; } = new List<Requestclosed>();
        public virtual ICollection<Requestconcierge> Requestconcierges { get; set; } = new List<Requestconcierge>();
        public virtual ICollection<Requestnote> Requestnotes { get; set; } = new List<Requestnote>();
        public virtual ICollection<Requeststatuslog> Requeststatuslogs { get; set; } = new List<Requeststatuslog>();
        public virtual Requesttype Requesttype { get; set; } = null!;
        public virtual ICollection<Requestwisefile> Requestwisefiles { get; set; } = new List<Requestwisefile>();
        public virtual ICollection<Smslog> Smslogs { get; set; } = new List<Smslog>();
        public string first_name { get; set; } = null!;
        public string? last_name { get; set; }
        public string email { get; set; } = null!;
        public string? phonenumber { get; set; }
        public string? street { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? zipcode { get; set; }
    }
}
