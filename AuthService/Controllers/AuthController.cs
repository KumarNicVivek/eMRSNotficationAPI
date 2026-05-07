using AuthService.CommonModel;
using AuthService.Models;
using CRUDENTITY.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Org.BouncyCastle.Asn1.Ocsp;
using SERVICEAPP.ServiceLayer;
using static Org.BouncyCastle.Math.EC.ECCurve;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IMemoryCache _memoryCache;
        private readonly ICaptchaService _captchaService;

        public AuthController(IConfiguration configuration, IUserService userService, IAuthService authService, IMemoryCache memoryCache,
            ICaptchaService captchaService  )
        {
            _configuration = configuration;
            _userService = userService;
            _authService = authService;
            _memoryCache = memoryCache;
            _captchaService = captchaService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestModel loginReqModel)
        {
            bool isValidUser = false;
            string receivedPasswordHash = string.Empty;

            try
            {
                if(loginReqModel == null || string.IsNullOrEmpty(loginReqModel.username) || string.IsNullOrEmpty(loginReqModel.Password))
                {
                    return BadRequest(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Invalid Request.",
                        Data = null
                    });
                }

                var cacheKey = $"LOGIN_NONCE_{loginReqModel.username.ToLower()}";

                if (!_memoryCache.TryGetValue(cacheKey, out string? nonce))
                {
                    return Unauthorized(new ApiResponse<string>
                    {
                        Success = false,
                        Message = "Nonce not found or expired. Please retry login.",
                        Data = null
                    });
                }

                string strMessage = string.Empty;

                if(!string.IsNullOrEmpty(loginReqModel.username) && !string.IsNullOrEmpty(loginReqModel.Password))
                {
                    var user = _userService.GerUserWithRoleByEmailId(loginReqModel.username);
                    if (user != null)
                    {
                        if (!string.IsNullOrEmpty(loginReqModel.Password))
                        {
                            //receivedSaltHash = request.Password.Substring(0, 128);
                            receivedPasswordHash = loginReqModel.Password.Substring(0, 128);
                        }
                        else
                        {
                            strMessage = "Please provide Proper Data.";
                            return BadRequest(strMessage);
                        }

                        var isVerifyPasswod = _userService.getSHA512CombinedashPassWordandSalt(loginReqModel.Password, nonce,receivedPasswordHash);

                        if (isVerifyPasswod)
                        {
                            isValidUser = true;
                            var accesstokenVal = await _authService.AuthenticateandGenerateTokenAsync
                                                (loginReqModel.username,
                                                 user.EncryptCode,user.RoleId.ToString());

                            // Generate refresh token
                            var refreshToken = Guid.NewGuid().ToString();   /*_authService.GenerateRefreshToken();*/


                            // Save refresh token for user in DB and access  after succesfull login in RefreshToken Action
                            //user.RefreshToken = refreshToken;
                            //user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                            //await _userService.UpdateAsync(user);


                            var permissions = _userService.GetPermissionsForUser(user.EncryptCode);

                            // Send refresh token in response header only if authentication is successful

                            if (permissions == null)
                            {
                                return Unauthorized(new ApiResponse<string>
                                {
                                    Success = false,
                                    Message = "There is some issue with user.",
                                    Data = null
                                });
                            }

                            var cookieOptions = new CookieOptions
                            {
                                HttpOnly = true,
                                //Secure = true, // Uncomment if using HTTPS
                                //SameSite = SameSiteMode.Strict, // Adjust based on your requirements
                                Expires = DateTime.UtcNow.AddDays(7) // Set appropriate expiration
                            };

                            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);

                            if (string.IsNullOrEmpty(accesstokenVal))
                            {
                                return StatusCode(500, new ApiResponse<string>
                                {
                                    Success = false,
                                    Message = "Login failed. Please try again.",
                                    Data = null
                                });
                            }

                            return Ok(new ApiResponse<LoginResponseModel>
                            {
                                Success = true,
                                Message = "Login successful.",
                                Data = new LoginResponseModel
                                {
                                    token = accesstokenVal,
                                    Role = user.RoleId.ToString(),
                                    Permissions = permissions
                                }
                            });
                        }
                        else
                        {
                            strMessage = "Invalid username or password.";
                            return Unauthorized(new ApiResponse<string>
                            {
                                Success = false,
                                Message = strMessage,
                                Data = null
                            });
                        }
                    }
                }

                return Ok(new { string.Empty });
            }
            catch(Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                isValidUser = false;
                //Console.WriteLine($"An error occurred during login: {ex.Message}");
                return StatusCode(500, new ApiResponse<string>
                {
                    Success = false,
                    Message = "An error occurred while processing your request.",
                    Data = null
                });
            }
            

        }

        [HttpGet("loginnonce")]
        public IActionResult GetLoginNonce(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required.");
            }

            // Generate a unique nonce (you can use a GUID or any other method)
            var nonce = Guid.NewGuid().ToString("N");

            var cacheKey = $"LOGIN_NONCE_{username.ToLower()}";

            // Store the nonce in memory cache with an expiration time (e.g., 5 minutes)
            _memoryCache.Set(cacheKey, nonce, TimeSpan.FromMinutes(5));

            return Ok(new { Nonce = nonce });
        }

        [HttpPost("refreshtok")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized();
            }
            //Validate the refresh token from DB or Cache
            var user = _userService.GetUserBySecretCode(refreshToken);
            // if (user == null)
            // {
            //     return Unauthorized();
            // }

            // Generate new Access Token
            //var newAccessToken = JwtTokenHelper.GenerateToken(
            //    _config,
            //    "vivek20feb@gmail.com",
            //    "District Nodal Officer"  // for Entry level Role
            //    );

            var refaccesstokenVal = await _authService.AuthenticateandGenerateTokenAsync
                                                ("vivek20feb@gmail.com",
                                                 user.EncryptCode, user.RoleId.ToString());


            return Ok(new { token = refaccesstokenVal });

        }

        [HttpGet]
        
        //public IActionResult RefreshCaptchaByteArray()
        //{
        //    string captchaText;
            
        //    string captchaKey = Guid.NewGuid().ToString();

          

            
        //}

        [HttpGet("refreshCaptcha")]
        [AllowAnonymous]
        public IActionResult RefreshCaptcha()
        {
            string captchaText;
            string relativePath = _captchaService.GenerateCaptchaImage(out captchaText);

            // Convert "~/captchas/xyz.png" to "/captchas/xyz.png"
            string imagePath = relativePath.Replace("~", "");

            // Generate unique key
            string captchaKey = Guid.NewGuid().ToString();

            // Store CAPTCHA for 5 minutes
            _memoryCache.Set(captchaKey, captchaText, TimeSpan.FromMinutes(5));

            // Build absolute URL
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            //string baseUrl = _configuration["AppSettings:BaseUrl"] ?? $"{Request.Scheme}://{Request.Host}";
            string imageUrl = $"{baseUrl}{imagePath}";

            return Ok(new
            {
                captchaKey,
                imageUrl
            });
        }
    }
}
