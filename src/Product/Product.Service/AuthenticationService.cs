using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Product.Domain.DTO.Authentication;
using Product.Domain.Entities;
using Product.Domain.Exceptions;
using Product.Domain.Interfaces.Repositories;
using Product.Domain.Interfaces.Services;
using Product.Domain.Secutiry;
using Product.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Product.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly JwtSettings _jwt;
        private readonly IUserRepository _repository;

        public AuthenticationService(IConfiguration config, IUserRepository repository)
        {
            var jwtSection = config.GetSection(nameof(JwtSettings));
            _jwt = jwtSection.Get<JwtSettings>();
            _repository = repository;
        }

        public async Task<string> Authenticate(AuthenticationDTO dto)
        {
            //ywUB54Vih5gwAfVhHbEwVt73ZSjVnDvLbxo2EGaehjQv/n3R/TZOTVHhK8468Z8dnl3Tmb3I0uiT+ibj/RphIg==
            //ywUB54Vih5gwAfVhHbEwVqhAI85hRzeYyeo1yLyWIuk=

            var loginData = CryptoHelper.Decrypt(dto.Data, _jwt.SecKey, _jwt.IV).Split(':');
            if (loginData.Length < 2)
                throw new LoginException("Invalid login information");

            var user = await _repository.GetUser(loginData[0], loginData[1]);
            if (user is null)
                throw new LoginException("Email e/or Password invalid");

            return GenerateToken(user);
        }

        private string GenerateToken(User user)
        {
            var handler = new JwtSecurityTokenHandler();
            var secutiryToken = handler.CreateJwtSecurityToken(new SecurityTokenDescriptor
            {
                Subject = CreateClaims(user),
                Expires = DateTime.Now.AddMinutes(_jwt.LifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwt.Issuer,
                Audience = _jwt.Audience
            });
            secutiryToken.Payload["context"] = new JwtContextVO(user.Id, user.Email);
            return handler.WriteToken(secutiryToken);
        }

        private ClaimsIdentity CreateClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, $"{user.Id}"),
                new Claim(JwtRegisteredClaimNames.Aud, _jwt.Audience),
                new Claim(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.GivenName, user.Name),
            };

            return new ClaimsIdentity(claims, "bearer");
        }
    }
}
