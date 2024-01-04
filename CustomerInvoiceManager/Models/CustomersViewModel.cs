using Invoicing.DataAccess.Entities;

namespace JLanesmithMSWebTechA3.Models
{
	public class CustomersViewModel
	{
        public List<Customer> Customers { get; set; }

        public string ActivePage { get; set; } = "A-E";
    }
}
