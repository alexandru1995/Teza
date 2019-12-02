using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using TestIntegrationApplication.Helpers;
using TestIntegrationApplication.Models;

namespace TestIntegrationApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IJwtToken _token;
        private readonly AuthenticationRequestModel _options;
        public HomeController(IJwtToken token, IOptions<AuthenticationRequestModel> options, IHttpClientFactory clientFactory)
        {
            _token = token;
            _options = options.Value;
            _clientFactory = clientFactory;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            var payload = new Dictionary<string, object>
            {
                {"scope", "Authorization"},
                {"response_type", "code"},
                {"client_id", _options.Client_Id},
                {"redirect_uri", $"{this.Request.Host}"}
            };

            ViewData["clientID"] = _token.Create(payload);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("OnLogin")]
        public async Task<IActionResult> OnLogin()
        {
            HttpContext.Request.Form.TryGetValue("AuthorizationCode", out var token);
            try
            {
                var key = Encoding.ASCII.GetBytes(_options.ServerSecret);
                JWT.Decode(token, key);
                var payload = JWT.Payload(token);
                var serviceData = JObject.Parse(payload);
                var authorizationCode = serviceData.GetValue("AuthorizationCode");
                var newPayload = new Dictionary<string, object>
                {
                    {"AuthorizationCode", authorizationCode},
                    {"client_id", _options.Client_Id}
                };

                var newToken = _token.Create(newPayload);
                var message = new JObject { new JProperty("Token", newToken) };
                var client = new HttpClient();
                var response = await client
                    .PostAsync(_options.Audience + "Token", new StringContent(message.ToString(), Encoding.UTF8, "application/json"));
                var responseToken = await response.Content.ReadAsStringAsync();
                JWT.Decode(responseToken, key);
                payload = JWT.Payload(responseToken);


            }
            catch (Exception e)
            {
                throw new ApplicationException("Error on authentication");
            }

            return View("SuccessLogin");
        }
    }
}
