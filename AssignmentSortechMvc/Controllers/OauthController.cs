using AssignmentSortechMvc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AssignmentSortechMvc.Controllers
{
    public class OauthController : Controller
    {
        private readonly IOauthService _oauth;

        public OauthController(IOauthService oauth)
        {
            _oauth = oauth;
        }

        public async void Callback(string code, string error, string state)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                if (!string.IsNullOrWhiteSpace(code))
                    await _oauth.GetToken(code);
            }
        }

        public async Task<IActionResult> RefreshToken()
        {
            var result = await _oauth.RefreshToken();
            return Content(result.ToString());
        }

        public async Task<IActionResult> RemoveToken()
        {
            var result = await _oauth.RemoveToken();
            return Content(result.ToString());
        }

        public async Task<IActionResult> OauthRedirect()
        {
            var url = await _oauth.OauthRedirect();
            return Redirect(url);
        }
    }
}
