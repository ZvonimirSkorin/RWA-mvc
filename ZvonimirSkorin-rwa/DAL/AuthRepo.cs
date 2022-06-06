using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ZvonimirSkorin_rwa.DAL
{
    public class AuthRepo
    {

        private static readonly string key = "tajni kljucjknf kfjbs shbfjsb hbsjds b jhsb j sjnk kjds f";
        private static JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        private static byte[] tokenKey = Encoding.ASCII.GetBytes(key);


       
        public static string Authenticate(string username, string password)
        {
            if (!isPasswordValid(username, password)) return null;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, username), new Claim(ClaimTypes.Role, "Admin") }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static bool validateToken(string token)
        {
            if (token == null) return false;
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            if (jwtToken == null) throw new Exception("Ne valja token");
            string userId = (string)(jwtToken.Claims.First(x => x.Type == "unique_name").Value);
            if (userId != null)
            {
                return true;
            }
            else return false;
        }
        public static bool isPasswordValid(string username, string password)
        {

            using (var conn = DBRepo.getDB())
            {
                SqlDataAdapter da = new SqlDataAdapter(
                      $"select UserName, PasswordHash  from AspNetUsers" +
                      $" where UserName = '{username}' and PasswordHash = '{password}'; ",
                    conn);
                ;
                DataSet ds = new DataSet();
                da.Fill(ds, "users");

                var users = ds.Tables[0].Rows.Count;
                if (users == 0) return false;

            }
            return true;
        }
    }
}
