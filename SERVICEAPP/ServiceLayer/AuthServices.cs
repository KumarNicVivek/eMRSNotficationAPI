using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CRUDENTITY.UOWRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SERVICEAPP.Utility;
//using SERVICEAPP.UtilityHelper;

namespace SERVICEAPP.ServiceLayer
{
    public class AuthServices : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IConfiguration _config;
        public AuthServices(IUnitOfWork unitOfWork, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _config = config;
        }

        public async Task<string?> AuthenticateandGenerateTokenAsync(string username, string EncryptUserId, string role)
        {

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT Key missing in configuration");

            var keyBytes = Convert.FromBase64String(jwtKey.Trim());

            var key = new SymmetricSecurityKey(keyBytes);

            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.NameIdentifier,EncryptUserId),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                            ClaimValueTypes.Integer64)
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"])),
                SigningCredentials = signCredential,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Simulate asynchronous behavior to resolve CS1998
            return await Task.FromResult(tokenString);


        }

        public async Task<string?> AuthenticateAsync(string username, string password,string role="Admin")
        {
            if (username != "admin" || password != "password" || string.IsNullOrEmpty(role))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtKey = _config["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT Key missing in configuration");

            var keyBytes = Convert.FromBase64String(jwtKey.Trim());

            var key = new SymmetricSecurityKey(keyBytes);

            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiryMinutes"])),
                SigningCredentials = signCredential,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // Simulate asynchronous behavior to resolve CS1998
            return await Task.FromResult(tokenString);
        }

        private bool VerifyClientCombinedHash(string strReceivedSaltHash, string strReceivedPasswordHash, string sessionSalt)
        {

            if (string.IsNullOrEmpty(strReceivedSaltHash) || string.IsNullOrEmpty(strReceivedPasswordHash) || string.IsNullOrEmpty(sessionSalt))
            {
                return false;
            }

            // 1. Recompute SHA512(salt)
            string sha512Salt = PasswordHashing.ComputeSHA512(sessionSalt); // returns 128-char hex string

            // 2. Extract parts
            string receivedSaltHash = strReceivedSaltHash;
            string receivedPasswordHash = strReceivedPasswordHash;
            //string saltval = strSaltval;

            // 3. Validate salt
            if (receivedSaltHash != sha512Salt)
                return false;

            // 4. Now you can compare receivedPasswordHash with stored SHA512(password)

            return true;
        }


    }
}
