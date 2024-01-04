using Invoicing.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.DataAccess.Services
{
	public class InvoicingService : IInvoicingService
	{
		private readonly InvoicingDbContext _invoicingDbContext;

		public InvoicingService(InvoicingDbContext invoicingDbContext)
		{
			_invoicingDbContext = invoicingDbContext;
		}

		public List<Customer>? GetCustomersFromTo(string filterFrom = "A", string filterTo = "Z")
		{
			return _invoicingDbContext.Customers
                .Where(c => string.Compare(c.Name, filterFrom) >= 0 &&
					string.Compare(c.Name, filterTo) <= 0 &&
					c.IsDeleted == false
				)
                .Include(c => c.Invoices)
				.ToList();
		}

		public Customer? GetCustomerById(int customerId)
		{
			return _invoicingDbContext.Customers
				.Include(customer => customer.Invoices)
				.FirstOrDefault(c => c.CustomerId == customerId);
		}

		public Invoice? GetInvoiceById(int invoiceId)
		{
			return _invoicingDbContext.Invoices
				.Include(i => i.LineItems.OrderBy(l => l.InvoiceLineItemId))
				.Include(i => i.PaymentTerms)
				.FirstOrDefault(i => i.InvoiceId == invoiceId);
		}

		public List<PaymentTerms> GetPaymentTerms()
		{
			return _invoicingDbContext.PaymentTerms.ToList();
		}

		public void AddCustomer(Customer customer)
		{
			_invoicingDbContext.Customers.Add(customer);
			_invoicingDbContext.SaveChanges();
		}

        public void UpdateCustomer(Customer customer)
        {
            _invoicingDbContext.Customers.Update(customer);
            _invoicingDbContext.SaveChanges();
        }

		public int AddInvoice(Invoice invoice)
		{
			_invoicingDbContext.Invoices.Add(invoice);
            _invoicingDbContext.SaveChanges();

			return invoice.InvoiceId;
		}

		public void AddLineItem(InvoiceLineItem lineItem)
		{
			_invoicingDbContext.InvoiceLineItems.Add(lineItem);
			_invoicingDbContext.SaveChanges();
		}

        public void UpdateDeletingStatus(Customer customer)
		{
			customer.IsDeleted = !customer.IsDeleted;

			_invoicingDbContext.SaveChanges();
		}

	}
}
