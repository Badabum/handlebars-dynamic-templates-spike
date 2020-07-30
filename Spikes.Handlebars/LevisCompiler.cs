using System;
using System.Linq;
using HandlebarsDotNet;

namespace Spikes.Handlebars
{
    public static class LevisCompiler
    {
        public static void RegisteHelpers()
        {
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