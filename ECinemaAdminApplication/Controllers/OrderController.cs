using ECinemaAdminApplication.Models;
using GemBox.Document;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Text;

namespace ECinemaAdminApplication.Controllers
{
    public class OrderController : Controller
    {

        public OrderController() 
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
        }
        public IActionResult Index()
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:44319/api/admin/GetAllActiveOrders";

            HttpResponseMessage response = client.GetAsync(URL).Result;

            var data = response.Content.ReadAsAsync<List<Order>>().Result;

            return View(data);
        }

        public IActionResult Details(Guid id)
        {
            HttpClient client = new HttpClient();

            string URL = "https://localhost:44319/api/admin/GetDetailsForOrder";
            //string URL2 = "https://localhost:44388/api/admin/GetAllActiveOrders";
            var model = new
            {
                Id = id
            };


            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");


            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            return View(result);
        }


        public FileContentResult CreateInvoice(Guid id)
        {

            HttpClient client = new HttpClient();

            string URL = "https://localhost:44319/api/admin/GetDetailsForOrder";
            var model = new
            {
                Id = id
            };


            HttpContent content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");


            HttpResponseMessage response = client.PostAsync(URL, content).Result;

            var result = response.Content.ReadAsAsync<Order>().Result;

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);

            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{Username}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();
            var total = 0.0;
            foreach (var item in result.TicketInOrders)
            {
                total += item.Quantity * item.OrderedTicket.TicketPrice;
                sb.AppendLine(item.OrderedTicket.MovieName + " with quantity of: " + item.Quantity + " and price of: $" + item.OrderedTicket.TicketPrice);

            }
            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", total.ToString());


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());


            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }


    }
}
