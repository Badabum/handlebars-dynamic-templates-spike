using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HandlebarsDotNet;

namespace Spikes.Handlebars
{
    public class Page<T>
    {
        public Page(bool isFirst, bool isLast, ImmutableList<T> items)
        {
            IsFirst = isFirst;
            IsLast = isLast;
            Items = items;
        }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
        public ImmutableList<T> Items { get; set; }
    }
    public static class LevisCompiler
    {
        public static void RegisteHelpers()
        {
            HandlebarsBlockHelper pagination = (output, options, context, arguments) =>
            {
                // helper args format: collection pagesize optional<firstpage_size>
                List<OrderInvoiceItem> enumerable = arguments[0] as List<OrderInvoiceItem>;
                int pageSize = int.Parse(arguments[1] as string);
                int firstPageSize = arguments.Length > 2 ? int.Parse(arguments[2] as string) : pageSize;

                Page<OrderInvoiceItem> firstPage = new Page<OrderInvoiceItem>(
                    true, false, enumerable.Take(firstPageSize).ToImmutableList());
                ImmutableList<Page<OrderInvoiceItem>> pages = enumerable
                    .Skip(firstPageSize)
                    .Split(pageSize)
                    .Select((p, index) => new Page<OrderInvoiceItem>(false, false, p))
                    .ToImmutableList();
                pages.Last().IsLast = true;
                ImmutableList<Page<OrderInvoiceItem>> result = ImmutableList<Page<OrderInvoiceItem>>.Empty
                    .Add(firstPage)
                    .AddRange(pages);
                result.ForEach(p => options.Template(output, p));
            };
            HandlebarsDotNet.Handlebars.RegisterHelper("paging", pagination);
            HandlebarsDotNet.Handlebars.RegisterHelper("date", (writer, context, args) =>
            {
                DateTime dt = (DateTime)args[0];
                var format = args[1] as string;
                writer.WriteSafeString(dt.ToString(format));
            });
            HandlebarsDotNet.Handlebars.RegisterHelper("price", (writer, context, args) =>
            {
                string formated = args[0].GetType() switch
                {
                    {} d when d == typeof(double) => ((double)args[0]).ToString("0.00"),
                    {} i when i == typeof(int) => ((int)args[0]).ToString("0.00"),
                    {} dd when dd == typeof(decimal) => ((decimal)args[0]).ToString("0.00"),
                    _ => args[0].ToString()
                };
                writer.WriteSafeString(formated);
            });
        }
        public static string Compile(string templateContent)
        {
            var template = HandlebarsDotNet.Handlebars.Compile(templateContent);
            var model = new InvoiceModel()
            {
                CurrencySymbol = System.Globalization.NumberFormatInfo.GetInstance(System.Globalization.CultureInfo.GetCultureInfo("en-GB")).CurrencySymbol,
                BillingAddress = new Address()
                {
                    FirstName = "John doe",
                    Address1 = "Manhattan",
                    Address2 = "",
                    Address3 = "",
                    City = "New York",
                    Country = "USA",
                    PostalCode = "02149",
                    Telephone = "+1323123123123"
                },
                ShippingAddress = new Address()
                {
                    FirstName = "John doe",
                    Address1 = "Manhattan",
                    Address2 = "",
                    Address3 = "",
                    City = "New York",
                    Country = "USA",
                    PostalCode = "02149",
                    Telephone = "+1323123123123"
                },
                CustomerNo = "13213",
                IsCopy = true,
                OrderInvoiceItems = Enumerable.Range(0, 40)
                    .Select(_ => new OrderInvoiceItem()
                    {
                        ProductCode = "12312431",
                        Description = "This is the description",
                        LineTotal = _ * 42,
                        Size = "M",
                        Quantity = 2,
                        Vat = "1231231254"
                    }).ToList(),
                PaymentMethod = "PayPal",
                ShippingTotal = 13123123123,
                SubTotal = 1231313,
                Total = 314143432423,
                VatTotal = 123124141434,
                OrderNumber = "13213erer122",
                OrderDate = DateTime.UtcNow
            };
            return template(model);
        }
    }
}