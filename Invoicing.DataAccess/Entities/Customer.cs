using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.DataAccess.Entities
{
	public class Customer
	{
		public int CustomerId { get; set; }

        [Display(Name = "Name:*")]
        [Required(ErrorMessage = "Required: Customer Name")]
		public string Name { get; set; } = null!;

        [Display(Name = "Address 1:*")]
        [Required(ErrorMessage = "Required: Address")]
        public string? Address1 { get; set; }

        [Display(Name = "Address 2:")]
        public string? Address2 { get; set; }

        [Display(Name = "City:*")]
        [Required(ErrorMessage = "Required: City")]
        public string? City { get; set; } = null!;

        [Display(Name = "Province/State:*")]
        [Required(ErrorMessage = "Required: Province/State Code")]
		[RegularExpression(@"^[a-zA-Z]{2}$", ErrorMessage = "Invalid Province/State Code")]
        public string? ProvinceOrState { get; set; } = null!;

        [Display(Name = "Postal/Zip Code:*")]
        [Required(ErrorMessage = "Required: Postal/Zip Code")]
        public string? ZipOrPostalCode { get; set; } = null!;

        [Display(Name = "Phone:*")]
        [Required(ErrorMessage = "Required: Phone Number")]
        [RegularExpression(@"^(\+1)?[-. ]?\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }

        [Display(Name = "Contact Last Name:")]
        public string? ContactLastName { get; set; }

        [Display(Name = "Contact First Name:")]
        public string? ContactFirstName { get; set; }

        [Display(Name = "Contact Email:")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? ContactEmail { get; set; }

		public bool IsDeleted { get; set; } = false;

		public List<Invoice>? Invoices { get; set; }
	}
}
