using Invoicing.DataAccess.Entities;

namespace JLanesmithMSWebTechA3.Models
{
    public class CustomerInvoiceViewModel
    {
        public Customer Customer { get; set; }

        public Invoice ActiveInvoice { get; set; }

        public List<PaymentTerms>? PaymentTerms { get; set; }

        public Invoice? NewInvoice { get; set; }

        public InvoiceLineItem? NewLineItem { get; set; }
    }
}
