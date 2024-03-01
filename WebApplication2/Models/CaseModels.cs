using System.ComponentModel.DataAnnotations;
using WebApplication2.Data;
namespace WebApplication2.Models
{
    public class CaseModels
    {
        public Requestclient Requestclient { get; set; }
        public List<Region> regions { get; set; }
        public List<Physician> physics { get; set; }
    }
}