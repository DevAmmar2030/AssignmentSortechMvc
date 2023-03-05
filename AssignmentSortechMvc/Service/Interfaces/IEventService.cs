using AssignmentSortechMvc.Dto;
using AssignmentSortechMvc.Enum;
using AssignmentSortechMvc.Models;

namespace AssignmentSortechMvc.Service.Interfaces
{
    public interface IEventService
    {
        Task<ValidationResult> CreateEvent(EventDto calendarEvent);
        Task<List<EventDto>> GetAllEvents();
        Task<EventDto> GetEventById(string id);
        Task<ValidationResult> RemoveEvent(string id);
    }
}
