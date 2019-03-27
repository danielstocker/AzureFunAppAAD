using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AzureFunASPNetCoreAAD.Models;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;

namespace AzureFunASPNetCoreAAD.Controllers
{
    public class HomeController : Controller
    {
        public async System.Threading.Tasks.Task<IActionResult> Index()
        {

            string authority = "https://login.microsoftonline.com/tenant-id"; //login.microsoftonline.com followed by tenant name of id
            string resource = "target-resource-app-id";  //Application id of the server AD app
            string clientId = "client-resource-app-id"; //Application id of the client AD app
            string clientSecret = @"secret-of-the-client-app"; //Secret key from the client AD app

            try
            {
                // create auth context
                AuthenticationContext authContext = new AuthenticationContext(authority);
                ClientCredential clientCred = new ClientCredential(clientId, clientSecret);

                var token = await authContext.AcquireTokenAsync(resource, clientCred);
                var authHeader = token.CreateAuthorizationHeader();

                // attach the auth context to a POST request to send credentials up
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.AccessTokenType, token.AccessToken);
         
                Uri requestUri = new Uri("https://funfunction.azurewebsites.net/api/HttpTrigger1");
                var content = new StringContent(@"{ ""name"": ""World"" }", System.Text.Encoding.Default, @"application/json");
                var result = await client.PostAsync(requestUri, content);

                this.ViewData["Data"] = await result.Content.ReadAsStringAsync();

                // continue without auth. this will send a GET request without using the credentials
                client = new HttpClient();
                requestUri = new Uri("https://funfunction.azurewebsites.net/api/HttpTrigger1?name=Globe");
                result = await client.GetAsync(requestUri);

                this.ViewData["Data2"] = await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CallWebApiUnprotectedAsync(): " + ex.Message);
            }


            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
