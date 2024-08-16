using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Web_Api___JWT.Security
{
    public static class TokenHandler
    {
        public static Token CreateToken (IConfiguration configuration)
        {
            Token token = new Token (); 
            SymmetricSecurityKey securitykey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"]));

            SigningCredentials credentials = new SigningCredentials(securitykey,
                SecurityAlgorithms.HmacSha256);

            token.Expiration = DateTime.Now.AddMinutes( Convert.ToInt16(configuration["Token:Expiration"]));

            JwtSecurityToken jwtsecuritytoken = new JwtSecurityToken(

                issuer: configuration["Token:Issuer"],
                audience: configuration["Token:Audience"],
                expires: token.Expiration,
                notBefore: DateTime.Now,
                signingCredentials: credentials



                );


            JwtSecurityTokenHandler tokenHandler = new();
            token.AccessToken = tokenHandler.WriteToken (jwtsecuritytoken);

            byte[] numbers = new byte[32];
            using RandomNumberGenerator random =  RandomNumberGenerator .Create ();
            random.GetBytes (numbers);
            token.RefreshToken = Convert.ToBase64String (numbers);

            return token;
        }
    }
}
