using System.IdentityModel.Tokens.Jwt;

namespace UserRegistration.Application.Helpers
{
    public static class JwtHelper
    {
        public static string GetClaimIdUserFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            foreach (var claim in jwtToken.Claims)
            {
                if (claim.Type == "nameid")
                {
                    return claim.Value;
                }
            }

            return null;
        }

        public static List<int> GetProfilesFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return null;

            var roleClaims = jwtToken.Claims
                .Where(c => c.Type == "role")
                .Select(c => int.Parse(c.Value))
                .ToList();

            return roleClaims;
        }
    }
}
