using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XExten.XPlus;

namespace Mily.Extension.Authentication
{
    public class JsonWebToken
    {
        public Guid KeyId { get; set; }
        public Guid RoleId { get; set; }
        public string UserName { get; set; }
        public string UserRole { get; set; }

        public static string InitToken(Action<JsonWebToken> Action)
        {
            JsonWebToken Author = new JsonWebToken();
            Action(Author);
            return XPlusEx.XTry(() => Author.GetAuthorToken(), (Ex) => Ex.Message);
        }
        private string GetAuthorToken()
        {
            Claim[] Claims = new Claim[]
            {
             new Claim("KeyId",KeyId.ToString()),
             new Claim("RoleId",RoleId.ToString()),
              new Claim("UserName",UserName),
             new Claim("UserRole",UserRole)
            };
            SymmetricSecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("EmilyEdnaMilyMicro"));
            SigningCredentials Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken Token = new JwtSecurityToken(null, null, Claims, DateTime.UtcNow.AddHours(8), DateTime.UtcNow.AddHours(11), Credentials);
           return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
