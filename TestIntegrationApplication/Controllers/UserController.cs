using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using TestIntegrationApplication.Models;
using TestIntegrationApplication.Services.Interfaces;

namespace TestIntegrationApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly AuthenticationRequestModel _options;
        private readonly IDistributedCache _cache;
        private readonly ITokenManager _tokenManager;
        public UserController(IOptions<AuthenticationRequestModel> options, IDistributedCache cache, ITokenManager tokenManager)
        {
            _options = options.Value;
            _cache = cache;
            _tokenManager = tokenManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = User.Identity.Name;
            var accessToken = await _cache.GetStringAsync(userId);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client
                .GetAsync(_options.Audience+"User/user");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await _tokenManager.DeactivateCurrentAsync();
                return StatusCode(401);
            }

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode(500);
            }
            var user =  JsonSerializer.Deserialize<Dictionary<string,object>>(await response.Content.ReadAsStringAsync()) ;

            return Json(user);
        }
    }
}