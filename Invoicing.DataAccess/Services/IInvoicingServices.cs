using Invoicing.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.DataAccess.Services
{
	public interface IInvoicingService
	{
		public List<Customer>? GetCustomersFromTo(string filterFrom = "A", string filterTo = "Z");
		public Customer? GetCustomerById(int customerId);
		public Invoice? GetInvoiceById(int invoiceId);
		public List<PaymentTerms> GetPaymentTerms();
		public void AddCustomer(Customer customer);
		public void UpdateCustomer(Customer customer);
		public int AddInvoice(Invoice invoice);
		public void AddLineItem(InvoiceLineItem lineItem);
		public void UpdateDeletingStatus(Customer customer);
	}
}
