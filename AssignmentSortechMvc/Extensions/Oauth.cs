using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace AssignmentSortechMvc.Extensions
{
    public class Oauth
    {
        public void Callback(string code, string error, string state)
        {
            if (string.IsNullOrWhiteSpace(error))
            {
                this.GetToken(code);
            }
        }

        public void GetToken(string code)
        {
            var tokenFile = "F:\\Projects\\AssignmentSortechMvc\\AssignmentSortechMvc\\Files\\Token.json";
            var credentialsFile = "F:\\Projects\\AssignmentSortechMvc\\AssignmentSortechMvc\\Files\\credentialsInfo.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));

            RestClient restClient = new RestClient();
            RestRequest Request = new RestRequest();

            Request.AddQueryParameter("client_id", credentials["client_id"].ToString());
            Request.AddQueryParameter("client_secret", credentials["client_secret"].ToString());
            Request.AddQueryParameter("code", code);
            Request.AddQueryParameter("grant_type", "authorization_code");
            //Request.AddQueryParameter("redirect", "");

            restClient.Options.BaseUrl = new System.Uri("https://accounts.google.com/o/oauth2/auth");

            var response = restClient.Post(Request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                System.IO.File.WriteAllText(tokenFile, response.Content);
            }
        }

        public void OauthRedirect()
        {
            var credentialsFile = "F:\\Projects\\AssignmentSortech\\AssignmentSortech\\Files\\credentialsInfo.json";
            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));

            var client_id = credentials["client_id"];
            var RedirectUrl = "https://accounts.google.com/o/oauth2/v2/auth?" +
                             "scope=https%3A//www.googleapis.com/auth/drive.metadata.readonly&" +
                             "access_type=online&" +
                             "include_granted_scopes=true&" +
                             "response_type=code&" +
                             "state=Ammar&" +
                             "redirect_uri=https%3A//oauth2.example.com/code&" +
                             "client_id=" + client_id;
        }
    }
}
