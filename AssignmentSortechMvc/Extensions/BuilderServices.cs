using AssignmentSortechMvc.Service.Business;
using AssignmentSortechMvc.Service.Interfaces;

namespace AssignmentSortechMvc.Extensions
{
    public static class BuilderServices
    {
        public static void Service(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IEventService, EventService>();
            builder.Services.AddTransient<IOauthService, OauthService>();
        }
    }
}
