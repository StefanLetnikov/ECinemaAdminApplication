using ECinemaAdminApplication.Models;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace ECinemaAdminApplication.Controllers
{
    public class UserController : Controller
    {
        [HttpGet("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("[action]")]
        public IActionResult ImportUsers(IFormFile file)
        {

            string pathToUpload = $"{Directory.GetCurrentDirectory()}\\files\\{file.FileName}";


            using (FileStream fileStream = System.IO.File.Create(pathToUpload))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }
            List<User> users = getAllUsersFromFile(file.FileName);

            HttpClient client = new HttpClient();

            string URL = "https://localhost:44386/api/admin/ImportAllUsers";
            string URL2 = "https://localhost:44388/api/admin/ImportAllUsers";


            HttpContent content = new StringContent(JsonConvert.SerializeObject(users), Encoding.UTF8, "application/json");


            HttpResponseMessage response = client.PostAsync(URL2, content).Result;

            //var result = response.Content.ReadAsAsync<bool>().Result;

            return RedirectToAction("Index","Order");
        }

        private List<User> getAllUsersFromFile(string fileName)
        {


            List<User> users = new List<User>();

            string filePath = $"{Directory.GetCurrentDirectory()}\\files\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using(var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using(var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while(reader.Read())
                    {
                        users.Add(new User()
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password = reader.GetValue(1).ToString(),
                            ConfirmPassword = reader.GetValue(2).ToString(),
                            PhoneNumber = reader.GetValue(3).ToString(),
                            FirstName  = reader.GetValue(4).ToString(),
                            LastName = reader.GetValue(5).ToString(),
                            UserName = reader.GetValue(6).ToString(),
                        });
                    }
                }
            }



            return users;
        }
    }
}
