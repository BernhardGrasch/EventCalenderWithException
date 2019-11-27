using System;
using System.Collections.Generic;

namespace EventCalendar.Entities
{
    public class Event
    {
        public Person Invitor { get; set; }
        public DateTime DateTime { get; set; }
        public string Title { get; set; }
        public List<Person> Participators { get; set; }

        public Event(Person invitor, string title, DateTime dateTime)
        {
            Invitor = invitor;
            Title = title;
            DateTime = dateTime;
            Participators = new List<Person>();
        }

        public bool AddParticipatorToEvent(Person participator)
        {
            bool canBeAdded = false;

            if(!(Participators.Contains(participator)))
            {
                Participators.Add(participator);
                canBeAdded = true;
            }
           
            return canBeAdded;
        }

        public bool RemoveParticipatorFromEvent(Person participator)
        {
            bool canBeRemoved = false;

            if(Participators.Contains(participator))
            {
                Participators.Remove(participator);
                canBeRemoved = true;
            }

            return canBeRemoved;
        }

        public bool PersonInList(Person person)
        {
            return Participators.Contains(person);
        }

        public void CancelEvent()
        {
            throw new InvalidOperationException("The event is already canceled!");
        }
    }
}
