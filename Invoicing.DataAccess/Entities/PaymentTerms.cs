using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoicing.DataAccess.Entities
{
	public class PaymentTerms
	{
		public int PaymentTermsId { get; set; }

		public string Description { get; set; } = null!;

		public int DueDays { get; set; }

		public List<Invoice>? Invoices { get; set; }
	}
}
