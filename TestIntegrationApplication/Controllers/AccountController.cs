using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using TestIntegrationApplication.Helpers;
using TestIntegrationApplication.Models;
using TestIntegrationApplication.Services.Interfaces;

namespace TestIntegrationApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IJwtToken _token;
        private readonly AuthenticationRequestModel _options;
        private readonly IDistributedCache _cache;
        private readonly ITokenManager _tokenManager;
        public AccountController(IJwtToken token, IOptions<AuthenticationRequestModel> options, IDistributedCache cache, ITokenManager tokenManager)
        {
            _token = token;
            _options = options.Value;
            _cache = cache;
            _tokenManager = tokenManager;
        }
        public IActionResult Login()
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

        [HttpPost]
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

                JWT.Decode(responseToken, Encoding.ASCII.GetBytes(_options.ServerSecret), JwsAlgorithm.HS256);
                payload = JWT.Payload(responseToken);
                var tokens = JsonSerializer.Deserialize<AuthorizationResponseModel>(payload);
                await _cache.RemoveAsync(tokens.UserId);
                await _cache.SetStringAsync(tokens.UserId, tokens.AccessToken);
                return RedirectToAction("SuccessLogin","Home", new {token = tokens.IdToken});
            }
            catch
            {
                throw new ApplicationException("Error on authentication");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> OnLogout()
        {
            
            var client = new HttpClient();
            var userId = User.Identity.Name;
            var accessToken = await _cache.GetStringAsync(userId);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client
                .GetAsync(_options.Audience + "Token/signout");

            await _tokenManager.DeactivateCurrentAsync();
            return StatusCode(200);
        }
    }
}