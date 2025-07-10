using localMarketingSystem.BAL.Interfaces;
using localMarketingSystem.DAL.Entities;
using localMarketingSystem.DTOs;
using localMarketingSystem.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace localMarketingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config, IUserService userService)
        {
            _userService = userService;
            _config = config;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<APIResponseHelper<string>> Login(LoginDTO loginDTO)
        {
            APIResponseHelper<string> response = new();
            try
            {
                MUser userDetails = await _userService.UserDeatilsByEmail(loginDTO.Email);
                if (userDetails == null)
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.Message = "Username or Password does not exists !!!";
                    return response;
                }
                if (PasswordHelper.VerifyPasswordHash(loginDTO.Password, userDetails.PasswordHash, userDetails.PasswordSalt))
                {
                    string roleName = await _userService.GetRoleByUserId(userDetails.UserId);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userDetails.UserId.ToString()),
                        new Claim(ClaimTypes.Name, userDetails.UserName.ToString()),
                         new Claim(ClaimTypes.Role, roleName),
                    };
                    //var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Auth:TokenKey").Value));
                    //var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                    //var tokeOptions = new JwtSecurityToken(
                    //    issuer: _config.GetSection("Auth:Issuer").Value,
                    //    audience: _config.GetSection("Auth:Audience").Value,
                    //    claims: claims,
                    //    expires: DateTime.Now.AddMinutes(30),
                    //    signingCredentials: signinCredentials
                    //);
                    //var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                    string tokenString = Generate(claims);
                    response.apiResponseStatus = Enum.APIResponseStatus.Success;
                    response.Message = "Login Successful.";
                    response.result = tokenString;
                    return response;
                }
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Login failed, please try again..";
                return response;
            } catch (Exception ex) 
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.Message = "Error " + ex;
                return response;
            }
        }
        private string Generate(List<Claim> claims)
        {
            var secretKey = Encoding.UTF8.GetBytes("DIag0Js8c0k3Ajw3R19kxBsmNqSJFEYf"); // longer that 16 character
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes("V3Znp4JRTGVWiuGv"); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            // var claims = _getClaims(user);

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _config.GetSection("Auth:Issuer").Value,
                Audience = _config.GetSection("Auth:Audience").Value,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now.AddMinutes(0),
                Expires = DateTime.Now.AddMinutes(60),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var securityToken = tokenHandler.CreateToken(descriptor);

            var encryptedJwt = tokenHandler.WriteToken(securityToken);

            return encryptedJwt;
        }
    }
}
