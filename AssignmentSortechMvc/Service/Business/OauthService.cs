using AssignmentSortechMvc.Enum;
using AssignmentSortechMvc.Extensions;
using AssignmentSortechMvc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AssignmentSortechMvc.Service.Business
{
    public class OauthService : IOauthService
    {
        public async Task<ValidationResult> GetToken(string code)
        {
            try
            {
                RestClient restClient = new RestClient();
                RestRequest Request = new RestRequest();

                Request.AddQueryParameter("client_id", UrlFiles.credentials("client_id"));
                Request.AddQueryParameter("client_secret", UrlFiles.credentials("client_secret"));
                Request.AddQueryParameter("code", code);
                Request.AddQueryParameter("grant_type", "authorization_code");
                Request.AddQueryParameter("redirect_uri", UrlFiles.redirectUrl);

                restClient.Options.BaseUrl = new System.Uri(UrlFiles.tokenUrl);

                var response = restClient.Post(Request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    System.IO.File.WriteAllText(UrlFiles.tokenFile, response.Content);

                    return ValidationResult.Success;
                }
                return ValidationResult.BadRequest;
            }
            catch
            {
                return ValidationResult.Exception;
            }
        }
        public async Task<ValidationResult> RefreshToken()
        {
            try
            {
                RestClient restClient = new RestClient();
                RestRequest Request = new RestRequest();

                Request.AddQueryParameter("client_id", UrlFiles.credentials("client_id"));
                Request.AddQueryParameter("client_secret",UrlFiles.credentials("client_secret"));
                Request.AddQueryParameter("grant_type", "refresh_token");
                Request.AddQueryParameter("refresh_token", UrlFiles.tokens("refresh_token"));

                restClient.Options.BaseUrl = new System.Uri(UrlFiles.tokenUrl);

                var response = restClient.Post(Request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var newToken = JObject.Parse(response.Content);
                    newToken["refresh_token"] = UrlFiles.tokens("refresh_token");
                    System.IO.File.WriteAllText(UrlFiles.tokenFile, newToken.ToString());
                    return ValidationResult.Success;
                }
                return ValidationResult.BadRequest;
            }
            catch
            {
                return ValidationResult.Exception;
            }
        }

        public async Task<ValidationResult> RemoveToken()
        {
            try
            {
                RestClient restClient = new RestClient();
                RestRequest Request = new RestRequest();

                Request.AddQueryParameter("token", UrlFiles.tokens("access_token"));

                restClient.Options.BaseUrl = new System.Uri("https://oauth2.googleapis.com/revoke");

                var response = restClient.Post(Request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return ValidationResult.Success;
                }
                return ValidationResult.BadRequest;
            }
            catch { return ValidationResult.Exception; }
        }

        public async Task<string> OauthRedirect()
        {
            var client_id = UrlFiles.credentials("client_id");
            var RedirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                             "scope=https://www.googleapis.com/auth/calendar+https://www.googleapis.com/auth/calendar.events&" +
                             "access_type=offline&" +
                             "include_granted_scopes=true&" +
                             "response_type=code&" +
                             "state=Ammar&" +
                             "redirect_uri=https://localhost:7098/Oauth/Callback&" +
                             "client_id=" + client_id;

            return RedirectUrl;
        }
    }
}
