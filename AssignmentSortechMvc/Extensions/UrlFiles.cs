using Newtonsoft.Json.Linq;

namespace AssignmentSortechMvc.Extensions
{
    public static class UrlFiles
    {
        public static string tokenFile = "F:\\Projects\\AssignmentSortechMvc\\AssignmentSortechMvc\\Files\\Token.json";
        public static string credentialsFile = "F:\\Projects\\AssignmentSortechMvc\\AssignmentSortechMvc\\Files\\credentialsInfo.json";
        public static string baseUrl = "https://www.googleapis.com/calendar/v3/calendars/primary/events";
        public static string gmailUrl = "https://www.googleapis.com/auth/gmail.send";
        public static string tokenUrl = "https://oauth2.googleapis.com/token";
        public static string redirectUrl = "https://localhost:7098/Oauth/Callback";
        public static string keyApi = "AIzaSyBW8N0M3BGdKO1XgYdL58XME6IHFqIeDo4";

        public static string tokens(string key)
        {
            var token = JObject.Parse(System.IO.File.ReadAllText(tokenFile));
            if (token != null)
            {
                return token[key].ToString();
            }

            return string.Empty;
        }

        public static string credentials(string key)
        {
            var credentials = JObject.Parse(System.IO.File.ReadAllText(credentialsFile));
            if (credentials != null)
            {
                return credentials[key].ToString();
            }

            return string.Empty;
        }
    }
}
