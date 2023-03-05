function addEvent() {
            var event = {
                "EventTitle": "Event Title",
                "Start.DateTime": "10-10-2023 11:30 am",
                "End.DateTime": '10-11-2023 11:30 am',
            };

            $.ajax({
                type: "POST",
                url: "/CalendarEvent/CreateEvent",
                data: JSON.stringify(event),
                datatype: "json",
                contentType: "application/json; charset=utf-8",
                success: function (r) {
                    
                }
            });
        }
      