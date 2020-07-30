using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Spikes.Handlebars
{
    public class InvoiceModel
    {
        public string CurrencySymbol { get; set; }
        public bool IsCopy { get; set; }
        public Address BillingAddress { get; set; }
        public Address ShippingAddress { get; set; }
        public string CustomerNo { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderInvoiceItem> OrderInvoiceItems { get; set; }
        public double SubTotal { get; set; }
        public double ShippingTotal { get; set; }
        public double VatTotal { get; set; }
        public double Total { get; set; }
        public ImmutableList<ImmutableList<OrderInvoiceItem>> Pages => MakePages();

        private ImmutableList<ImmutableList<OrderInvoiceItem>> MakePages()
        {
            const int firstPageSize = 6;
            const int otherPagesSize = 16;
            ImmutableList<ImmutableList<OrderInvoiceItem>> items = ImmutableList<ImmutableList<OrderInvoiceItem>>.Empty;
            items = items.Add(OrderInvoiceItems.Take(firstPageSize).ToImmutableList());
            items = items.AddRange(OrderInvoiceItems
                .Skip(firstPageSize)
                .Split(otherPagesSize));
            return items;
        }
    }

    public class Address
    {
        public string FirstName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
    }

    public class OrderInvoiceItem
    {
        public string ProductCode { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public string Vat { get; set; }
        public int LineTotal { get; set; }
    }
}