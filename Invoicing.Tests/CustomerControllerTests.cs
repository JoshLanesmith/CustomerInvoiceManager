using Invoicing.DataAccess.Entities;
using Invoicing.DataAccess.Services;
using JLanesmithMSWebTechA3.Controllers;
using JLanesmithMSWebTechA3.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Invoicing.Tests
{
	public class CustomerControllerTests
	{
		private readonly Mock<IInvoicingService> _mockInvoicingService = new Mock<IInvoicingService>();

		[Fact]
		public void GetCustomers_GET_ReturnCustomersViewModel()
		{
			//Arrange
			List<Customer> customers = new List<Customer>
			{
				new Customer { 
					CustomerId = 1, 
					Name = "A",
					Address1 = "A1",
					City = "A2",
					ProvinceOrState = "ON",
					ZipOrPostalCode = "H0H 0H0",
					Phone = "555-555-5555",
					IsDeleted = false 
				},
				new Customer {
					CustomerId = 2,
					Name = "B",
					Address1 = "B1",
					City = "B2",
					ProvinceOrState = "ON",
					ZipOrPostalCode = "H0H 0H0",
					Phone = "555-555-5555",
					IsDeleted = false
				},
			};

			_mockInvoicingService
				.Setup(invoicingService => invoicingService.GetCustomersFromTo(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(customers)
				;

			CustomerController controller = new CustomerController(_mockInvoicingService.Object);

			//Act
			ViewResult result = controller.GetCustomers("A", "E") as ViewResult;

			CustomersViewModel viewModel = result?.Model as CustomersViewModel;

			//Assert
			Assert.NotNull(viewModel);
			Assert.Equal(customers, viewModel?.Customers);
		}

		[Fact]
		public void AddCustomer_GET_ReturnsViewResult()
		{
			//Arrange
			CustomerController controller = new CustomerController(_mockInvoicingService.Object);

			//Act
			IActionResult result = controller.AddCustomer();

			//Assert
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void EditCustomer_GET_ReturnsNotFoundResult_WhenCustomerDoesNotExist()
		{
			//Arrange
			_mockInvoicingService
				.Setup(invoicingService => invoicingService.GetCustomerById(It.IsAny<int>()))
				.Returns((Customer)null);

			CustomerController controller = new CustomerController(_mockInvoicingService.Object);

			//Act
			IActionResult result = controller.EditCustomer(1);

			//Assert
			Assert.IsType<NotFoundResult>(result);
		}
	}
}