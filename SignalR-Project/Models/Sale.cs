namespace SignalR_Project.Models
{
    public class Sale
	{
		public int Id { get; set; }
		public int? EmployeeId { get; set; }
		public virtual Employee Employee { get; set; }
		public decimal Amount { get; set; }
	}
}

