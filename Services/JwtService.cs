using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Web_Api.Interfaces;
using Web_Api.Models.AuthModels;
using Web_Api.Models.DbModels;

namespace Web_Api.Services
{
	public class JwtService : IJwt
	{
		private readonly JwtModel _jwtModel;
		private readonly IConfiguration _configuration;

		public JwtService(IConfiguration configuration, IOptions<JwtModel> jwtOptions)
		{
			_configuration = configuration;
			_jwtModel = jwtOptions.Value;
		}

		public string GenerateWebToken(User user)
		{
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, user.Id.ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtModel.SecretKey));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

			var token = new JwtSecurityToken(
				issuer: _jwtModel.Issuer,
				audience: _jwtModel.Audience,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(_jwtModel.ExpirationInMinutes),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}

