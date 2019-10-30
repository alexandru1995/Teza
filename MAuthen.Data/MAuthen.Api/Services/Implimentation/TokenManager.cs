using MAuthen.Api.Models.Authentication;
using MAuthen.Api.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MAuthen.Api.Services.Implimentation
{
    public class TokenManager : ITokenManager
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<JwtOptions> _jwtOptions;

        public TokenManager(IDistributedCache cach,
            IHttpContextAccessor httpContextAccessor,
            IOptions<JwtOptions> jwtOptions)
        {
            _cache = cach;
            _httpContextAccessor = httpContextAccessor;
            _jwtOptions = jwtOptions;
        }

        public async Task<bool> IsCurrentActiveToken()
            => await IsActiveAsync(GetCurrentAsync());

        public async Task DeactivateCurrentAsync()
            => await DeactivateAsync(GetCurrentAsync());

        public async Task<bool> IsActiveAsync(string token)
            => await _cache.GetStringAsync(GetKey(token)) == null;


        public async Task DeactivateAsync(string token)
            => await _cache.SetStringAsync(GetKey(token), " ", new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtOptions.Value.ExpiryMinutes)
            });


        private string GetCurrentAsync()
        {
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();
            //var token = _httpContextAccessor.HttpContext.Request.Cookies["Token"];
            //return token == StringValues.Empty ?
            //        string.Empty : token;

        }
        private static string GetKey(string token)
            => $"tokens: {token} : deactivated";
    }
}
