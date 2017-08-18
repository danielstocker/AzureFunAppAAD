using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FunAppAdClient
{
    class Program
    {
        static void Main(string[] args)
        {
            CallFunction(null).GetAwaiter().GetResult();
        }
        static async Task CallFunction(string[] args)
        {
            var webApiUrl = "https://funappad.azurewebsites.net/api/HttpTriggerCSharp1?name=test"; //url of the Azure Function
            string authority = "https://login.microsoftonline.com/microsoft.onmicrosoft.com"; //https://login.microsoftonline.com/<AADTenant>
            string resource = "0fd86ddc-c6b2-4cd7-b542-2b1ce11e5100";  //Application id of the server AD app
            string clientId = "51bdeb1e-46cc-4f3c-98cd-4ac1eb32d821"; //Application id of the client AD app
            string clientSecret = "------"; //Secret key from the client AD app
          
            try
            {
                AuthenticationContext authContext = new AuthenticationContext(authority);
                ClientCredential clientCred = new ClientCredential(clientId, clientSecret);

                var token = await authContext.AcquireTokenAsync(resource, clientCred);
                var authHeader = token.CreateAuthorizationHeader();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(token.AccessTokenType, token.AccessToken);
                Uri requestURI = new Uri(webApiUrl);
                Console.WriteLine($"Reading values from '{requestURI}'.");
                HttpResponseMessage httpResponse = await client.GetAsync(requestURI);
                Console.WriteLine($"HTTP Status Code: '{httpResponse.StatusCode.ToString()}'");
                Console.WriteLine($"HTTP Response: '{httpResponse.ToString()}'");
                string responseString = await httpResponse.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CallWebApiUnprotectedAsync(): " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}
