using MyDuoCards.Models.DBModels;
using System.Security.Claims;


namespace MyDuoCards.Models.Extensions
{
    static public class Cookies
    {
        public static ClaimsPrincipal ClaimCreator(this User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

    }
}
