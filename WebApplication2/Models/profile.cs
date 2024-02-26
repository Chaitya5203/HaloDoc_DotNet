using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication2.Data;
using System.Linq;

namespace WebApplication2.Models
{
    public class profile    
    {
        public List<Request>? Request { get; set; }
        public User? User { get; set; }    
        public DateOnly? DOB { get; set; }
    }
}
