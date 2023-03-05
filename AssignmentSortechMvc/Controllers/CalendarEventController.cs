using AssignmentSortechMvc.Dto;
using AssignmentSortechMvc.Enum;
using AssignmentSortechMvc.Models;
using AssignmentSortechMvc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Ninject.Activation;
using RestSharp;

namespace AssignmentSortechMvc.Controllers
{
    public class CalendarEventController : Controller
    {
        private readonly IEventService _event;

        public CalendarEventController(IEventService eventService)
        {
            _event = eventService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateEvent(EventDto calendarEvent)
        {
            if(calendarEvent == null)
                return BadRequest();

            var Result = await _event.CreateEvent(calendarEvent);

            if(Result== ValidationResult.Success)
            {
                return RedirectToAction("GetAllEvent");
            }
            return StatusCode((int)Result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvent()
        {
            var Result = await _event.GetAllEvents();

            if (Result !=null || Result.Count()>0)
            {
                return View(Result);
            }
            return BadRequest();
        }

        public async Task<ActionResult> GetEventById(string idEvent)
        {
            if (string.IsNullOrWhiteSpace(idEvent))
                return BadRequest();

            var Result = await _event.GetEventById(idEvent);

            if (Result !=null)
            {
                return View(Result);
            }
            return BadRequest();
        }

        public async Task<ActionResult> DeleteEvent(string idEvent)
        {
            if (string.IsNullOrWhiteSpace(idEvent))
                return BadRequest();

            var Result = await _event.RemoveEvent(idEvent);

            if (Result == ValidationResult.Success)
            {
                return View(Result);
            }
            return BadRequest((int)Result);
        }
    }
}
