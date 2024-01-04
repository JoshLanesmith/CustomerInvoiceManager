using Invoicing.DataAccess.Entities;
using Invoicing.DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using JLanesmithMSWebTechA3.Models;

namespace JLanesmithMSWebTechA3.Controllers
{
	public class CustomerController : Controller
	{
		private readonly IInvoicingService _invoicingService;

		public CustomerController(IInvoicingService invoicingService)
		{
			_invoicingService = invoicingService;
		}

		[HttpGet("/customers/{filterFrom}-{filterTo}")]
		public IActionResult GetCustomers(string filterFrom, string filterTo)
		{
			List<Customer> customers = _invoicingService.GetCustomersFromTo(filterFrom, filterTo);

			for (int i = 0; i < customers.Count(); i++)
			{
				if (customers[i].Invoices != null && customers[i].Invoices.Count() > 0)
				{
					for (int j = 0; j < customers[i].Invoices.Count(); j++)
					{
						customers[i].Invoices[j] = _invoicingService.GetInvoiceById(customers[i].Invoices[j].InvoiceId);

					}
					customers[i].Invoices = customers[i].Invoices.OrderBy(i => i.InvoiceDueDate).ToList(); 
				}
			}

			CustomersViewModel customersViewModel = new CustomersViewModel()
			{
				Customers = customers,
				ActivePage = $"{filterFrom}-{filterTo}"
			};

			return View("List", customersViewModel);
		}

		[HttpGet("/customers/add")]
		public IActionResult AddCustomer()
		{
			Customer customer = new Customer();

			CustomerViewModel customerViewModel = new CustomerViewModel()
			{
				Customer = customer
			};

			return View("Add", customerViewModel);
		}

		[HttpPost("/customers/add")]
		public IActionResult AddCustomer(CustomerViewModel customerViewModel)
		{
			if (ModelState.IsValid)
			{
				_invoicingService.AddCustomer(customerViewModel.Customer);
				return RedirectToAction("GetCustomers", new { filterFrom = "A", filterTo = "E" });
			}
			else
			{
				return View("Add", customerViewModel);
			}
		}

		[HttpGet("/customers/{customerId:int}/edit")]
		public IActionResult EditCustomer(int customerId)
		{
			Customer customer = _invoicingService.GetCustomerById(customerId);

			if (customer == null)
			{
				return NotFound();
			}




			CustomerViewModel customerViewModel = new CustomerViewModel()
			{
				Customer = customer
			};

			return View("Edit", customerViewModel);
		}

		[HttpPost("/customers/{customerId:int}/edit")]
		public IActionResult EditCustomer(CustomerViewModel customerViewModel)
		{
			if (ModelState.IsValid)
			{
				_invoicingService.UpdateCustomer(customerViewModel.Customer);
				return RedirectToAction("GetCustomers", new { filterFrom = "A", filterTo = "E" });
			}
			else
			{
				return View("Edit", customerViewModel);
			}
		}

		[HttpGet("/customers/{customerId:int}/invoices/{invoiceId:int}")]
		public IActionResult GetCustomerInvoices(int customerId, int invoiceId)
		{
			Customer customer = _invoicingService.GetCustomerById(customerId);

			if (customer == null)
			{
				return NotFound();
			}

            Invoice selectedInvoice = new Invoice();
            if (invoiceId != -1)
            {
                selectedInvoice = _invoicingService.GetInvoiceById(invoiceId);

            }

            List<PaymentTerms> paymentTerms = _invoicingService.GetPaymentTerms();

			CustomerInvoiceViewModel customerInvoiceViewModel = new CustomerInvoiceViewModel()
			{
				Customer = customer,
				ActiveInvoice = selectedInvoice,
				PaymentTerms = paymentTerms
			};

			return View("Invoices", customerInvoiceViewModel);
		}

		[HttpGet("/customers/{customerId:int}/delete")]
		public IActionResult Delete(int customerId)
		{
			Customer customer = _invoicingService.GetCustomerById(customerId);

			if (customer == null)
			{
				return NotFound();
			}

			_invoicingService.UpdateDeletingStatus(customer);

			TempData["Message"] = $"{customer.Name} is deleted!";
			TempData["ClassName"] = "danger";
			TempData["CustomerId"] = customer.CustomerId;

			return RedirectToAction("GetCustomers", new { filterFrom = "A", filterTo = "E" });
		}

		[HttpGet("/customers/{customerId:int}/undo-delete")]
		public IActionResult UndoDelete(int customerId)
		{
			Customer customer = _invoicingService.GetCustomerById(customerId);

			if (customer == null)
			{
				return NotFound();
			}

			_invoicingService.UpdateDeletingStatus(customer);

			TempData["Message"] = $"{customer.Name} is restored!";
			TempData["ClassName"] = "success";

			return RedirectToAction("GetCustomers", new { filterFrom = "A", filterTo = "E" });
		}

		[HttpPost()]
		public IActionResult AddInvoice(CustomerInvoiceViewModel customerInvoiceViewModel)
		{
			int newInvId = 0;
			if (customerInvoiceViewModel.NewInvoice.InvoiceDate != null &&
				customerInvoiceViewModel.NewInvoice.PaymentTermsId != -1)
			{
				newInvId = _invoicingService.AddInvoice(customerInvoiceViewModel.NewInvoice);
			}

			int activeInvoiceId = customerInvoiceViewModel.ActiveInvoice.InvoiceId != 0 ? customerInvoiceViewModel.ActiveInvoice.InvoiceId : newInvId;

			return RedirectToAction("GetCustomerInvoices",
				new
				{
					customerId = customerInvoiceViewModel.NewInvoice.CustomerId,
					invoiceId = activeInvoiceId
				}
				);

		}

		[HttpPost()]
		public IActionResult AddLineItem(CustomerInvoiceViewModel customerInvoiceViewModel)
		{
			if (!string.IsNullOrEmpty(customerInvoiceViewModel.NewLineItem.Description) &&
				customerInvoiceViewModel.NewLineItem.Amount > 0)
			{
				_invoicingService.AddLineItem(customerInvoiceViewModel.NewLineItem);
			}
			return RedirectToAction("GetCustomerInvoices",
				new
				{
					customerId = customerInvoiceViewModel.Customer.CustomerId,
					invoiceId = customerInvoiceViewModel.ActiveInvoice.InvoiceId
				}
				);

		}
	}
}
