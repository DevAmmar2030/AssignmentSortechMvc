using AssignmentSortechMvc.Models;
using AssignmentSortechMvc.Service.Interfaces;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using RestSharp;
using AssignmentSortechMvc.Enum;
using AssignmentSortechMvc.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AssignmentSortechMvc.Extensions;

namespace AssignmentSortechMvc.Service.Business
{
    public class EventService : IEventService
    {
        public async Task<ValidationResult> CreateEvent(EventDto calendarEvent)
        {
            try
            {
                RestClient restClient = new RestClient();
                RestRequest Request = new RestRequest();

                var newEvent = await GenrateEvent(calendarEvent);

                var model = JsonConvert.SerializeObject(newEvent, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

                Request.AddQueryParameter("key", UrlFiles.keyApi);
                Request.AddHeader("Authorization", "Bearer " + UrlFiles.tokens("access_token"));
                Request.AddHeader("Accept", "application/json");
                Request.AddHeader("Content-Type", "application/json");
                Request.AddParameter("application/json", model, ParameterType.RequestBody);

                restClient.Options.BaseUrl = new System.Uri(UrlFiles.baseUrl);

                var response = restClient.Post(Request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SentEmail();
                    return ValidationResult.Success;
                }
            }
            catch
            {
                return ValidationResult.Exception;
            }
            return ValidationResult.RequestFailed;
        }

        public async Task<List<EventDto>> GetAllEvents()
        {
            RestClient restClient = new RestClient();
            RestRequest Request = new RestRequest();

            Request.AddQueryParameter("key", UrlFiles.keyApi);
            Request.AddHeader("Authorization", "Bearer " + UrlFiles.tokens("access_token"));
            Request.AddHeader("Accept", "application/json");

            restClient.Options.BaseUrl = new System.Uri(UrlFiles.baseUrl);

            var response = restClient.Get(Request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calendarEvent = JObject.Parse(response.Content);
                var allEvents = calendarEvent["items"].ToObject<IEnumerable<Event>>();

                var dto = new List<EventDto>();
                foreach (var item in allEvents)
                {
                    dto.Add(
                        new EventDto
                        {
                            Id = item.Id,
                            Summary = item.Summary,
                            Description = item.Description,
                            StartDateTime = item.Start.DateTime.ToString(),
                            EndDateTime = item.End.DateTime.ToString()
                        });
                }
                return dto;
            }
            return null;
        }

        public async Task<EventDto> GetEventById(string id)
        {
            RestClient restClient = new RestClient();
            RestRequest Request = new RestRequest();

            Request.AddQueryParameter("key", UrlFiles.keyApi);
            Request.AddHeader("Authorization", "Bearer " + UrlFiles.tokens("access_token"));
            Request.AddHeader("Accept", "application/json");

            restClient.Options.BaseUrl = new System.Uri(UrlFiles.baseUrl + "/" + id);

            var response = restClient.Get(Request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JObject calendarEvent = JObject.Parse(response.Content);
                var result = calendarEvent.ToObject<Event>();
                if (result != null)
                {
                    return new EventDto
                    {
                        Id=result.Id,
                        Summary = result.Summary,
                        Description = result.Description,
                        StartDateTime = result.Start.DateTime.ToString(),
                        EndDateTime = result.End.DateTime.ToString(),

                    };
                }
                return null;
            }
            return null;
        }

        public async Task<ValidationResult> RemoveEvent(string id)
        {
            try
            {
                RestClient restClient = new RestClient();
                RestRequest Request = new RestRequest();

                Request.AddQueryParameter("key", UrlFiles.keyApi);
                Request.AddHeader("Authorization", "Bearer " + UrlFiles.tokens("access_token"));
                Request.AddHeader("Accept", "application/json");

                restClient.Options.BaseUrl = new System.Uri(UrlFiles.baseUrl + "/" + id);

                var response = restClient.Delete(Request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return ValidationResult.Success;
                }
                return ValidationResult.RequestFailed;
            }

            catch { return ValidationResult.Exception; }
        }

        #region Helper Method
        private async Task<Event> GenrateEvent(EventDto _eventDto)
        {
            try
            {
                //TimeZone curTimeZone = TimeZone.CurrentTimeZone;

                Event newEvent = new Event()
                {
                    Summary = _eventDto.Summary,
                    Description = _eventDto.Description,
                    Start = new EventDateTime()
                    {
                        DateTime = DateTime.Parse(_eventDto.StartDateTime),
                        TimeZone = "America/Los_Angeles",//curTimeZone.StandardName,
                    },
                    End = new EventDateTime()
                    {
                        DateTime = DateTime.Parse(_eventDto.EndDateTime),
                        TimeZone = "America/Los_Angeles", //curTimeZone.StandardName,
                    },
                    SendNotifications = true,
                };

                return newEvent;
            }
            catch
            {
                return null;
            }
        }

        private void SentEmail()
        {
            var to = new { email = "devammarahmed@gmail.com‏" };
            var from = new { email = "testprojectammar@gmail.com‏" };
            var args = new
            {
                from = from,
                to = new[] { to },
                subject = "Create Event",
                text = "Create Event Create Event Create Event!",
            };

            var client = new RestClient(UrlFiles.gmailUrl);
            var request = new RestRequest("/send", RestSharp.Method.Post);

            var model = JsonConvert.SerializeObject(args, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            request.AddHeader("Authorization", "Bearer " + UrlFiles.tokens("access_token"));
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", model, ParameterType.RequestBody);

            RestResponse response = client.Execute(request);

            Console.WriteLine(response.Content);

        }
        #endregion
    }
}
