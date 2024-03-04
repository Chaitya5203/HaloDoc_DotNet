namespace WebApplication2.Models
{
    public class AdminDashboardTableModel
    {
        public string physician;
        public string Name { get; set; }
        public string Requestor { get; set; }
        public string Phonenumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public int Requestid { get; set; }
        public int statusId { get; set; }
        public DateTime Requesteddate { get; set; }
        public int RequestTypeId { get; set; }
        public DateTime? Dateofservice { get; internal set; }
        public string RegionName { get; internal set; }
        public string RequestTypeName { get; internal set; }
    }
}
