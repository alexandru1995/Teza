using Jose;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestIntegrationApplication.Helpers;
using TestIntegrationApplication.Models;

namespace TestIntegrationApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly IJwtToken _token;
        private readonly AuthenticationRequestModel _options;
        private readonly IDistributedCache _cache;
        public HomeController(IJwtToken token, IOptions<AuthenticationRequestModel> options, IDistributedCache cache)
        {
            _token = token;
            _options = options.Value;
            _cache = cache;

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

        public IActionResult SuccessLogin()
        {
            
            return View();
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
              
                JWT.Decode(responseToken, Encoding.ASCII.GetBytes(_options.ServerSecret),JwsAlgorithm.HS256);
                payload = JWT.Payload(responseToken);
                var tokens = JsonSerializer.Deserialize<AuthorizationResponseModel>(payload);
                await _cache.RemoveAsync(tokens.UserId);
                await _cache.SetStringAsync(tokens.UserId, tokens.AccessToken);
                ViewData["IdToken"] = token;
                return View("SuccessLogin");
            }
            catch
            {
                throw new ApplicationException("Error on authentication");
            }
        }
    }
}
