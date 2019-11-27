using System;
using System.Collections.Generic;
using System.Text;

namespace EventCalendar.Entities
{
    public class EventWithLimit : Event
    {
        public int MaxParticipators;

        public EventWithLimit(Person invitor, string title, DateTime dateTime, int maxParticipators)
            : base(invitor, title, dateTime)
        {
            MaxParticipators = maxParticipators;
        }
    }
}
