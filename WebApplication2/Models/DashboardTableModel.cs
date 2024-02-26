using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class DashboardTableModel
    {
        public int Requesttypeid { get; set; }
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
        public int? Intdate { get; set; } 
        public string Strmonth { get; set; } = null!;
        public int? Intyear { get; set; }
        public string Phonenumber { get; set; } = null!;
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Notes { get; set; } = null!;
        public DateTime Createddate { get; set; } 
        public string Name { get; set; } = null!;
    }
}
    