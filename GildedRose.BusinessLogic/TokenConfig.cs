using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace GildedRose.BusinessLogic
{
    public class TokenConfig
    {
        private const string KEY = "371697a9-35c5-4376-8158-3a45c7af4bf2";

        public const string ISSUER = "GildedRoseServer";
        public const int DAYS_LIFETIME = 30;
        public const string AUDIENCE = "http://localhost:54809";
        public static SymmetricSecurityKey SymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
